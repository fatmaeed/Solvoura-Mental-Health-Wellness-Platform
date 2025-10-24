import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutlinedBtn } from './outlined-btn';

describe('OutlinedBtn', () => {
  let component: OutlinedBtn;
  let fixture: ComponentFixture<OutlinedBtn>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OutlinedBtn]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OutlinedBtn);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
