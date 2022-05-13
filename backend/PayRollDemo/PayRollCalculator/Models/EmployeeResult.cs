namespace PayRollCalculator.Models
{
    public class EmployeeResult
    {
        public Employee? employee { get; set; }
        public bool isFound => employee != null;
    }
}
