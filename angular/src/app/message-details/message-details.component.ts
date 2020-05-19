import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  MessageServiceProxy,
  GetMessageForViewDto,
  CreateMessageDto,
  UserServiceProxy,
  UserDto,
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
  templateUrl: "./message-details.component.html",
  styles: [
    `
      .chat-list {
        overflow: auto;
        height: 50vh;
      }
    `,
  ],
  styleUrls: ["../app.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class MessageDetailsComponent extends AppComponentBase
  implements OnInit {
  public messages: GetMessageForViewDto[];
  public sender: UserDto;
  public _id: number;
  public title: string;
  public chatBox: string;

  constructor(
    injector: Injector,
    private _messageService: MessageServiceProxy,
    private _userService: UserServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _router: Router
  ) {
    super(injector);
  }

  public getMessages(id: number): void {
    this._messageService.getMessagesBySender(id).subscribe((result) => {
      this.messages = result;
    });
  }

  public getUser(id: number): void {
    this._userService.get(id).subscribe((result) => {
      this.sender = result;
    });
  }

  public sendMessage(): void {
    const message = new CreateMessageDto();
    message.content = this.chatBox;
    message.receiverId = this._id;
    this._messageService.sendMessage(message).subscribe((result) => {
      this.messages.push(result);
      this.chatBox = "";
    });
  }

  ngOnInit(): void {
    this._id = Number(this._activatedRoute.snapshot.paramMap.get("id"));
    this.getMessages(this._id);
    this.getUser(this._id);
  }
}
