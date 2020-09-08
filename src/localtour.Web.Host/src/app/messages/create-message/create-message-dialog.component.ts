import {
  Component,
  Injector,
  Optional,
  Inject,
  OnInit,
} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { finalize } from "rxjs/operators";
import * as _ from "lodash";
import { AppComponentBase } from "@shared/app-component-base";
import {
  MessageServiceProxy,
  CreateMessageDto,
  UserDto,
  UserServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";
import { AbpSessionService } from "abp-ng2-module/dist/src/session/abp-session.service";
import { AppSessionService } from "@shared/session/app-session.service";

@Component({
  templateUrl: "./create-message-dialog.component.html",
  styles: [
    `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
      .ui-autocomplete .ui-autocomplete-input {
        width: 100% !important;
      }
    `,
  ],
})
export class CreateMessageDialogComponent extends AppComponentBase
  implements OnInit {
  public saving = false;
  public content: string;
  public user: UserDto;
  public users: UserDto[];

  constructor(
    injector: Injector,
    private _messageService: MessageServiceProxy,
    private _userService: UserServiceProxy,
    private _appSessionService: AppSessionService,
    private _dialogRef: MatDialogRef<CreateMessageDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public _id: number,
    private _httpClient: HttpClient
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getUsers();
  }

  public sendMessage(): void {
    const message = new CreateMessageDto();
    message.content = this.content;
    message.receiverId = this.user.id;
    this._messageService
      .sendMessage(message)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((result) => {
        this.notify.info(this.l("SavedSuccessfully"));
        this.close(true);
      });
  }

  public getUsers(username?: string) {
    this._userService.getAll(username, undefined, undefined, undefined).subscribe(result => {
      this.users = result.items;
      this.users = this.users.filter(user => user.id != this._appSessionService.user.id)
    });
  }

  save(): void {
    this.saving = true;
    this.sendMessage();
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
