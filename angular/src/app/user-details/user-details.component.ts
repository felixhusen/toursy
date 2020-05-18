import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  RequestServiceProxy,
  GetRequestForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";

@Component({
  templateUrl: "./user-details.component.html",
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})

export class UserDetailsComponent extends AppComponentBase implements OnInit {
  public user: any;

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  public getUser(event?: any): void {

    // this._requestService
    //   .getAll(this.searchQuery, this.sort, this.skipCount, this.maxResultCount)
    //   .subscribe((result) => {
    //     this.requests = result.items;
    //     this.totalCount = result.totalCount;
    //     console.log("Result")
    //     console.log(this.requests)
    //   });
  }

  ngOnInit(): void {
    this.getUser();
  }
}
