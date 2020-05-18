import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  BookingServiceProxy,
  GetBookingForViewDto,
  CreateOrEditBookingDto,
  TourServiceProxy,
  GetTourForViewDto,
  CreateOrEditTransactionDto,
  TransactionServiceProxy,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { ActivatedRoute, Router } from "@angular/router";
import { Route } from "@angular/compiler/src/core";
import { AppSessionService } from "@shared/session/app-session.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
  templateUrl: "./create-booking.component.html",
  styles: [
    `
      .tour-preview-image {
        width: 100%;
        height: 200px;
        object-fit: cover;
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
  public transaction: CreateOrEditTransactionDto = new CreateOrEditTransactionDto();
  public tour: GetTourForViewDto;
  public tourId: number;
  public isLinear: boolean = true;
  // public transactionForm: FormGroup;
  // public bookingForm: FormGroup;

  constructor(
    injector: Injector,
    private _bookingService: BookingServiceProxy,
    private _transactionService: TransactionServiceProxy,
    private _appSessionService: AppSessionService,
    private _tourService: TourServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _router: Router
  ) {
    super(injector);
  }

  public getTour(id: number): void {
    this._tourService.getTourForView(id).subscribe((result) => {
      this.tour = result;
    });
  }

  public async submitBooking() {
    this.booking.totalPrice = this.tour.tour.price * this.booking.numberOfPeople;
    this.transaction.amount = this.booking.totalPrice;
    const result = await this._bookingService.createOrEdit(this.booking).toPromise();
    this.booking.id = result.id;
    this.notify.info("Booking submitted");
  }

  public async submitTransaction() {
    this.transaction.bookingId = this.booking.id;
    this.transaction.amount = this.booking.totalPrice;
    this.transaction.transactionDate = moment();
    console.log(this.transaction);
    await this._transactionService.createOrEdit(this.transaction).toPromise();
    this.notify.info("Transaction submitted");
  }

  public defaultBookingInformation(): void {
    this.booking.numberOfPeople = 1;
    this.booking.postCode = 2000;
    this.booking.name = this._appSessionService.user.name + " " + this._appSessionService.user.surname;
    this.booking.email = this._appSessionService.user.emailAddress;
    this.booking.userId = this._appSessionService.user.id;
    this.booking.tourId = this.tourId;
  }

  ngOnInit(): void {
    this.tourId = Number(this._activatedRoute.snapshot.paramMap.get("id"));
    this.getTour(this.tourId);
    this.defaultBookingInformation();

    // this.bookingForm = this._formBuilder.group({
    //   firstCtrl: ['', Validators.required]
    // });
    // this.transactionForm = this._formBuilder.group({
    //   secondCtrl: ['', Validators.required]
    // });
  }
}
