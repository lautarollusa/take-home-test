using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Fundo.Domain.Entities;
using Fundo.Infrastructure.Repositories;
using Fundo.Services;

namespace Fundo.Tests
{
    public class LoanServiceTests
    {
        private readonly Mock<ILoanRepository> _repo = new();
        private readonly ILoanService _service;

        public LoanServiceTests()
        {
            _service = new LoanService(_repo.Object);
        }

        [Fact]
        public async Task CreateLoan_Valid_ReturnsActiveLoan()
        {
            var expected = new Loan
            {
                Id = 1,
                Amount = 1000m,
                CurrentBalance = 1000m,
                ApplicantName = "John Doe",
                Status = LoanStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _repo.Setup(r => r.AddAsync(It.IsAny<Loan>())).ReturnsAsync(expected);

            var result = await _service.CreateLoanAsync(1000m, "John Doe");

            Assert.NotNull(result);
            Assert.Equal(1000m, result.Amount);
            Assert.Equal(1000m, result.CurrentBalance);
            Assert.Equal("John Doe", result.ApplicantName);
            Assert.Equal(LoanStatus.Active, result.Status);
            _repo.Verify(r => r.AddAsync(It.IsAny<Loan>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task CreateLoan_InvalidAmount_Throws(decimal amount)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateLoanAsync(amount, "John Doe"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreateLoan_InvalidName_Throws(string name)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateLoanAsync(1000m, name));
        }

        [Fact]
        public async Task MakePayment_ReducesBalance()
        {
            var loan = new Loan { Id = 1, Amount = 1000m, CurrentBalance = 1000m, ApplicantName = "John Doe", Status = LoanStatus.Active };
            _repo.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);
            _repo.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).ReturnsAsync((Loan l) => l);

            var result = await _service.MakePaymentAsync(1, 300m);

            Assert.Equal(700m, result.CurrentBalance);
            Assert.Equal(LoanStatus.Active, result.Status);
            _repo.Verify(r => r.UpdateAsync(It.IsAny<Loan>()), Times.Once);
        }

        [Fact]
        public async Task MakePayment_ExactBalance_SetsPaid()
        {
            var loan = new Loan { Id = 1, Amount = 500m, CurrentBalance = 500m, ApplicantName = "John Doe", Status = LoanStatus.Active };
            _repo.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);
            _repo.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).ReturnsAsync((Loan l) => l);

            var result = await _service.MakePaymentAsync(1, 500m);

            Assert.Equal(0m, result.CurrentBalance);
            Assert.Equal(LoanStatus.Paid, result.Status);
        }

        [Fact]
        public async Task MakePayment_OnPaidLoan_Throws()
        {
            var loan = new Loan { Id = 1, Amount = 1000m, CurrentBalance = 0m, ApplicantName = "John Doe", Status = LoanStatus.Paid };
            _repo.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.MakePaymentAsync(1, 100m));
        }

        [Fact]
        public async Task MakePayment_NegativeAmount_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.MakePaymentAsync(1, -50m));
        }

        [Fact]
        public async Task MakePayment_LoanNotFound_Throws()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Loan)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.MakePaymentAsync(999, 50m));
        }
    }
}