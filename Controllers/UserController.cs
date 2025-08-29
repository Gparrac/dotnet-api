using System.Data.SqlTypes;
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



}
