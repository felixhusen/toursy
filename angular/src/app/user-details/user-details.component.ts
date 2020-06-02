import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  RequestServiceProxy,
  GetRequestForViewDto,
  GetBookingForViewDto,
  BookingServiceProxy,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { AppSessionService } from "@shared/session/app-session.service";

@Component({
  templateUrl: "./user-details.component.html",
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})

export class UserDetailsComponent extends AppComponentBase implements OnInit {
  public user: any;
  private username: string = this.appSession.getShownLoginName();
  private email: string = this.appSession.user.emailAddress;
  private firstName: string = this.appSession.user.name;
  private lastName: string = this.appSession.user.surname;
  private editing: boolean = false;
  public bookings: GetBookingForViewDto[];


  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy,
  ) {
    super(injector);
  }

  public save(): void {
    this.editing = false;
    // update backend components
  }

  public getBookings(): void {
      this._bookingService
          .getAll(
            undefined,
            undefined,
            undefined,
            undefined,
            undefined
          )
          .subscribe((result) => {
            this.bookings = result.items;
            console.log("Result");
            console.log(this.bookings);
          });
  }

  ngOnInit(): void {
    this.getBookings();
  }
}
