import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Course, TeeSheetLockLine } from 'src/app/booking_online/model/model';
import { CourseService } from 'src/app/booking_online/service/course.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Constant, DefaultLockTeeSheet } from 'src/app/common/enum';

@Component({
  selector: 'app-dialog-tee-sheet-lock-line',
  templateUrl: './dialog-tee-sheet-lock-line.component.html',
  styleUrls: ['./dialog-tee-sheet-lock-line.component.scss']
})
export class DialogTeeSheetLockLineComponent extends  BaseComponent implements OnInit {

  teeSheetLockLine: TeeSheetLockLine;

  listCourse : Course[] = []
  courseSelectControl = new FormControl()
  courseFilteredOptions: Observable<Course[]>

  listFlight = []
  listDayOfWeek = []

  rangeLinedow = [0,1,2]
  constructor(
    public dialogRef: MatDialogRef<DialogTeeSheetLockLineComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private courseSv: CourseService,
    public dialog: MatDialog
  ) {
    super(dialog);
    this.teeSheetLockLine = data || new TeeSheetLockLine();
    this.teeSheetLockLine.startTee = this.teeSheetLockLine.startTee || DefaultLockTeeSheet.startTee
    this.teeSheetLockLine.startTime = this.teeSheetLockLine.startTime || DefaultLockTeeSheet.time
    this.teeSheetLockLine.endTime = this.teeSheetLockLine.endTime || DefaultLockTeeSheet.time
    this.teeSheetLockLine.flightSeq = this.teeSheetLockLine.flightSeq || DefaultLockTeeSheet.flight
    this.teeSheetLockLine.dow = this.teeSheetLockLine.dow || DefaultLockTeeSheet.dow
  }

  async ngOnInit() {
    this.initial();
    this.tranformData();  
    
  }

  async initial() {
    var c_Org_Id = this.teeSheetLockLine.c_Org_Id;
    this.listCourse = (await this.courseSv.getByOrg(c_Org_Id))?.data || [];
    this.courseSelectControl.setValue(this.listCourse.find(x => x.id == this.teeSheetLockLine.c_Course_Id));
    this.courseFilteredOptions = this.courseSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterCourse(value))
    )
  }

  tranformData() {
     Constant.Flight.forEach(x => {
        this.listFlight.push({
          value: x.value, name: x.name, checked: true
        })
     })

     Constant.DayOfWeek.forEach(x => {
      this.listDayOfWeek.push({
        value: x.value, name: x.name, checked: true
      })
     })
     let flight = this.teeSheetLockLine.flightSeq.split("");
     this.listFlight.forEach(x => x.checked = flight.includes(x.value.toString()))
     let dow = this.teeSheetLockLine.dow.split("");
     this.listDayOfWeek.forEach(x => x.checked = dow.includes(x.value.toString()))
  }


  private _filterCourse(value: string): Course[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listCourse.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedCourse(event) {
  }
  changeCourseText() {
    this.teeSheetLockLine.c_Course_Id = this.courseSelectControl.value?.id || null
  }
  
  displayCourse(course?: Course): string {
    return course?.name || ''
  }
  validate() {
    if ((this.teeSheetLockLine.startTime || DefaultLockTeeSheet.time) >= (this.teeSheetLockLine.endTime || DefaultLockTeeSheet.time)) {
      this.alert("Thời gian bắt đầu phải trước thời gian kết thúc")
      return false
    }
    
    if (this.teeSheetLockLine.flightSeq === ""){
      this.alert("Chưa chọn flight")
      return false
    }

    if (this.teeSheetLockLine.dow === ""){
      this.alert("Chưa chọn ngày áp dụng")
      return false
    }
    return true
  }

  save() {
    this.teeSheetLockLine.flightSeq = this.listFlight.filter(x => x.checked).map(x => x.value).join("");
    this.teeSheetLockLine.dow = this.listDayOfWeek.filter(x => x.checked).map(x => x.value).sort((a,b)=> a-b).join(""); 
    this.teeSheetLockLine.courseName = this.listCourse.find(x => x.id == this.teeSheetLockLine.c_Course_Id)?.name;
    this.teeSheetLockLine.startTee = 1;
    if (this.validate()) {
      this.dialogRef.close(this.teeSheetLockLine);
    }
  }

}
