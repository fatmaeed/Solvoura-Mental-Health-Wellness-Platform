import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayServices } from './display-services';

describe('DisplayServices', () => {
  let component: DisplayServices;
  let fixture: ComponentFixture<DisplayServices>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DisplayServices]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DisplayServices);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
