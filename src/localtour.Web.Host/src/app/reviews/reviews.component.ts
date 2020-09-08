import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  ReviewServiceProxy,
  GetReviewForViewDto
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";
import { AppSessionService } from "@shared/session/app-session.service";

@Component({
  templateUrl: "./reviews.component.html",
  styleUrls: ['../app.component.less'],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})

export class ReviewsComponent extends AppComponentBase implements OnInit {
  public searchQuery: string;
  public reviews: GetReviewForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 25;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;
  public userId: number;

  constructor(injector: Injector, private _reviewService: ReviewServiceProxy, private _appSessionService: AppSessionService) {
    super(injector);
  }

  public getReviews(event?: any): void {
    if (event) {
        this.pageEvent = event;
        this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
        this.maxResultCount = this.pageEvent.pageSize;
    }

    this._reviewService
      .getAll(this.searchQuery, this.sort, this.skipCount, this.maxResultCount)
      .subscribe((result) => {
        console.log(result);
        this.reviews = result.items;
        this.totalCount = result.totalCount;
      });
  }

  public paginate(event: any) {
    this.maxResultCount = event.rows;
    this.skipCount = this.maxResultCount * event.page;
    console.log("Skip Count: " + this.skipCount);
    console.log(event);
    this.getReviews();
  }

  public async deleteReview(id: number) {
    if (confirm("Are you sure to delete this review?")) {
      await this._reviewService.delete(id).toPromise();
      this.getReviews();
    }
  }

  ngOnInit(): void {
    this.userId = this._appSessionService.userId;
    this.getReviews();
  }
}
