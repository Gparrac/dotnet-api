using System.Data.SqlTypes;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserSalaryController(IConfiguration config) : ControllerBase
{
    DataContextDapper _dapper = new (config);
    
    [HttpGet("user-salaries")]
    public IEnumerable<UserSalary> GetUsers()
    {
        string sql = @"
            SELECT  [UserId]
            , [Salary]
            FROM  TutorialAppSchema.UsersSalary;
        ";
        return _dapper.LoadData<UserSalary>(sql);

    }
    [HttpGet("user-salaries/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        string sql = @$"
            SELECT  [UserId]
            , [Salary]      
            FROM  TutorialAppSchema.UsersSalary
            WHERE [UserId] = @userId;
        ";
        return _dapper.LoadDataSingle<UserSalary>(sql, new { userId});
    }

    [HttpPut("user-salaries")]
    public IActionResult EditUser(UserSalary user)
    {
        string sql = @"
    UPDATE TutorialAppSchema.Users
    SET [Salary] = @Salary
    WHERE [UserId] = @UserId;
";

        bool awsExecuted = _dapper.ExecuteSqlWithParams(sql, new
        {
            user.Salary,
            user.UserId
        });
        if (awsExecuted)
            return Ok();
        return BadRequest();   
    }
    [HttpDelete("user-salaries")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users
        WHERE [UserId] = @UserId; 
";

        bool awsExecuted = _dapper.ExecuteSqlWithParams(sql, new
        {
            UserId = userId
        });
        if (awsExecuted)
            return Ok();
        return BadRequest();   
    }
    [HttpPost("user-salaries")]
    public IActionResult AddUserSalary(CreateUserSalaryDTO user)
    {
              string sql = @"
    INSERT INTO TutorialAppSchema.Users
    ([Salary]) VALUES (
        @Salary
    )";
    bool awsExecuted = _dapper.ExecuteSqlWithParams(sql, new
        {
            user.Salary
        });
           
        if (awsExecuted)
            return Ok();
        return BadRequest();  

    }

}
