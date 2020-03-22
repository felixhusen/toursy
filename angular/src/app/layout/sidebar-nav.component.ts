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
        new MenuItem(this.l('Tours'), '', 'las la-route', '/app/tours'),
        new MenuItem(this.l('Bookings'), 'Pages.User.Bookings', 'las la-calendar-check', '/app/bookings'),
        new MenuItem(this.l('Transactions'), 'Pages.User.Transactions', 'las la-money-check-alt', '/app/bookings'),
        new MenuItem(this.l('Disputes'), 'Pages.User.Disputes', 'las la-archive', '/app/bookings'),
        new MenuItem(this.l('Requests'), 'Pages.User.Requests', 'las la-briefcase', '/app/bookings'),
        new MenuItem(this.l('Reviews'), 'Pages.User.Reviews', 'las la-star', '/app/bookings'),
        new MenuItem(this.l('Tenants'), 'Pages.Tenants', 'business', '/app/tenants'),
        new MenuItem(this.l('Users'), 'Pages.Users', 'las la-user', '/app/users'),
        new MenuItem(this.l('Roles'), 'Pages.Roles', 'las la-user-tag', '/app/roles'),
        new MenuItem(this.l('About'), '', 'las la-info-circle', '/app/about'),

        // new MenuItem(this.l('MultiLevelMenu'), '', 'menu', '', [
        //     new MenuItem('ASP.NET Boilerplate', '', '', '', [
        //         new MenuItem('Home', '', '', 'https://aspnetboilerplate.com/?ref=abptmpl'),
        //         new MenuItem('Templates', '', '', 'https://aspnetboilerplate.com/Templates?ref=abptmpl'),
        //         new MenuItem('Samples', '', '', 'https://aspnetboilerplate.com/Samples?ref=abptmpl'),
        //         new MenuItem('Documents', '', '', 'https://aspnetboilerplate.com/Pages/Documents?ref=abptmpl')
        //     ]),
        //     new MenuItem('ASP.NET Zero', '', '', '', [
        //         new MenuItem('Home', '', '', 'https://aspnetzero.com?ref=abptmpl'),
        //         new MenuItem('Description', '', '', 'https://aspnetzero.com/?ref=abptmpl#description'),
        //         new MenuItem('Features', '', '', 'https://aspnetzero.com/?ref=abptmpl#features'),
        //         new MenuItem('Pricing', '', '', 'https://aspnetzero.com/?ref=abptmpl#pricing'),
        //         new MenuItem('Faq', '', '', 'https://aspnetzero.com/Faq?ref=abptmpl'),
        //         new MenuItem('Documents', '', '', 'https://aspnetzero.com/Documents?ref=abptmpl')
        //     ])
        // ])
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
