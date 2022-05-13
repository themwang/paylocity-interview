using PayRollCalculator.Models;

namespace PayRollCalculator.Calculation
{
    public class DeductionCalculator : IDeductionCalculator
    {
        //assumptions
        private const int NumberOfPayPeriodsPerYear = 26;
        private const decimal PayAmount = 2000;
        private const decimal EmployeeDeductionPerYear = 1000;
        private const decimal DependentDeductionPerYear = 500;
        private const decimal Discount = .1m;

        public Task<PayrollDeduction> GetPayrollDeduction(Employee employee)
        {
            var deductions = EmployeeDeductionPerYear;

            if (employee.Dependents.Count > 0)
            {
                deductions += DependentDeductionPerYear * employee.Dependents.Count;
            }
            
            if (ShouldGetDiscount(employee))
            {
                deductions = deductions * (1 - Discount);
            }

            var deductionPerPayPeriod = decimal.Round(deductions / NumberOfPayPeriodsPerYear, 2);

            return Task.FromResult(new PayrollDeduction
            {
                TotalDeductionPerYear = deductions,
                TotalDeductionPerPayCheck = deductionPerPayPeriod,
                TotalNetPayPerPayCheck = PayAmount - deductionPerPayPeriod,
            });
        }

        private bool ShouldGetDiscount(Employee employee)
        {
            bool result = false;

            if (employee.EmployeeName.StartsWith("A", StringComparison.InvariantCultureIgnoreCase)
                || employee.Dependents.Exists(d => d.Name.StartsWith("A", StringComparison.InvariantCultureIgnoreCase)))
            {
                result = true;
            }

            return result;
        }
    }
}
