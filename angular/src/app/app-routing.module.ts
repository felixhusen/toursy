import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { HomeComponent } from "./home/home.component";
import { AboutComponent } from "./about/about.component";
import { UsersComponent } from "./users/users.component";
import { TenantsComponent } from "./tenants/tenants.component";
import { RolesComponent } from "app/roles/roles.component";
import { ChangePasswordComponent } from "./users/change-password/change-password.component";
import { ToursComponent } from "./tours/tours.component";
import { BookingsComponent } from "./bookings/bookings.component";
import { TransactionsComponent } from "./transactions/transactions.component";
import { DisputesComponent } from "./disputes/disputes.component";
import { RequestsComponent } from "./requests/requests.component";
import { ReviewsComponent } from "./reviews/reviews.component";
import { TourDetailsComponent } from "./tour-details/tour-details.component";
import { UserDetailsComponent } from "./user-details/user-details.component";
import { CreateBookingComponent } from "./bookings/create-booking/create-booking.component";
import { CreateTransactionComponent } from "./transactions/create-transaction/create-transaction.component";

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "",
        component: AppComponent,
        children: [
          {
            path: "home",
            component: HomeComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "tours",
            component: ToursComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "tours/:id",
            component: TourDetailsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "bookings",
            component: BookingsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "bookings/create-booking/:id",
            component: CreateBookingComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "disputes",
            component: DisputesComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "reviews",
            component: ReviewsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "requests",
            component: RequestsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "transactions",
            component: TransactionsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "transactions/create-transaction/:id",
            component: CreateTransactionComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "users",
            component: UsersComponent,
            data: { permission: "Pages.Users" },
            canActivate: [AppRouteGuard],
          },
          {
            path: "users/:id",
            component: UserDetailsComponent,
            data: { permission: "Pages.Users" },
            canActivate: [AppRouteGuard],
          },
          {
            path: "roles",
            component: RolesComponent,
            data: { permission: "Pages.Roles" },
            canActivate: [AppRouteGuard],
          },
          {
            path: "tenants",
            component: TenantsComponent,
            data: { permission: "Pages.Tenants" },
            canActivate: [AppRouteGuard],
          },
          { path: "about", component: AboutComponent },
          { path: "update-password", component: ChangePasswordComponent },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
