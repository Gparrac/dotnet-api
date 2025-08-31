namespace DotnetAPI.DTOs;

public partial class CreateUserSalaryDTO

{
    public string Salary { get; set; }

    public CreateUserSalaryDTO()
    {
        Salary ??= "";

    }
}