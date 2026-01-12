import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Loan } from '../models/loan';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  private apiUrl: string;

  constructor(private http: HttpClient) {
    // Use relative API path that works in both dev and Docker
    this.apiUrl = '/api/loans';
  }

  getAll(): Observable<Loan[]> {
    return this.http.get<Loan[]>(this.apiUrl);
  }

  getById(id: number): Observable<Loan> {
    return this.http.get<Loan>(`${this.apiUrl}/${id}`);
  }

  create(amount: number, applicantName: string): Observable<Loan> {
    return this.http.post<Loan>(this.apiUrl, { amount, applicantName });
  }

  makePayment(id: number, paymentAmount: number): Observable<Loan> {
    return this.http.post<Loan>(`${this.apiUrl}/${id}/payment`, { paymentAmount });
  }
}


