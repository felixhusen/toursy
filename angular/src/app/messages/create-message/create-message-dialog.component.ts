import {
  Component,
  Injector,
  Optional,
  Inject,
  OnInit,
  ElementRef,
  ViewChild,
} from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { finalize } from "rxjs/operators";
import * as _ from "lodash";
import { AppComponentBase } from "@shared/app-component-base";
import {
  CreateOrEditTourDto,
  TourServiceProxy,
  GetTourForEditOutput,
  TourDateDto,
  MessageServiceProxy,
  CreateMessageDto,
} from "@shared/service-proxies/service-proxies";
import { AppConsts } from "@shared/AppConsts";
import { HttpClient } from "@angular/common/http";
import * as moment from "moment";

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
    `,
  ],
})
export class CreateMessageDialogComponent extends AppComponentBase
  implements OnInit {
  public saving = false;
  public content: string;
  public userId: number;

  constructor(
    injector: Injector,
    private _messageService: MessageServiceProxy,
    private _dialogRef: MatDialogRef<CreateMessageDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number,
    private _httpClient: HttpClient
  ) {
    super(injector);
  }

  ngOnInit(): void {}

  public sendMessage(): void {
    const message = new CreateMessageDto();
    message.content = this.content;
    message.receiverId = this.userId;
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

  save(): void {
    this.saving = true;
    this.sendMessage();
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
