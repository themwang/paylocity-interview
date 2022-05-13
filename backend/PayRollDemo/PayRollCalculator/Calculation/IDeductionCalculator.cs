using PayRollCalculator.Models;

namespace PayRollCalculator.Calculation
{
    public interface IDeductionCalculator
    {
        Task<PayrollDeduction> GetPayrollDeduction (Employee employee);
    }
}
