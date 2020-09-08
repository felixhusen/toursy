import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  GetBookingForViewDto,
  BookingServiceProxy,
  UserServiceProxy,
  UserDto,
  RoleServiceProxy,
  RoleDto,
} from "@shared/service-proxies/service-proxies";

@Component({
  templateUrl: "./user-details.component.html",
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})

export class UserDetailsComponent extends AppComponentBase implements OnInit {
  public user: UserDto;
  private editing: boolean = false;
  public bookings: GetBookingForViewDto[];
  private roleName: string;

  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy,
    private _userService: UserServiceProxy,
    private _roleService: RoleServiceProxy
  ) {
    super(injector);
  }

  public save(): void {
    this.editing = false;
    this._userService.update(this.user).subscribe(() => {
      this.notify.info("Changes saved");
      location.reload();
    })
  }

  public cancel(): void {
    this.editing = false;
  }

  public async getUser() {
    this.user = await this._userService.get(this.appSession.userId).toPromise();
    this.roleName = this.user.roleNames[0].toUpperCase().charAt(0) + this.user.roleNames[0].toLowerCase().slice(1);
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
    this.getUser();
  }
}
