import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoanService } from './services/loan.service';
import { Loan } from './models/loan';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule, FormsModule],
})
export class AppComponent implements OnInit {
  loans: Loan[] = [];
  paying: Record<number, boolean> = {};
  inputs: Record<number, number> = {};

  constructor(private loanService: LoanService) {}

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.loanService.getAll().subscribe({
      next: (data) => (this.loans = data),
      error: (err) => console.error(err),
    });
  }

  pay(loan: Loan): void {
    const amount = this.inputs[loan.id];
    if (!amount || amount <= 0) return;

    this.paying[loan.id] = true;
    this.loanService.makePayment(loan.id, amount).subscribe({
      next: (updated) => {
        const i = this.loans.findIndex((l) => l.id === loan.id);
        if (i >= 0) this.loans[i] = updated;
        this.inputs[loan.id] = 0;
        this.paying[loan.id] = false;
      },
      error: (err) => {
        console.error(err);
        this.paying[loan.id] = false;
      },
    });
  }
}
