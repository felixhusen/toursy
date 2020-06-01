import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TourServiceProxy,
  GetTourForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { MatDialog } from "@angular/material";
import { CreateOrEditTourDialogComponent } from "./create-or-edit-tour/create-or-edit-tour-dialog.component";
import { AppSessionService } from "@shared/session/app-session.service";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
  templateUrl: "./tours.component.html",
  styleUrls: ["../app.component.less", "./tours.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class ToursComponent extends AppComponentBase implements OnInit {
  public nameFilter: string;
  public minPriceFilter: number;
  public maxPriceFilter: number;
  public descriptionFilter: string;
  public startDateFilter: moment.Moment;
  public endDateFilter: moment.Moment;
  public locationNameFilter: string;
  public userIdFilter: number;
  public tours: GetTourForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 6;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [6, 12, 24, 96];
  public pageEvent: PageEvent;
  public title: string = "All Tours";
  public defaultImageLink: string =
    "https://attendantdesign.com/wp-content/uploads/2017/08/tour-1-1.jpg";
  public userId: number;
  private showingAdvanced: boolean = false;
  public loading: boolean = false;

  constructor(
    injector: Injector,
    private _tourService: TourServiceProxy,
    private _appSessionService: AppSessionService,
    private _fileDownloadService: FileDownloadService,
    private _dialog: MatDialog
  ) {
    super(injector);
  }

  public getTours(event?: any): void {
    this.loading = true;
    if (event) {
      this.pageEvent = event;
      this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
      this.maxResultCount = this.pageEvent.pageSize;
    }


    /* The specification for TourServiceProxy.getAll
      /**
       * @param name (optional)
       * @param minPrice (optional)
       * @param maxPrice (optional)
       * @param description (optional)
       * @param startDate (optional)
       * @param endDate (optional)
       * @param longitude (optional)
       * @param latitude (optional)
       * @param radius (optional)
       * @param accommodation (optional)
       * @param style (optional)
       * @param userId (optional)
       * @param sorting (optional)
       * @param skipCount (optional)
       * @param maxResultCount (optional)
       * @return Success
       */

    this._tourService
      .getAll(
        this.nameFilter,
        this.minPriceFilter,
        this.maxPriceFilter,
        this.descriptionFilter,
        this.startDateFilter,
        this.endDateFilter,
        this.locationNameFilter,
        this.userIdFilter,
        this.sort,
        this.skipCount,
        this.maxResultCount
      )
      .subscribe((result) => {
        this.tours = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      });
  }

  public exportToExcel(): void {

    this._tourService
      .getToursToExcel(
        this.nameFilter,
        this.minPriceFilter,
        this.maxPriceFilter,
        this.descriptionFilter,
        this.startDateFilter,
        this.endDateFilter,
        this.locationNameFilter,
        this.userIdFilter,
        this.sort,
        this.skipCount,
        this.maxResultCount
      )
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }

  public createTour(): void {
    this.showCreateOrEditTourDialog();
  }

  public editTour(id: number): void {
    this.showCreateOrEditTourDialog(id);
  }

  public toggleMyTours(): void {
    this.title = "My Tours";
    this.userIdFilter = this._appSessionService.userId;
    this.getTours();
  }

  public toggleAllTours(): void {
    this.title = "All Tours";
    this.userIdFilter = undefined;
    this.getTours();
  }

  private showCreateOrEditTourDialog(id?: number): void {
    let createOrEditUserDialog;
    if (id === undefined || id <= 0) {
      createOrEditUserDialog = this._dialog.open(CreateOrEditTourDialogComponent);
    } else {
      createOrEditUserDialog = this._dialog.open(CreateOrEditTourDialogComponent, {
          data: id
      });
    }

    createOrEditUserDialog.afterClosed().subscribe((result) => {
      if (result) {
        this.getTours();
      }
    });
  }

  ngOnInit(): void {
    this.userId = this._appSessionService.user.id;
    this.getTours();
  }
}
