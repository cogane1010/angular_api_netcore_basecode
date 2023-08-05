import { HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Customer, MemberCard } from 'src/app/booking_online/model/model';
import { CustomerService } from 'src/app/booking_online/service/customer.service';
import { MemberCardService } from 'src/app/booking_online/service/member-card.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { UploadSizeMax } from 'src/app/common/enum';
import { DialogMemberCardListComponent } from '../dialog-member-card-list/dialog-member-card-list.component';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent extends BaseComponent implements OnInit {

  id : string;
  customer: Customer;
  oldInfo: Customer;

  avatarFiles: File;
  avatarProgress: string;
  avatarDefault: any = "assets/image/noavatar.png"
  avatarUrl: any = "assets/image/noavatar.png";
  isUploaded = true;

  defaultImage: any = 'url("assets/imgs/portrait-icon.jpg")';
  imageUrl: any = this.defaultImage;
  imageData: any;
  imageType: string;
  base64textString: any;
  @ViewChild('fileAvaInput') fileAvaInput: ElementRef;
  filePath: string;
  fileToUpload: File



  phonePattern = "[0-9]{8,14}";
  emailPattern = "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";

  extensionFileAccept = ["png","jpg", "jpeg"]
  minDate: Date;
  maxDate: Date;

  isNewCustomer = true;

  isReadonly = false;
  constructor(
    public dialog: MatDialog, 
    protected _route: ActivatedRoute,
    protected _router: Router,
    private customerService: CustomerService,
    private memberCardSv: MemberCardService,
    private _sanitizer: DomSanitizer,
    private cd: ChangeDetectorRef
  ) {
    super(dialog);
    this.setDateValid();
  }

  setDateValid() {
    this.maxDate = new Date();
    this.minDate = new Date();
    this.minDate.setFullYear(this.minDate.getFullYear() -100);
  }

  async ngOnInit() {
    this.customer = new Customer();

    this._route.params.subscribe(params => {
      this.id = params['id'] 
    });

    if(this.id === "add") {
       this.customer.isActive = true       
    }
    else {
       this.loadData();
    }
  }

  loadData() {
    this.customerService.get(this.id).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res.message)
          return
        }
        else {
          this.customer = res.data;
          this.oldInfo = res.data;
          this.isNewCustomer = false;
          if(this.customer.imageData){
            this.imageUrl = this._sanitizer.bypassSecurityTrustStyle(`url(${this.customer.imageData})`);
          } 
          if(this.customer.avatarFileId) {
            this.getFile()
          }
          this.processMemberCard();
        }
      },
      err => {
          this.alert('error');
          console.log(err);
      }
    );
  }

  processMemberCard() {
    if(this.customer.memberCards) {
      if (this.customer.memberCards.length) {
        this.customer.memberCards.forEach( x => {
          if(x.coursesMemberCard && x.coursesMemberCard.length) {

          }
          else {
            x.coursesMemberCard = [{
              mC_MemberCard_Id: x.id,
              validFrom: x.golf_Effective_Date,
              validTo: x.golf_Expire_Date,
              courseName: x.orgName,
              customerGroupName: x.customerGroupName,
            }]
          }
        }) 
      }
    }
    else {
      this.customer.memberCards = []
    }
  }
  

  fileProgressAvatar(fileInput) {
    var extension = fileInput.target.files[0].name.split('.').pop().toLowerCase(); 
    if (!this.extensionFileAccept.includes(extension)) {
      this.error("Chỉ được upload file .jpg, .jpeg, .png");
      return
    }
    if (fileInput.target.files[0].size > UploadSizeMax.size){
      this.error("Upload thất bại! File upload không được vượt "+ UploadSizeMax.title+"MB");
      return;
    }
    
    this.isUploaded = false;
    this.avatarFiles = fileInput.target.files[0];
    this.preview();
  }

  fileInputClick(elementId: any) {
    var file_input = document.getElementById(elementId);
    file_input.click();
    
  }

  preview() {
    if (this.avatarFiles) {
        const mimeType = this.avatarFiles.type;
        if (mimeType.match(/image\/*/) == null) {
            return;
        }

        const reader = new FileReader();
        reader.readAsDataURL(this.avatarFiles);
        reader.onload = (_event) => {
            this.avatarUrl = reader.result;
        };
    } else {
        this.avatarUrl = "assets/image/noavatar.png";
    }
  }

  uploadAvatar() {
    const formData = new FormData();
    formData.append(this.avatarFiles.name, this.avatarFiles, this.avatarFiles.name);
    if(this.avatarFiles)
    this.customerService.uploadFile(formData).subscribe(events => {
        if (events.type === HttpEventType.UploadProgress) {
            this.avatarProgress = Math.round(events.loaded / events.total * 100) + '%';
        } else if (events.type === HttpEventType.Response) {
            this.avatarProgress = '';
            this.isUploaded = true;
            this.alert('Upload thành công');
            if (events.body) {
                const body = events.body as any;
                this.customer.avatarFileId = body.data.id;
                this.customer.img_Url = body.data.filePath;
                this.getFile();
            }
        }
      }
    );
  }

  getFile() {
    const fileId =  this.customer.avatarFileId;
    this.customerService.downloadFile(fileId).then(
        (res: any) => {
            this.createImageFromBlob(res);
        }
        , error => {
            if (error.error) {
                AppGlobals.arrayBufferToString(error.error, 'UTF-8', console.log.bind(console));
            }
        }
    );
  }


  createImageFromBlob(image: any) {
      if(image) {
          const contentType = 'application/octet-stream';
          const blob = new Blob([image], { type: contentType });
          const reader = new FileReader();
          reader.addEventListener('load', () => {
              this.avatarUrl = this._sanitizer.bypassSecurityTrustUrl(reader.result.toString());
          }, false);
          if (blob) {
              reader.readAsDataURL(blob);
          }
      }
      else {
          this.avatarUrl = this.avatarDefault
      }
    
  }

  removeAvatar() {
    this.avatarUrl = this.avatarDefault
    this.isUploaded = false;
  }
  async update() { 
   await this.customerService.addOrEdit(this.customer).then(
      res => {
         if (!res?.isSuccess) {
          this.alert(res.message)
          return
         }
          this.alert('Cập nhật thành công');
          // tao moi
          if (res.data) {
              this.id = res.data.id;
          }
          this.loadData();
          this._router.navigate(['/booking/customer', this.id]);
      },
      error => {
          console.log(error);
          this.error(error);
      })
  }
  back() {
    this._router.navigateByUrl('/booking/customer');
  }

  openPopupMemberCard(item) {
    const dialogRef = this.dialog.open(DialogMemberCardListComponent, {
      width: '80vw',
      data: Object.assign({}, this.customer)
    })
    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.loadData()
       
    })
  }

  async resetPassword() {
    if (!(await this.confirm('Reset password?'))) return;
    this.customerService.resetPassword(this.customer).then(
      res => {
         if (!res?.isSuccess) {
          this.alert(res.message)
          return
         }
          this.alert('Cập nhật thành công');
      },
      error => {
          console.log(error);
          this.error(error);
      })
  }

   refreshMemberCard(memberCard: MemberCard) {
    memberCard.mB_Customer_Id = this.customer.id;
     this.memberCardSv.refresh(memberCard).then(
      res => {
         if (!res?.isSuccess) {
          this.alert(res.message)
          return
         }
         this.loadData();
         this.alert('Cập nhật thành công');
      },
      error => {
          console.log(error);
          this.error(error);
      })
   }

   async reassignMemberCard(memberCard: MemberCard) {
      if (!(await this.confirm('Gỡ thẻ hội viên?'))) return
      this.memberCardSv.reassign(memberCard).then(
        res => {
           if (!res?.isSuccess) {
            this.alert(res.message)
            return
           } 
           this.loadData();
           this.alert('Cập nhật thành công');
        },
        error => {
            console.log(error);
            this.error(error);
        })
   }

   uploadAvaFile(event) {
    let reader = new FileReader();
    let file = event.target.files[0];
    //this.fileToUpload = file;
    if (event.target.files && event.target.files[0]) {
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.imageType = reader.result.toString().split(',')[0];
        console.log(this.imageType)
        this.imageUrl = this._sanitizer.bypassSecurityTrustStyle(`url(${reader.result})`);
      }      
      var reader1 = new FileReader();
      reader1.onload =this._handleReaderLoaded.bind(this);
      reader1.readAsBinaryString(file);
      this.cd.markForCheck();
    }
  }
  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.base64textString= btoa(binaryString);
    this.customer.imageData = this.imageType + ',' + this.base64textString;
  }

  removeUploadedAvaFile() {
    //this.imageUrl =  this._sanitizer.bypassSecurityTrustStyle(`${this.defaultImage}`);
    this.imageUrl =  '';
    //this.promotion.imageData = '';
    //this.promotion.img_Url = '';
    this.fileAvaInput.nativeElement.value = "";
  }


}
