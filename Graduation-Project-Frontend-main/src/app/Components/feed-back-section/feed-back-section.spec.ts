import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedBackSection } from './feed-back-section';

describe('FeedBackSection', () => {
  let component: FeedBackSection;
  let fixture: ComponentFixture<FeedBackSection>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedBackSection]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeedBackSection);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
