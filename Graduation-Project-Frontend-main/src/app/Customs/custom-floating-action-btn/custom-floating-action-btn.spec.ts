import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomFloatingActionBtn } from './custom-floating-action-btn';

describe('CustomFloatingActionBtn', () => {
  let component: CustomFloatingActionBtn;
  let fixture: ComponentFixture<CustomFloatingActionBtn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomFloatingActionBtn]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomFloatingActionBtn);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
