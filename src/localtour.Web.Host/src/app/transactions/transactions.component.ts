import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TourServiceProxy,
  GetTourForViewDto,
  TransactionServiceProxy,
  GetTransactionForViewDto
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { FileDownloadService } from "@shared/utils/file-download.service";

@Component({
  templateUrl: "./transactions.component.html",
  styleUrls: ['../app.component.less', './transactions.component.less'],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})

export class TransactionsComponent extends AppComponentBase implements OnInit {
  public searchQuery: string;
  public transactions: GetTransactionForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 25;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;
  public title: string = "My Transactions";
  public mode: string;
  public loading: boolean = false;

  constructor(injector: Injector, private _transactionService: TransactionServiceProxy, private _fileDownloadService: FileDownloadService) {
    super(injector);
  }

  public getTransactions(event?: any): void {
    this.loading = true;
    if (event) {
        this.pageEvent = event;
        this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
        this.maxResultCount = this.pageEvent.pageSize;
    }

    this._transactionService
      .getAll(this.searchQuery, this.mode, this.sort, this.skipCount, this.maxResultCount)
      .subscribe((result) => {
        this.transactions = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      });
  }

  public toggleMyTransaction() {
    this.title = "My Transactions";
    this.mode = undefined;
    this.getTransactions();
  }

  public toggleCustomerTransaction() {
    this.title = "Customer's Transactions";
    this.mode = "CustomerTransactions";
    this.getTransactions();
  }

  public toggleCancellationRequests() {
    this.title = "Transaction Cancellation Requests";
    this.mode = "CancellationRequests";
    this.getTransactions();
  }

  public togglePendingRequests() {
    this.title = "Pending Requests";
    this.mode = "PendingRequests";
    this.getTransactions();
  }

  public async requestCancelTransaction(id: number) {
    if (confirm("Are you sure to cancel this transaction?")) {
      await this._transactionService.cancelTransaction(id).toPromise();
      this.getTransactions();
    }
  }

  public async approveTransaction(id: number, status: string) {
    if (status === "Cancellation Requested") {
      if (confirm("Are you sure to approve the cancellation of this transaction?")) {
        await this._transactionService.approveTransactionCancellation(id).toPromise();
        this.getTransactions();
      }
    } else {
      if (confirm("Are you sure to approve the this transaction?")) {
        await this._transactionService.approveTransaction(id).toPromise();
        this.getTransactions();
      }
    }
  }

  public exportToExcel(): void {
    this._transactionService.getTransactionsToExcel(this.searchQuery, this.mode, undefined, undefined, undefined)
      .subscribe((result) => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }

  public paginate(event: any) {
    this.maxResultCount = event.rows;
    this.skipCount = this.maxResultCount * event.page;
    console.log("Skip Count: " + this.skipCount);
    console.log(event);
    this.getTransactions();
  }

  ngOnInit(): void {
    this.getTransactions();
  }
}
