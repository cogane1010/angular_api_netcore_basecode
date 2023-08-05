import { Component, OnInit, ViewChild, ChangeDetectorRef,ElementRef } from '@angular/core';
import { M_Promotion, Organization, Course,CustomerGroup } from '../../../model/model';
import { MatDialog } from '@angular/material/dialog';
import { PromotionSettingComponentService } from '../../../service/PromotionSettingListComponent.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { CourseService } from '../../../service/course.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';
import { MatTable } from '@angular/material/table';

@Component({
  selector: 'app-promotion-setting-add',
  templateUrl: './promotion-setting-add.component.html',
  styleUrls: ['./promotion-setting-add.component.scss']
})

export class PromotionSettingAddComponent extends BaseComponent implements OnInit {

  id : string;
  promotion: M_Promotion = {};
  listOrganization : Organization[] = [];
  isReadonly = false;
  listCourse : Course[] = [];
  listCustomer : CustomerGroup[] = [];

  defaultImage: any = 'url("assets/imgs/portrait-icon.jpg")';
  imageUrl: any = this.defaultImage;
  imageUrl1: any = this.defaultImage;
  imageData: any;
  imageType: string;
  base64textString: any;
  @ViewChild('fileAvaInput') fileAvaInput: ElementRef;
  @ViewChild('fileAvaInput1') fileAvaInput1: ElementRef;
  filePath: string;
  fileToUpload: File

  constructor(
    public dialog: MatDialog, 
    protected _route: ActivatedRoute,
    protected _router: Router,
    private promotionService: PromotionSettingComponentService,
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
    this.listCourse = (await this.courseSv.getCourseByUser()).data || [];  
    this.listCustomer =  (await this.promotionService.getCustomers()).data || []; 
    if(this.id === "add") {
      this.promotion.isActive = true
    }
    else {
        this.loadData();
    }  
  }

  loadData() {
    this.promotionService.get(this.id).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res.message)
          return
        }
        else {
          this.promotion = res.data;
          if(this.promotion.imageData){
            this.imageUrl = this._sanitizer.bypassSecurityTrustStyle(`url(${this.promotion.imageData})`);
          } 
          if(this.promotion.imageDataEn){
            this.imageUrl1 = this._sanitizer.bypassSecurityTrustStyle(`url(${this.promotion.imageDataEn})`);
          } 
          this.promotion.promotionCourses.forEach(e => {
            var courID = e.c_Course_Id;
            this.listCourse.forEach(c => {
              if(c.id == courID){
                c.selected = true;
              }
            })
          })
          this.promotion.promotionCustomerGroup.forEach(e => {
            var cusGr = e.mB_CustomerGroup_Id;
            this.listCustomer.forEach(c => {
              if(c.id == cusGr){
                c.selected = true;
              }
            })
          })
        }
      },
      err => {
          this.alert('error');
          console.log(err);
      }
    );
  }
  

  back() {
    this._router.navigateByUrl('/booking/promotion-setting');
  }

  addOrEdit(promotion: M_Promotion) {
    if(!this.promotion.start_Date || !this.promotion.end_Date){
      this.alert('Chọn ngày bắt đầu hoặc ngày kết thúc')
      return
    }
    promotion.customerGroups = this.listCustomer.filter(x => x.selected)
    promotion.course = this.listCourse.filter(x => x.selected)
    promotion.promotion_Type = 'normal'

    this.promotionService.addOrEdit(promotion).then(
      res => {
        if (!res?.isSuccess) {
          this.alert('Cập nhật thất bại')
          return
        }
        //this.promotion = res.data
        this._router.navigateByUrl('/booking/promotion-setting');
        this.alert('Cập nhật thành công')
      },
      err => {
        console.log(err);
      }
    )
  }


  uploadAvaFile(event, typeImg: string) {
    let reader = new FileReader();
    let file = event.target.files[0];
    //this.fileToUpload = file;
    if (event.target.files && event.target.files[0]) {
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.imageType = reader.result.toString().split(',')[0];
        if(typeImg == 'en'){
          this.imageUrl1 = this._sanitizer.bypassSecurityTrustStyle(`url(${reader.result})`);
        }else{
          this.imageUrl = this._sanitizer.bypassSecurityTrustStyle(`url(${reader.result})`);
        }        
      }      
      var reader1 = new FileReader();
      reader1.onload =this._handleReaderLoaded.bind(this,typeImg);
      reader1.readAsBinaryString(file);
      this.cd.markForCheck();
    }
  }
  _handleReaderLoaded(typeImg: string, readerEvt ) {
    var binaryString = readerEvt.target.result;
    this.base64textString= btoa(binaryString);    
    if(typeImg == 'en'){
      this.promotion.imageDataEn = this.imageType + ',' + this.base64textString;
    }else{
      this.promotion.imageData = this.imageType + ',' + this.base64textString;
    }  
  }

  removeUploadedAvaFile(typeImg: string) {
    //this.imageUrl =  this._sanitizer.bypassSecurityTrustStyle(`${this.defaultImage}`);
    if(typeImg == 'en'){
      this.imageUrl1 =  '';
      this.promotion.imageDataEn = '';
      this.promotion.img_Url_En = '';
      this.fileAvaInput1.nativeElement.value = "";
    }else{
      this.imageUrl =  '';
      this.promotion.imageData = '';
      this.promotion.img_Url = '';
      this.fileAvaInput.nativeElement.value = "";
    } 
  }

 
  onChangeToDate(idDate: number){
    var fDate = new Date(this.promotion.start_Date);
    var tDate = new Date(this.promotion.end_Date);
    console.log(fDate.getTime());
    console.log(tDate.getTime());

    if(fDate && tDate && 
      fDate.getTime() > tDate.getTime()){
      if(idDate == 1){
        this.promotion.start_Date = null;
      }else{
        this.promotion.end_Date = null;
      }
      alert('Ngày bắt đầu phải sớm hơn ngày ');
    }
  }

  onChangeTime(idTime: number){
    var now = new Date();
    var fToday: Date;
    var tToday: Date;
    if(this.promotion.applyTime_From){
      var fdate = this.promotion.applyTime_From.split(':');
      fToday = new Date(now.getFullYear(), now.getMonth(), now.getDate(), Number(fdate[0]), Number(fdate[1]), 0);
    }
        
    if(this.promotion.applyTime_To){
      var tdate = this.promotion.applyTime_To.split(':');
      tToday = new Date(now.getFullYear(), now.getMonth(), now.getDate(), Number(tdate[0]), Number(tdate[1]), 0);
    }
    if(fToday && tToday && fToday.getTime() >= tToday.getTime()){
      if(idTime){
        this.promotion.applyTime_From = '';
      }else{
        this.promotion.applyTime_To = '';
      }
      alert('Thời gian bắt đầu phải sớm hơn ngày kết thúc');
    }
  }

  
  
}

