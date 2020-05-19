import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  MessageServiceProxy,
  MessageDto,
  GetMessageForViewDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";

@Component({
  templateUrl: "./messages.component.html",
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class MessagesComponent extends AppComponentBase implements OnInit {
  public messages: GetMessageForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 5;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;

  constructor(
    injector: Injector,
    private _messageService: MessageServiceProxy
  ) {
    super(injector);
  }

  public getMessages(event?: any): void {
    if (event) {
      this.pageEvent = event;
      this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
      this.maxResultCount = this.pageEvent.pageSize;
    }

    this._messageService.getMessages().subscribe((result) => {
      this.messages = result.items;
    });
  }

  public paginate(event: any) {
    this.maxResultCount = event.rows;
    this.skipCount = this.maxResultCount * event.page;
    console.log("Skip Count: " + this.skipCount);
    console.log(event);
    this.getMessages();
  }

  ngOnInit(): void {
    this.getMessages();
  }
}
