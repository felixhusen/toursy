import { Component, Injector, Optional, Inject, OnInit, ElementRef, ViewChild } from "@angular/core";
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material";
import { finalize } from "rxjs/operators";
import * as _ from "lodash";
import { AppComponentBase } from "@shared/app-component-base";
import {
  CreateOrEditDisputeDto,
  DisputeServiceProxy,
  GetDisputeForEditOutput,
  BookingServiceProxy,
  GetBookingForViewDto,
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";

@Component({
  templateUrl: "./create-or-edit-dispute-dialog.component.html",
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
export class CreateOrEditDisputeDialogComponent extends AppComponentBase
  implements OnInit {
  public saving = false;
  public dispute: GetDisputeForEditOutput = new GetDisputeForEditOutput();
  public title: string = "Create New Dispute";
  public bookings: GetBookingForViewDto[];

  constructor(
    injector: Injector,
    private _disputeService: DisputeServiceProxy,
    private _dialogRef: MatDialogRef<CreateOrEditDisputeDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number,
    private _bookingService: BookingServiceProxy,
    private _httpClient: HttpClient
  ) {
    super(injector);
    this.dispute.dispute = new CreateOrEditDisputeDto();
  }

  public getDispute(): void {
    this._disputeService.getDisputeForEdit(this._id).subscribe((result) => {
      this.dispute = result;
    });
  }

  public getBookings(): void {
    this._bookingService
      .getAll(undefined, undefined, undefined, undefined)
      .subscribe((result) => {
        this.bookings = result.items;
      });
  }

  public deleteDispute() {
    this._disputeService.delete(this._id).subscribe(() => {
      this.notify.info("Dispute has been deleted");
      this.close(true);
    })
  }

  ngOnInit(): void {
    if (this._id) {
      this.title = "Edit Dispute";
      this.getDispute();
    }
    this.getBookings();
  }

  save(): void {
    this.saving = true;
    console.log(this.dispute.dispute);
    this._disputeService
      .createOrEdit(this.dispute.dispute)
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
