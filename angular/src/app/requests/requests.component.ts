import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  RequestServiceProxy,
  GetRequestForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { CreateOrEditRequestDialogComponent } from "./create-or-edit-request/create-or-edit-request-dialog.component";
import { MatDialog } from "@angular/material";

@Component({
  templateUrl: "./requests.component.html",
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class RequestsComponent extends AppComponentBase implements OnInit {
  public searchQuery: string;
  public requests: GetRequestForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 5;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;

  constructor(
    injector: Injector,
    private _requestService: RequestServiceProxy,
    private _dialog: MatDialog
  ) {
    super(injector);
  }

  public paginate(event: any) {
    this.maxResultCount = event.rows;
    this.skipCount = this.maxResultCount * event.page;
    console.log("Skip Count: " + this.skipCount);
    console.log(event);
    this.getRequests();
  }

  private showCreateOrEditRequestDialog(id?: number): void {
    let createOrEditRequestDialog;
    if (id === undefined || id <= 0) {
      createOrEditRequestDialog = this._dialog.open(CreateOrEditRequestDialogComponent);
    } else {
      createOrEditRequestDialog = this._dialog.open(CreateOrEditRequestDialogComponent, {
          data: id
      });
    }

    createOrEditRequestDialog.afterClosed().subscribe((result) => {
      if (result) {
        this.getRequests();
      }
    });
  }

  public getRequests(event?: any): void {
    if (event) {
      this.pageEvent = event;
      this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
      this.maxResultCount = this.pageEvent.pageSize;
    }

    this._requestService
      .getAll(this.searchQuery, this.sort, this.skipCount, this.maxResultCount)
      .subscribe((result) => {
        this.requests = result.items;
        this.totalCount = result.totalCount;
        console.log("Result")
        console.log(this.requests)
      });
  }

  ngOnInit(): void {
    this.getRequests();
  }
}
