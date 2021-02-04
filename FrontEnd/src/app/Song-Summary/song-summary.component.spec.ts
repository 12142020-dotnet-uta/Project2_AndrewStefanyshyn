import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SongSummaryComponent } from './song-summary.component';

describe('SongSummaryComponent', () => {
  let component: SongSummaryComponent;
  let fixture: ComponentFixture<SongSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SongSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SongSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
