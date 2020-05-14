import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  BookingServiceProxy,
  GetBookingForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";

@Component({
  templateUrl: "./bookings.component.html",
  styleUrls: ["../app.component.less", "./bookings.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class BookingsComponent extends AppComponentBase implements OnInit {
  public searchQuery: string;
  public bookings: GetBookingForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 5;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [5, 10, 25, 100];
  public pageEvent: PageEvent;
  public displayedColumns: string[] = ['booking.tourId', 'booking.userId', 'booking.address', 'booking.promoCode', 'booking.numberOfPeople', 'booking.totalPrice'];

  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy
  ) {
    super(injector);
  }

  public getBookings(event?: any): void {
    if (event) {
      this.pageEvent = event;
      this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
      this.maxResultCount = this.pageEvent.pageSize;
    }

    this._bookingService
      .getAll(this.searchQuery, this.sort, this.skipCount, this.maxResultCount)
      .subscribe((result) => {
        this.bookings = result.items;
        this.totalCount = result.totalCount;
        console.log("Result")
        console.log(this.bookings)
      });
  }

  public requestCancelBooking(id: number) {
    const booking = this.bookings.find(e => e.booking.id == id);
  }

  public paginate(event: any) {
    this.maxResultCount = event.rows;
    this.skipCount = this.maxResultCount * event.page;
    console.log("Skip Count: " + this.skipCount);
    console.log(event);
    this.getBookings();
  }

  ngOnInit(): void {
    this.getBookings();
  }
}
