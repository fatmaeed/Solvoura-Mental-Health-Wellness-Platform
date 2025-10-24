import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeatureCardComponant } from './feature-card-componant';

describe('FeatureCardComponant', () => {
  let component: FeatureCardComponant;
  let fixture: ComponentFixture<FeatureCardComponant>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeatureCardComponant]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeatureCardComponant);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
