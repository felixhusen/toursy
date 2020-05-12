import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  BookingServiceProxy,
  GetBookingForViewDto,
  CreateOrEditBookingDto,
  TourServiceProxy,
  GetTourForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { ActivatedRoute, Router } from "@angular/router";
import { Route } from "@angular/compiler/src/core";

@Component({
  templateUrl: "./create-booking.component.html",
  styles: [
    `
      .tour-image {
        width: 100%;
        height: 200px;
        object-fit: contain;
        margin-bottom: 3rem;
      }
    `,
  ],
  styleUrls: ["../../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class CreateBookingComponent extends AppComponentBase implements OnInit {
  public booking: CreateOrEditBookingDto = new CreateOrEditBookingDto();
  public tour: GetTourForViewDto;

  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy,
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

  public validate(): void {
    this._router.navigateByUrl("/app/transactions/create-transaction");
  }

  public getTourId(): void {
    try {
      this._activatedRoute.queryParams.subscribe((params) => {
        this.booking.tourId = params["tourId"];
        console.log(params);
      });
    } catch {
      console.log("No Tour Id");
    }
  }

  ngOnInit(): void {
    this.getTour(1);
  }
}
