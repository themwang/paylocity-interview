namespace PayRollCalculator.Models
{
    public class PayrollDeduction
    {
        public decimal TotalDeductionPerYear { get; set; }
        public decimal TotalDeductionPerPayCheck { get; set; }
        public decimal TotalNetPayPerPayCheck { get; set; }
    }
}
