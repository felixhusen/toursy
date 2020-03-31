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
  public slides: any[] = [
    {
      image:
        "https://www.sydney.com/sites/sydney/files/styles/landscape_992x558/public/2019-10/165838.jpg?itok=L1Xp4apm"
    },
    {
      image:
        "https://lp-cms-production.imgix.net/2019-06/65830387.jpg?fit=crop&q=40&sharp=10&vib=20&auto=format&ixlib=react-8.6.4"
    }
  ];
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
      });
  }

  public ngOnInit(): void {
    this.getTours();
  }

}
