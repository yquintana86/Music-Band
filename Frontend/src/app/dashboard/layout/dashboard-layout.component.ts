import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SideMenuComponent } from "../../shared/components/side-menu/side-menu.component";
import { HeaderNavComponent } from "../components/header-nav/header-nav.component";

@Component({
  selector: 'app-dashboard',
  imports: [RouterModule, SideMenuComponent, HeaderNavComponent],
  templateUrl: './dashboard-layout.component.html',
  styleUrl: './dashboard-layout.component.css'
})
export default class DashboardLayoutComponent{


}
