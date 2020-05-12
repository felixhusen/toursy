import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { GetTourForViewDto, TourServiceProxy } from "@shared/service-proxies/service-proxies";
import * as moment from "moment";

@Component({
  templateUrl: "./home.component.html",
  styleUrls: ['../app.component.less', './home.component.less'],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})

export class HomeComponent extends AppComponentBase implements OnInit {
  public slides: any = [];
  public nameFilter: string;
  public priceFilter: number;
  public descriptionFilter: string;
  public startDateFilter: moment.Moment;
  public endDateFilter: moment.Moment;
  public longitudeFilter: string;
  public latitudeFilter: string;
  public tours: GetTourForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 6;
  public skipCount: number = 0;
  public sort: string = "tour.id desc";
  public defaultImageLink: string = "https://attendantdesign.com/wp-content/uploads/2017/08/tour-1-1.jpg";

  constructor(injector: Injector, private _tourService: TourServiceProxy) {
    super(injector);
  }

  public getTours(event?: any): void {
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
        this.setupCarousel();
      });
  }

  public setupCarousel(): void {
    const length = this.tours.length < 2 ? this.tours.length : 2;
    for (let i = 0; i < length; i++) {
      this.slides.push(this.tours[i]);
    }
  }

  public ngOnInit(): void {
    this.getTours();
  }

}
