import { Component, Injector, Optional, Inject, OnInit, ElementRef, ViewChild } from "@angular/core";
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material";
import { finalize } from "rxjs/operators";
import * as _ from "lodash";
import { AppComponentBase } from "@shared/app-component-base";
import {
  CreateOrEditRequestDto,
  RequestServiceProxy,
  GetRequestForEditOutput,
  BookingServiceProxy,
  GetBookingForViewDto,
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";

@Component({
  templateUrl: "./create-or-edit-request-dialog.component.html",
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
export class CreateOrEditRequestDialogComponent extends AppComponentBase
  implements OnInit {
  public saving = false;
  public request: GetRequestForEditOutput = new GetRequestForEditOutput();
  public title: string = "Create New Request";
  public bookings: GetBookingForViewDto[];

  constructor(
    injector: Injector,
    private _requestService: RequestServiceProxy,
    private _dialogRef: MatDialogRef<CreateOrEditRequestDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public _id: number,
    private _bookingService: BookingServiceProxy,
    private _httpClient: HttpClient
  ) {
    super(injector);
    this.request.request = new CreateOrEditRequestDto();
  }

  public getRequest(): void {
    this._requestService.getRequestForEdit(this._id).subscribe((result) => {
      this.request = result;
    });
  }

  public getBookings(): void {
    this._bookingService
      .getAll(undefined, undefined, undefined, undefined, undefined)
      .subscribe((result) => {
        this.bookings = result.items;
      });
  }

  public deleteRequest() {
    this._requestService.delete(this._id).subscribe(() => {
      this.notify.info("Request has been deleted");
      this.close(true);
    })
  }

  ngOnInit(): void {
    if (this._id) {
      this.title = "Edit Request";
      this.getRequest();
    }
    this.getBookings();
  }

  save(): void {
    this.saving = true;
    const booking = this.bookings.find(booking => booking.booking.id == this.request.request.bookingId);
    this.request.request.tourId = booking.booking.tourId;
    console.log(this.request.request);
    this._requestService
      .createOrEdit(this.request.request)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(result => {

        this.notify.info(this.l("SavedSuccessfully"));
        this.close(true);
      });

      
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
