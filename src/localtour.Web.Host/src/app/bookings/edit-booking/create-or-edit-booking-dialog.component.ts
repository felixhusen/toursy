import {
  Component,
  Injector,
  Optional,
  Inject,
  OnInit,
  ElementRef,
  ViewChild,
} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { finalize } from "rxjs/operators";
import * as _ from "lodash";
import { AppComponentBase } from "@shared/app-component-base";
import {
  CreateOrEditBookingDto,
  BookingServiceProxy,
  GetBookingForEditOutput,
  TourServiceProxy,
  GetTourForViewDto,
  TourDateDto,
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";

@Component({
  templateUrl: "./create-or-edit-booking-dialog.component.html",
  styles: [
    `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
    `,
  ],
})
export class CreateOrEditBookingDialogComponent extends AppComponentBase
  implements OnInit {
  public saving = false;
  public booking: GetBookingForEditOutput = new GetBookingForEditOutput();
  public title: string = "Create New Booking";
  public tour: GetTourForViewDto;
  public tourDates: TourDateDto[];
  public states: { id: number; code: string }[] = [
    {
      id: 1,
      code: "QLD",
    },
    {
      id: 2,
      code: "ACT",
    },
    {
      id: 3,
      code: "TAS",
    },
    {
      id: 4,
      code: "NT",
    },
    {
      id: 5,
      code: "SA",
    },
    {
      id: 6,
      code: "WA",
    },
    {
      id: 7,
      code: "VIC",
    },
    {
      id: 8,
      code: "NSW",
    },
  ];

  @ViewChild("imageUpload", { static: false }) imageUpload: ElementRef;

  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy,
    private _dialogRef: MatDialogRef<CreateOrEditBookingDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public _id: number,
    private _httpClient: HttpClient,
    private _tourService: TourServiceProxy
  ) {
    super(injector);
    this.booking.booking = new CreateOrEditBookingDto();
  }

  public getBooking(): void {
    this._bookingService.getBookingForEdit(this._id).subscribe((result) => {
      this.booking = result;
      console.log(this.booking);
      this.getTour(this.booking.booking.tourId);
    });
  }

  public deleteBooking() {
    this._bookingService.delete(this._id).subscribe(() => {
      this.notify.info("Booking has been deleted");
      this.close(true);
    });
  }

  public async approveBooking(id: number) {
    if (confirm("Are you sure to approve this booking?")) {
      await this._bookingService.approveBooking(id).toPromise();
      this.close(true);
    }
  }

  public async approveBookingCancellation(id: number) {
    if (confirm("Are you sure to approve cancellation of this booking?")) {
      await this._bookingService.approveBookingCancellation(id).toPromise();
      this.close(true);
    }
  }

  public async requestCancelBooking(id: number) {
    if (confirm("Are you sure to cancel this booking?")) {
      await this._bookingService.requestCancelBooking(id).toPromise();
      this.close(true);
    }
  }

  public getTour(id: number): void {
    this._tourService.getTourForView(id).subscribe((result) => {
      this.tour = result;
      this.tourDates = result.tourDates;
    });
  }

  ngOnInit(): void {
    if (this._id) {
      this.title = "Edit Booking";
      this.getBooking();
    }
  }

  save(): void {
    this.saving = true;
    console.log(this.booking);
    this._bookingService
      .createOrEdit(this.booking.booking)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((result) => {
        this.notify.info(this.l("SavedSuccessfully"));
        this.close(true);
      });
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
