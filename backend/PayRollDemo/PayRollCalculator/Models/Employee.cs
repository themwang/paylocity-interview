namespace PayRollCalculator.Models
{
    public class Employee
    {
        public string EmployeeName { get; set; } = "";
        public string EmployeeId { get; set; } = "";
        public List<Dependent> Dependents { get; set; } = new List<Dependent>();
    }
}
