import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MenuItem } from '@shared/layout/menu-item';

@Component({
    templateUrl: './sidebar-nav.component.html',
    selector: 'sidebar-nav',
    encapsulation: ViewEncapsulation.None
})
export class SideBarNavComponent extends AppComponentBase {

    menuItems: MenuItem[] = [
        new MenuItem(this.l('Dashboard'), '', 'las la-home', '/app/home'),
        new MenuItem(this.l('Tours'), 'Pages.Tour.View', 'las la-route', '/app/tours'),
        new MenuItem(this.l('Bookings'), 'Pages.Booking.View', 'las la-calendar-check', '/app/bookings'),
        new MenuItem(this.l('Transactions'), 'Pages.Transaction.View', 'las la-money-check-alt', '/app/transactions'),
        new MenuItem(this.l('Messages'), '', 'las la-comments', '/app/messages'),
        new MenuItem(this.l('Disputes'), 'Pages.Dispute.View', 'las la-archive', '/app/disputes'),
        new MenuItem(this.l('Requests'), 'Pages.Request.View', 'las la-briefcase', '/app/requests'),
        new MenuItem(this.l('Reviews'), 'Pages.Review.View', 'las la-star', '/app/reviews'),
        new MenuItem(this.l('Tenants'), 'Pages.Tenants', 'business', '/app/tenants'),
        new MenuItem(this.l('Users'), 'Pages.Users', 'las la-user', '/app/users'),
        new MenuItem(this.l('Roles'), 'Pages.Roles', 'las la-user-tag', '/app/roles'),
        // new MenuItem(this.l('About'), '', 'las la-info-circle', '/app/about'),
    ];

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    showMenuItem(menuItem): boolean {
        if (menuItem.permissionName) {
            return this.permission.isGranted(menuItem.permissionName);
        }

        return true;
    }
}
