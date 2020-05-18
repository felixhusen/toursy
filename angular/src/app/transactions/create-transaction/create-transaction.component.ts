import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TransactionServiceProxy,
  GetTransactionForViewDto,
  CreateOrEditTransactionDto,
  TourServiceProxy,
  GetTourForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { ActivatedRoute, Router } from "@angular/router";
import { Route } from "@angular/compiler/src/core";

@Component({
  templateUrl: "./create-transaction.component.html",
  styles: [`
    .tour-image {
      width: 100%;
      height: 200px;
      object-fit: contain;
      margin-bottom: 3rem;
    }
  `],
  styleUrls: ["../../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class CreateTransactionComponent extends AppComponentBase implements OnInit {
  public transaction: CreateOrEditTransactionDto = new CreateOrEditTransactionDto();
  public tour: GetTourForViewDto;

  constructor(
    injector: Injector,
    private _transactionService: TransactionServiceProxy,
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

  ngOnInit(): void {
    this.getTour(1);
  }
}
