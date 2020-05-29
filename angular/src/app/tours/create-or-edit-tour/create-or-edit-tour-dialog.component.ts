import { Component, Injector, Optional, Inject, OnInit, ElementRef, ViewChild } from "@angular/core";
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material";
import { finalize } from "rxjs/operators";
import * as _ from "lodash";
import { AppComponentBase } from "@shared/app-component-base";
import {
  CreateOrEditTourDto,
  TourServiceProxy,
  GetTourForEditOutput,
  TourDateDto,
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";

@Component({
  templateUrl: "./create-or-edit-tour-dialog.component.html",
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
export class CreateOrEditTourDialogComponent extends AppComponentBase
  implements OnInit {
  public saving = false;
  public tour: GetTourForEditOutput = new GetTourForEditOutput();
  public tourDates: TourDateDto[] = [];
  public startDate: moment.Moment;
  public startTime: string;
  public endDate: moment.Moment;
  public endTime: string;
  public title: string = "Create New Tour";

  @ViewChild('imageUpload', { static: false }) imageUpload: ElementRef;

  constructor(
    injector: Injector,
    private _tourService: TourServiceProxy,
    private _dialogRef: MatDialogRef<CreateOrEditTourDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number,
    private _httpClient: HttpClient
  ) {
    super(injector);
    this.tour.tour = new CreateOrEditTourDto();
    this.tour.tourPictures = [];
  }

  public getTour(): void {
    this._tourService.getTourForEdit(this._id).subscribe((result) => {
      this.tour = result;
      this.tourDates = this.tour.tourDates;
    });
  }

  public addDate(): void {
    const newDate = new TourDateDto();
    let startTime = new Time(this.startTime);  // this.startTime is the value gathered by the form field
    newDate.startDate = moment(this.startDate).set({minutes: startTime.minutes, hours: startTime.hours});
    let endTime = new Time(this.endTime);
    newDate.endDate = moment(this.endDate).set({minutes: endTime.minutes, hours: endTime.hours});
    if (this._id) newDate.tourId = this._id;
    this.tourDates.push(newDate);
  }

  public deleteDate(dateNumber: number): void {
    this.tourDates = this.tourDates.filter(date => date.id != dateNumber);
  }

  public uploadTourPicture(event: any): void {
    this.notify.info("Uploading picture...");

    const formData: FormData = new FormData();
    const file = event.target.files[0];
    formData.append("file", file, file.name);

    let url_ =
      AppConsts.remoteServiceBaseUrl +
      "/api/services/app/Tour/UploadTourPicture";

    if (this._id) url_ += "?TourId=" + this._id;

    this._httpClient
      .post<any>(url_, formData)
      .pipe(finalize(() => {
          this.imageUpload.nativeElement.value = null;
      }))
      .subscribe((response) => {
        if (response.success) {
          this.tour.tourPictures.push(response.result);
          this.notify.info("Picture has been uploaded");
        } else if (response.error != null) {
          this.notify.error("Error uploading picture");
        }
      });
  }

  public deleteTourPicture(tourPictureId: number) {
    this._tourService.deleteTourPicture(tourPictureId).subscribe(() => {
      this.tour.tourPictures = this.tour.tourPictures.filter(e => e.id !== tourPictureId);
      this.notify.info("Picture has been deleted");
    })
  }

  public deleteTour() {
    this._tourService.delete(this._id).subscribe(() => {
      this.notify.info("Tour has been deleted");
      this.close(true);
    })
  }

  ngOnInit(): void {
    if (this._id) {
      this.title = "Edit Tour";
      this.getTour();
    }
  }

  save(): void {
    this.saving = true;

    this._tourService
      .createOrEdit(this.tour.tour)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(result => {

        if (!this._id) {
          for (let tourPicture of this.tour.tourPictures) {
            tourPicture.tourId = result.id;
            this._tourService.updateTourPicture(tourPicture).subscribe(() => {});
          }
        }

        for (let tourDate of this.tourDates) {
          if (!tourDate.tourId) tourDate.tourId = result.id;
          var timezoneOffset = moment(tourDate.startDate).utcOffset();
          if (timezoneOffset != 0) {
            console.log("Modifying date time according to timezone offset of " + (timezoneOffset / 60) + " hours");
            tourDate.startDate = moment(tourDate.startDate).set({"minutes": moment(tourDate.startDate).get("minutes") + timezoneOffset});
            tourDate.endDate = moment(tourDate.endDate).set({"minutes": moment(tourDate.endDate).get("minutes") + timezoneOffset});
          }
          this._tourService.createOrEditTourDate(tourDate).subscribe(() => {})
        }

        this.notify.info(this.l("SavedSuccessfully"));
        this.close(true);
      });


  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}


class Time {
  public minutes: number;
  public hours: number;
  constructor(timeString: string) {
    this.minutes = Number(timeString.slice(3));
    this.hours = Number(timeString.slice(0, 2));
  }
}
