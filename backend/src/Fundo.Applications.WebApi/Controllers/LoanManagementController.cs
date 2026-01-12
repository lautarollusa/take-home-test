using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Domain.Entities;
using Fundo.Services;

namespace Fundo.Applications.WebApi.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoanManagementController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanManagementController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> Create([FromBody] CreateLoanRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loan = await _loanService.CreateLoanAsync(request.Amount, request.ApplicantName);
                return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Loan>>> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoanById(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
                return NotFound(new { error = $"Loan with id {id} not found." });

            return Ok(loan);
        }

        [HttpPost("{id}/payment")]
        public async Task<ActionResult<Loan>> MakePayment(int id, [FromBody] PaymentRequest request)
        {
            try
            {
                var loan = await _loanService.MakePaymentAsync(id, request.PaymentAmount);
                return Ok(loan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class CreateLoanRequest
    {
        public decimal Amount { get; set; }
        public string? ApplicantName { get; set; }
    }

    public class PaymentRequest
    {
        public decimal PaymentAmount { get; set; }
    }
}