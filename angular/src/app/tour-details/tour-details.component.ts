import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TourServiceProxy,
  GetTourForViewDto,
  CreateOrEditReviewDto,
  ReviewServiceProxy,
  GetReviewForViewDto,
  GetReviewForEditOutput,
  ReviewDto
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { ActivatedRoute, Router } from "@angular/router";
import { AppSessionService } from "@shared/session/app-session.service";


@Component({
  templateUrl: "./tour-details.component.html",
  styleUrls: ["../app.component.less", "./tour-details.component.less"],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None,
})
export class TourDetailsComponent extends AppComponentBase implements OnInit {
  public tour: GetTourForViewDto;
  public id: number;
  public rating: number = 0;
  public reviewText: string;
  public testReviews: GetReviewForViewDto[] = [];
  public reviews: GetReviewForViewDto[];
  public maxResultCount: number = 25;
  public skipCount: number = 0;
  public sort: string;

  public review: GetReviewForEditOutput = new GetReviewForEditOutput();

  constructor(
    injector: Injector,
    private _tourService: TourServiceProxy,
    private _reviewService: ReviewServiceProxy,
    private _appSessionService: AppSessionService,
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

  public bookTour(id: number): void {
    this._router.navigateByUrl('/app/bookings/create-booking', { queryParams: { tourId: id }});
  }

  public setRating(to: number): void {
    let stars = document.querySelectorAll(".ratestar");
    for (let i = 0; i < 5; i++) {
      stars[i].innerHTML = i < to ? "star" : "star_border";
    }
    this.rating = to;
  }

  public saveReview(): void {
    this._reviewService
      .createOrEdit(this.review.review)
      .subscribe(result => {
        let submitReview = new CreateOrEditReviewDto();
        submitReview.userId = this._appSessionService.userId;
        submitReview.tourId = this.tour.tour.id;
        submitReview.rating = this.rating;
        submitReview.description = this.reviewText;
        submitReview.datePosted = moment();

        this._reviewService.createOrEdit(submitReview).subscribe();
      });
  }

  // does not return any results at the moment
  public getReviewsForTour(): void {
    this._reviewService
      .getAll(undefined, undefined, undefined, undefined)
      .subscribe((result) => {
        console.log(result);
        this.reviews = result.items;
        this.reviews = this.reviews.filter(item => item.review.tourId === this.tour.tour.id);
      });
  }

  public deleteReview(reviewId: number): void {
    this.reviews.filter(rev => rev.review.id != reviewId);
    // delete from backend too (?)
  }

  private openReviewDialog(): void {
    document.querySelector("#reviewCard").scrollIntoView({behavior: "smooth"});
  }

  ngOnInit(): void {
    this.id = Number(this._activatedRoute.snapshot.paramMap.get("id"));
    this.getTour(this.id);  // changed from _id to id
    this.getReviewsForTour();
  }
}
