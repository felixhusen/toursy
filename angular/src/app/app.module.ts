import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientJsonpModule } from "@angular/common/http";
import { HttpClientModule } from "@angular/common/http";
import { MatCarouselModule } from "@ngmodule/material-carousel";
import { ModalModule } from "ngx-bootstrap";
import { NgxPaginationModule } from "ngx-pagination";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import {
  DlDateTimeDateModule,
  DlDateTimePickerModule,
} from "angular-bootstrap-datetimepicker";
import { AbpModule } from "@abp/abp.module";
import { ServiceProxyModule } from "@shared/service-proxies/service-proxy.module";
import { SharedModule } from "@shared/shared.module";
import { AngularFileUploaderModule } from "angular-file-uploader";
import { TableModule } from "primeng/table";
import { PaginatorModule } from "primeng/paginator";
import { AutoCompleteModule } from "primeng/autocomplete";

import { HomeComponent } from "@app/home/home.component";
import { AboutComponent } from "@app/about/about.component";
import { TopBarComponent } from "@app/layout/topbar.component";
import { TopBarLanguageSwitchComponent } from "@app/layout/topbar-languageswitch.component";
import { SideBarUserAreaComponent } from "@app/layout/sidebar-user-area.component";
import { SideBarNavComponent } from "@app/layout/sidebar-nav.component";
import { SideBarFooterComponent } from "@app/layout/sidebar-footer.component";
import { RightSideBarComponent } from "@app/layout/right-sidebar.component";
// tenants
import { TenantsComponent } from "@app/tenants/tenants.component";
import { CreateTenantDialogComponent } from "./tenants/create-tenant/create-tenant-dialog.component";
import { EditTenantDialogComponent } from "./tenants/edit-tenant/edit-tenant-dialog.component";
// roles
import { RolesComponent } from "@app/roles/roles.component";
import { CreateRoleDialogComponent } from "./roles/create-role/create-role-dialog.component";
import { EditRoleDialogComponent } from "./roles/edit-role/edit-role-dialog.component";
// users
import { UsersComponent } from "@app/users/users.component";
import { CreateUserDialogComponent } from "@app/users/create-user/create-user-dialog.component";
import { EditUserDialogComponent } from "@app/users/edit-user/edit-user-dialog.component";
import { ChangePasswordComponent } from "./users/change-password/change-password.component";
import { ResetPasswordDialogComponent } from "./users/reset-password/reset-password.component";
import { UserDetailsComponent } from "./user-details/user-details.component";
// tours
import { ToursComponent } from "@app/tours/tours.component";
import { CreateOrEditTourDialogComponent } from "@app/tours/create-or-edit-tour/create-or-edit-tour-dialog.component";
import { TourDetailsComponent } from "./tour-details/tour-details.component";
// bookings
import { BookingsComponent } from "@app/bookings/bookings.component";
import { CreateBookingComponent } from "./bookings/create-booking/create-booking.component";
// transactions
import { TransactionsComponent } from "@app/transactions/transactions.component";
import { CreateTransactionComponent } from "./transactions/create-transaction/create-transaction.component";
// reviews
import { ReviewsComponent } from "./reviews/reviews.component";
// requests
import { RequestsComponent } from "./requests/requests.component";
// disputes
import { DisputesComponent } from "./disputes/disputes.component";
import { MessagesComponent } from "./messages/messages.component";
import { MessageDetailsComponent } from "./message-details/message-details.component";
import { CreateMessageDialogComponent } from "./messages/create-message/create-message-dialog.component";
import { CreateOrEditRequestDialogComponent } from "./requests/create-or-edit-request/create-or-edit-request-dialog.component";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    TopBarComponent,
    TopBarLanguageSwitchComponent,
    SideBarUserAreaComponent,
    SideBarNavComponent,
    SideBarFooterComponent,
    RightSideBarComponent,
    // tenants
    TenantsComponent,
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    RolesComponent,
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    UsersComponent,
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ChangePasswordComponent,
    ResetPasswordDialogComponent,
    UserDetailsComponent,
    // tours
    ToursComponent,
    CreateOrEditTourDialogComponent,
    TourDetailsComponent,
    // bookings
    BookingsComponent,
    CreateBookingComponent,
    // transactions
    TransactionsComponent,
    CreateTransactionComponent,
    // reviews
    ReviewsComponent,
    // requests
    RequestsComponent,
    CreateOrEditRequestDialogComponent,
    // disputes
    DisputesComponent,
    // messages
    MessagesComponent,
    MessageDetailsComponent,
    CreateMessageDialogComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule.forRoot(),
    AbpModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
    MatCarouselModule,
    AngularFileUploaderModule,
    TableModule,
    DlDateTimeDateModule, // <--- Determines the data type of the model
    DlDateTimePickerModule,
    PaginatorModule,
    AutoCompleteModule
  ],
  providers: [],
  entryComponents: [
    // tenants
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ResetPasswordDialogComponent,
    // tours
    CreateOrEditTourDialogComponent,
    // bookings
    BookingsComponent,
    // transactions
    TransactionsComponent,
    // reviews
    ReviewsComponent,
    // requests
    RequestsComponent,
    CreateOrEditRequestDialogComponent,
    // disputes
    DisputesComponent,
    // messages
    CreateMessageDialogComponent,
  ],
})
export class AppModule {}
