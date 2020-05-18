import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TourServiceProxy,
  GetTourForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  templateUrl: "./tour-details.component.html",
  styleUrls: ["../app.component.less", "./tour-details.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class TourDetailsComponent extends AppComponentBase implements OnInit {
  public tour: GetTourForViewDto;
  public _id: number;
  
  constructor(
    injector: Injector,
    private _tourService: TourServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _router: Router
  ) {
    super(injector);
  }

  public getTour(id: number): void {
    this._tourService.getTourForView(id).subscribe((result) => {
      this.tour = result;
    });
  }

  public bookTour(id: number): void {
    this._router.navigateByUrl('/app/bookings/create-booking', { queryParams: { tourId: id }});
  }

  ngOnInit(): void {
    this._id = Number(this._activatedRoute.snapshot.paramMap.get("id"));
    this.getTour(this._id);
  }
}
