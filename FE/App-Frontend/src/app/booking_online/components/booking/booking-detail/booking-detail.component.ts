import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Booking, BookingOtherType, BookingSpecialRequest } from 'src/app/booking_online/model/model';
import { BookingService } from 'src/app/booking_online/service/booking.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-booking-detail',
  templateUrl: './booking-detail.component.html',
  styleUrls: ['./booking-detail.component.scss']
})
export class BookingDetailComponent extends BaseComponent implements OnInit {

  id : string;
  listBookingOther: BookingOtherType[] = []
  booking: Booking = new Booking();
  totalAmount: number;
  depositAmount: number;
  paymentType = "SeABank"

  constructor(
    public dialogRef: MatDialogRef<BookingDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string, 
    private bookingSv: BookingService,
    protected _route: ActivatedRoute,
    //protected _router: Router,
    public dialog: MatDialog
  ) {
    super(dialog);
    this.dialogRef.disableClose = true
    this.id = data || ''
    this.booking.bookingSpecialRequests = []
    this.booking.bookingTeetime = []
  }

  async ngOnInit() {
    // this._route.params.subscribe(params => {
    //   this.id = params['id'] 
    // });
      
    await this.loadData();
  
 
  }
  

  async loadData () {
     this.booking = (await this.bookingSv.get(this.id))?.data;
     this.totalAmount = this.booking.nonRefundedFee;
     this.depositAmount = this.booking.totalEstimateFee;
     console.log(this.booking.bookingSpecialRequests)
  }




  back(){
    
  }

}

