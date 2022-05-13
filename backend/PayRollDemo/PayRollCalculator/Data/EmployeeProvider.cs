using PayRollCalculator.Models;

namespace PayRollCalculator.Data
{
    public class EmployeeProvider : IEmployeeProvider
    {
        // Should save this in a database
        private Dictionary<string, Employee> _employees = new Dictionary<string, Employee>();

        public Task<List<string>> GetEmployeeDependentTypes()
        {
            var dependentTypes = new List<string> {
                DependentType.Spouse,
                DependentType.Parent,
                DependentType.Child,
                DependentType.Other };

            return Task.FromResult(dependentTypes);
        }

        public Task<EmployeeResult> GetEmployee(string employeeId)
        {
            if (_employees.ContainsKey(employeeId))
            {
                var employee = _employees[employeeId];

                return Task.FromResult(new EmployeeResult { employee = employee });
            }
            return Task.FromResult(new EmployeeResult());
        }

        public Task SaveEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (_employees.ContainsKey(employee.EmployeeId.ToString()))
            {
                var employeeToUpdate = _employees[employee.EmployeeId.ToString()];
                employeeToUpdate.EmployeeName = employee.EmployeeName;
                employeeToUpdate.Dependents = employee.Dependents;
            }
            else
            {
                _employees.Add(employee.EmployeeId.ToString(), employee);
            }

            return Task.CompletedTask;
        }
    }
}
