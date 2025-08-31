using System.Data.SqlTypes;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    [HttpGet("users")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT  [UserId]
            , [FirstName]
            , [LastName]
            , [Email]
            , [Gender]
            , [Active]
            FROM  TutorialAppSchema.Users;
        ";
        return _dapper.LoadData<User>(sql);

    }
    [HttpGet("users/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @$"
            SELECT  [UserId]
            , [FirstName]
            , [LastName]
            , [Email]
            , [Gender]
            , [Active]            
            FROM  TutorialAppSchema.Users
            WHERE [UserId] = {userId};
        ";
        return _dapper.LoadDataSingle<User>(sql);
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
    UPDATE TutorialAppSchema.Users
    SET [FirstName] = @FirstName,
        [LastName]  = @LastName,
        [Email]     = @Email,
        [Gender]    = @Gender,
        [Active]    = @Active
    WHERE [UserId] = @UserId;
";

        bool awsExecuted = _dapper.ExecuteSqlWithParams(sql, new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Gender,
            user.Active,
            user.UserId
        });
        if (awsExecuted)
            return Ok();
        return BadRequest();   
    }
    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users
        WHERE [UserId] = @UserId;3eeeeeeeeeeee 
";

        bool awsExecuted = _dapper.ExecuteSqlWithParams(sql, new
        {
            UserId = userId
        });
        if (awsExecuted)
            return Ok();
        return BadRequest();   
    }
    [HttpPost("AddUser")]
    public IActionResult AddUser(CreateUserDTO user)
    {
              string sql = @"
    INSERT INTO TutorialAppSchema.Users
    ([FirstName],
    [LastName],
    [Email],
    [Gender],
    [Active]) VALUES (
        @FirstName,
        @LastName,
        @Email,
        @Gender,
        @Active
    )";
    bool awsExecuted = _dapper.ExecuteSqlWithParams(sql, new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Gender,
            user.Active
        });
        if (awsExecuted)
            return Ok();
        return BadRequest();  

    }

}
