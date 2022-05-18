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
            if (ShouldGetDiscount(employee.EmployeeName))
            {
                deductions = deductions * (1 - Discount);
            }

            foreach (var dependent in employee.Dependents)
            {
                if (!string.IsNullOrEmpty(dependent.Name))
                {
                    var dependentDeduction = DependentDeductionPerYear;
                    if (ShouldGetDiscount(dependent.Name))
                    {
                        dependentDeduction = dependentDeduction * (1 - Discount);
                    }
                    deductions += dependentDeduction;
                }
            }

            var deductionPerPayPeriod = decimal.Round(deductions / NumberOfPayPeriodsPerYear, 2);

            return Task.FromResult(new PayrollDeduction
            {
                TotalDeductionPerYear = deductions,
                TotalDeductionPerPayCheck = deductionPerPayPeriod,
                TotalNetPayPerPayCheck = PayAmount - deductionPerPayPeriod,
            });
        }

        private bool ShouldGetDiscount(String name)
        {
            bool result = false;

            if (name.StartsWith("A", StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
            }

            return result;
        }
    }
}
