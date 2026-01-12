import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { LoanService } from './loan.service';
import { environment } from '../../environments/environment';

describe('LoanService', () => {
  let service: LoanService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.apiUrl}/loans`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [LoanService],
    });
    service = TestBed.inject(LoanService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should GET all loans', () => {
    service.getAll().subscribe();
    const req = httpMock.expectOne(baseUrl);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should POST payment', () => {
    const id = 1; const paymentAmount = 100;
    service.makePayment(id, paymentAmount).subscribe();
    const req = httpMock.expectOne(`${baseUrl}/${id}/payment`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ paymentAmount });
    req.flush({});
  });
});