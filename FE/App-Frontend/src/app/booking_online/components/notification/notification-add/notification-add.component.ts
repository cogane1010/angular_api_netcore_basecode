import { Component, OnInit, ViewChild, ChangeDetectorRef,ElementRef } from '@angular/core';
import { PromotionSettingFilterModel } from '../../../model/paging';
import { GF_Notification, Organization, BodyData} from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { NotificationService } from '../../../service/notification.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';
import { MatTable } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { CourseService } from '../../../service/course.service';
import { DialogSendNotificattionComponent } from '../dialog-send-notificattion/dialog-send-notificattion.component'

@Component({
   selector: 'app-notification-add',
   templateUrl: './notification-add.component.html',
   styleUrls: ['./notification-add.component.scss']
})

export class NotificationAddComponent extends BaseComponent implements OnInit {

  id : string;
  model: GF_Notification = {};
  listOrganization : Organization[] = [];
  isReadonly = false;
  
  defaultImage: any = 'url("assets/imgs/portrait-icon.jpg")';
  imageUrl: any = this.defaultImage;
  imageData: any;
  imageType: string;
  base64textString: any;
  @ViewChild('fileAvaInput') fileAvaInput: ElementRef;
  filePath: string;
  fileToUpload: File

  constructor(
    public dialog: MatDialog, 
    protected _route: ActivatedRoute,
    protected _router: Router,
    private notificationService: NotificationService,
    private organizationSv: OrganizationService,
    private courseSv: CourseService,
    private _sanitizer: DomSanitizer,
    private cd: ChangeDetectorRef
    

  ) {
    super(dialog);
    this._route.params.subscribe(params => {
      this.id = params['id'] 
    });
  }

  async ngOnInit() {
    
    if(this.id === "add") {

    }
    else {
        this.loadData();
    }  
  }

  loadData() {
    this.notificationService.get(this.id).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res.message)
          return
        }
        else {
          this.model = res.data;         
          if(this.model.imageData){
            this.imageUrl = this._sanitizer.bypassSecurityTrustStyle(`url(${this.model.imageData})`);
          } 
         
        }
      },
      err => {
          this.alert('error');
          console.log(err);
      }
    );
  }
  

  back() {
    this._router.navigateByUrl('/booking/notification');
  }


  
  addOrEdit(promotion: GF_Notification) {
    
    this.notificationService.addOrEdit(promotion).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res?.message)
          return
        }
        //this.promotion = res.data
        this._router.navigateByUrl('/booking/notification');
        this.alert('Cập nhật thành công')
      },
      err => {
        console.log(err);
      }
    )
  }

  sendNotification(notification: GF_Notification) {

    //var data: BodyData = {title: notification.message_Title, body: notification.message_Content, imgUrl: notification.img_FullUrl}
    const dialogRef = this.dialog.open(DialogSendNotificattionComponent, {
      width: '20vw',
      data: notification
      })

      dialogRef.afterClosed().subscribe(res => {
          //if (res) this.getData()
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
    this.model.imageData = this.imageType + ',' + this.base64textString;
  }

  removeUploadedAvaFile() {
    this.imageUrl =  this._sanitizer.bypassSecurityTrustStyle(`${this.defaultImage}`);
    this.model.imageData = ""
    this.fileAvaInput.nativeElement.value = "";
  }

 
  
  
  
}

