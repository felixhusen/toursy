import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  GetDisputeForViewDto,
  DisputeServiceProxy,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { CreateOrEditDisputeDialogComponent } from "./create-or-edit-dispute/create-or-edit-dispute-dialog.component";
import { MatDialog } from "@angular/material";

@Component({
  templateUrl: "./disputes.component.html",
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class DisputesComponent extends AppComponentBase implements OnInit {
  public searchQuery: string;
  public disputes: GetDisputeForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 5;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;

  constructor(
    injector: Injector,
    private _disputeService: DisputeServiceProxy,
    private _dialog: MatDialog
  ) {
    super(injector);
  }

  public getDisputes(event?: any): void {
    if (event) {
      this.pageEvent = event;
      this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
      this.maxResultCount = this.pageEvent.pageSize;
    }

    this._disputeService
      .getAll(this.searchQuery, this.sort, this.skipCount, this.maxResultCount)
      .subscribe((result) => {
        this.disputes = result.items;
        this.totalCount = result.totalCount;
      });
  }

  private showCreateOrEditDisputeDialog(id?: number): void {
    let createOrEditDisputeDialog;
    if (id === undefined || id <= 0) {
      createOrEditDisputeDialog = this._dialog.open(
        CreateOrEditDisputeDialogComponent
      );
    } else {
      createOrEditDisputeDialog = this._dialog.open(
        CreateOrEditDisputeDialogComponent,
        {
          data: id,
        }
      );
    }

    createOrEditDisputeDialog.afterClosed().subscribe((result) => {
      if (result) {
        this.getDisputes();
      }
    });
  }

  public paginate(event: any) {
    this.maxResultCount = event.rows;
    this.skipCount = this.maxResultCount * event.page;
    console.log("Skip Count: " + this.skipCount);
    console.log(event);
    this.getDisputes();
  }

  ngOnInit(): void {
    this.getDisputes();
  }
}
