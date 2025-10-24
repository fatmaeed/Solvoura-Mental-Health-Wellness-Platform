import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { NavBar } from "../../Layouts/nav-bar/nav-bar";
import { Footer } from "../../Layouts/footer/footer";
import { HeroSection } from "../../Components/hero-section/hero-section";
import { AboutUs } from "../../Components/about-us/about-us";
import { CustomFloatingActionBtn } from "../../Customs/custom-floating-action-btn/custom-floating-action-btn";
import { JoinUs } from "../../Components/join-us/join-us";
import { IllnessSection } from "../../Components/illness-section/illness-section";
import { DisplayServices } from "../../Components/OurServices/display-services/display-services";
import { ServiceProviderList } from "../../Components/ServiceProvider/service-provider-list/service-provider-list";
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AccountService } from '../../Services/Auth/account-service';
import { JwtHelperService } from '../../Services/jwt-helper-service';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [NavBar, Footer, HeroSection, AboutUs, CustomFloatingActionBtn, JoinUs, IllnessSection, DisplayServices, ServiceProviderList],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  constructor(private router: Router ,public accountService: AccountService, private jwtHelper: JwtHelperService, private cdr:ChangeDetectorRef,private route: ActivatedRoute, private scroller: ViewportScroller) {}

  ngOnInit(): void {
    const token = this.jwtHelper.getDecodedToken();
    this.cdr.detectChanges();
    this.route.fragment.subscribe(fragment => {
      if (fragment) {
        setTimeout(() => {
          this.scroller.scrollToAnchor(fragment);
        }, 0);
      }
    });
  }


}
