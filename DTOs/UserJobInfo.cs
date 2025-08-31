namespace DotnetAPI.DTOs;

public partial class CreateUserJobInfoDTO

{
    public string JobTitle { get; set; }
    public string Deparment { get; set; }
    public CreateUserJobInfoDTO()
    {
        JobTitle ??= "";
        Deparment ??= "";

    }
}