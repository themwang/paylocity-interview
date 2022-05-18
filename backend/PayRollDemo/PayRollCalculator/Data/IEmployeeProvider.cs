using PayRollCalculator.Models;

namespace PayRollCalculator.Data
{
    public interface IEmployeeProvider
    {
        Task<string> SaveEmployee(Employee employee);
        Task<EmployeeResult> GetEmployee(string employeeId);
        Task<List<string>> GetEmployeeDependentTypes();
    }
}
