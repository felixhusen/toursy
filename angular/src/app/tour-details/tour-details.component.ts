import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TourServiceProxy,
  GetTourForViewDto
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";

@Component({
  templateUrl: "./tours.component.html",
  styleUrls: ['../app.component.less', './tours.component.less'],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})

export class ToursComponent extends AppComponentBase implements OnInit {
  public nameFilter: string;
  public priceFilter: number;
  public descriptionFilter: string;
  public startDateFilter: moment.Moment;
  public endDateFilter: moment.Moment;
  public longitudeFilter: string;
  public latitudeFilter: string;
  public tours: GetTourForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 5;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;
  public defaultImageLink: string = "https://attendantdesign.com/wp-content/uploads/2017/08/tour-1-1.jpg";

  constructor(injector: Injector, private _tourService: TourServiceProxy) {
    super(injector);
  }

  public getTours(event?: any): void {
    if (event) {
        this.pageEvent = event;
        this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
        this.maxResultCount = this.pageEvent.pageSize;
    }

    this._tourService
      .getAll(
        this.nameFilter,
        this.priceFilter,
        this.descriptionFilter,
        this.startDateFilter,
        this.endDateFilter,
        this.longitudeFilter,
        this.latitudeFilter,
        this.sort,
        this.skipCount,
        this.maxResultCount
      )
      .subscribe(result => {
        this.tours = result.items;
        this.totalCount = result.totalCount;
      });
  }

  ngOnInit(): void {
    this.getTours();
  }
}
