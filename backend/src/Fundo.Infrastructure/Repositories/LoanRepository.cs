using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fundo.Domain.Entities;
using Fundo.Infrastructure.Data;

namespace Fundo.Infrastructure.Repositories
{
    public interface ILoanRepository
    {
        Task<Loan> GetByIdAsync(int id);
        Task<List<Loan>> GetAllAsync();
        Task<Loan> AddAsync(Loan loan);
        Task<Loan> UpdateAsync(Loan loan);
    }

    public class LoanRepository : ILoanRepository
    {
        private readonly FundoDbContext _context;

        public LoanRepository(FundoDbContext context)
        {
            _context = context;
        }

        public async Task<Loan> GetByIdAsync(int id)
        {
            return await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loans.OrderByDescending(l => l.CreatedAt).ToListAsync();
        }

        public async Task<Loan> AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan> UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return loan;
        }
    }
}