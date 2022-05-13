using PayRollCalculator.Calculation;
using PayRollCalculator.Data;
using PayRollCalculator.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IEmployeeProvider, EmployeeProvider>();
builder.Services.AddScoped<IDeductionCalculator, DeductionCalculator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/employee", async (Employee employee, IEmployeeProvider employeeProvider) =>
{
    await employeeProvider.SaveEmployee(employee);
    return Results.Ok();
});

app.MapGet("/employee/dependent-types", async (IEmployeeProvider employeeProvider) =>
{
    var dependentTypes = await employeeProvider.GetEmployeeDependentTypes();
    return Results.Ok(dependentTypes);
});

app.MapGet("/employee/{id}", async (String id, IEmployeeProvider employeeProvider) =>
{
    var employeeResult = await employeeProvider.GetEmployee(id); 
    if(employeeResult.isFound)
    {
        return Results.Ok(employeeResult.employee);
    }
    return Results.NotFound();
});

app.MapGet("/employee/{id}/payroll-deduction", async (String id, IEmployeeProvider employeeProvider, IDeductionCalculator deductionCalculator) =>
{    
    var employeeResult = await employeeProvider.GetEmployee(id);
    if (employeeResult.isFound)
    {
        var payrollDeduction = await deductionCalculator.GetPayrollDeduction(employeeResult.employee!);
        return Results.Ok(payrollDeduction);
    }
    return Results.NotFound();
});

app.Run();
