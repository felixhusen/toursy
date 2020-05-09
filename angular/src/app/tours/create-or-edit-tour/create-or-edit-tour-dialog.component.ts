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
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";

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
  saving = false;
  tour: GetTourForEditOutput = new GetTourForEditOutput();

  title: string = "Create New Tour";

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
    });
  }

  public uploadTourPicture(event: any): void {
    this.notify.info("Uploading picture...");

    const formData: FormData = new FormData();
    const file = event.target.files[0];
    formData.append("file", file, file.name);

    let url_ =
      AppConsts.remoteServiceBaseUrl +
      "/api/services/app/Tour/UploadTourPicture";

    if (this._id) url_ = "?TourId=" + this._id;

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

        this.notify.info(this.l("SavedSuccessfully"));
        this.close(true);
      });

      
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
