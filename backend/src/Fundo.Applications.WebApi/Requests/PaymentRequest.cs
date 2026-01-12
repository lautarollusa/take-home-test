using System.ComponentModel.DataAnnotations;

public class PaymentRequest
{
    [Required(ErrorMessage = "Payment amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
    public decimal PaymentAmount { get; set; }
}