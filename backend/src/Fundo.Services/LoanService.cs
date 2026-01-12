using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Domain.Entities;
using Fundo.Infrastructure.Repositories;

namespace Fundo.Services
{
    public interface ILoanService
    {
        Task<Loan> CreateLoanAsync(decimal amount, string applicantName);
        Task<Loan> GetLoanByIdAsync(int id);
        Task<List<Loan>> GetAllLoansAsync();
        Task<Loan> MakePaymentAsync(int loanId, decimal paymentAmount);
    }

    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<Loan> CreateLoanAsync(decimal amount, string applicantName)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0.");

            if (string.IsNullOrWhiteSpace(applicantName))
                throw new ArgumentException("Applicant name is required.");

            var loan = new Loan
            {
                Amount = amount,
                CurrentBalance = amount,
                ApplicantName = applicantName,
                Status = LoanStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            return await _loanRepository.AddAsync(loan);
        }

        public async Task<Loan> GetLoanByIdAsync(int id)
        {
            return await _loanRepository.GetByIdAsync(id);
        }

        public async Task<List<Loan>> GetAllLoansAsync()
        {
            return await _loanRepository.GetAllAsync();
        }

        public async Task<Loan> MakePaymentAsync(int loanId, decimal paymentAmount)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);

            if (loan == null)
                throw new KeyNotFoundException($"Loan with id {loanId} not found.");

            if (loan.Status == LoanStatus.Paid)
                throw new InvalidOperationException("Cannot make payment on an already paid loan.");

            if (paymentAmount <= 0)
                throw new ArgumentException("Payment amount must be greater than 0.");

            loan.CurrentBalance -= paymentAmount;

            if (loan.CurrentBalance <= 0)
            {
                loan.CurrentBalance = 0;
                loan.Status = LoanStatus.Paid;
            }

            return await _loanRepository.UpdateAsync(loan);
        }
    }
}