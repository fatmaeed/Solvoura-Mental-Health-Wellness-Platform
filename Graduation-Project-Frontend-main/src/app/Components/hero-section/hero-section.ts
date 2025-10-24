import { Component } from '@angular/core';
import { CustomBtn } from "../../Customs/custom-btn/custom-btn";
import { CustomSearch } from "../../Customs/custom-search/custom-search";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-hero-section',
  imports: [ CustomBtn, CustomSearch,RouterLink],
  templateUrl: './hero-section.html',
  styleUrl: './hero-section.css'
})
export class HeroSection {

}
