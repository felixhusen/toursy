import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  BookingServiceProxy,
  GetBookingForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { CreateOrEditBookingDialogComponent } from "./edit-booking/create-or-edit-booking-dialog.component";
import { MatDialog } from "@angular/material";

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
  public maxResultCount: number = 25;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [5, 10, 25, 100];
  public pageEvent: PageEvent;
  public title: string = "My Bookings";
  public mode: string;
  public loading: boolean = false;

  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _dialog: MatDialog
  ) {
    super(injector);
  }

  public getBookings(event?: any): void {
    this.loading = true;
    if (event) {
      this.pageEvent = event;
      this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
      this.maxResultCount = this.pageEvent.pageSize;
    }

    this._bookingService
      .getAll(
        this.searchQuery,
        this.mode,
        this.sort,
        this.skipCount,
        this.maxResultCount
      )
      .subscribe((result) => {
        this.bookings = result.items;
        this.totalCount = result.totalCount;
        console.log("Result");
        console.log(this.bookings);
        this.loading = false;
      });
  }

  public exportToExcel(): void {
    this._bookingService
      .getBookingsToExcel(
        this.searchQuery,
        this.mode,
        undefined,
        undefined,
        undefined
      )
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }

  private showCreateOrEditBookingDialog(id?: number): void {
    let createOrEditUserDialog;
    if (id === undefined || id <= 0) {
      createOrEditUserDialog = this._dialog.open(
        CreateOrEditBookingDialogComponent
      );
    } else {
      createOrEditUserDialog = this._dialog.open(
        CreateOrEditBookingDialogComponent,
        {
          data: id,
        }
      );
    }

    createOrEditUserDialog.afterClosed().subscribe((result) => {
      if (result) {
        this.getBookings();
      }
    });
  }

  public toggleMyBooking() {
    this.title = "My Bookings";
    this.mode = undefined;
    this.getBookings();
  }

  public toggleCustomerBooking() {
    this.title = "Customer's Bookings";
    this.mode = "CustomerBookings";
    this.getBookings();
  }

  public toggleCancellationRequests() {
    this.title = "Booking Cancellation Requests";
    this.mode = "CancellationRequests";
    this.getBookings();
  }

  public togglePendingRequests() {
    this.title = "Pending Requests";
    this.mode = "PendingRequests";
    this.getBookings();
  }

  public async requestCancelBooking(id: number) {
    if (confirm("Are you sure to cancel this booking?")) {
      await this._bookingService.requestCancelBooking(id).toPromise();
      this.getBookings();
    }
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
