using System;

namespace Fundo.Domain.Entities
{
    public enum LoanStatus
    {
        Active = 0,
        Paid = 1
    }

    public class Loan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ApplicantName { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
