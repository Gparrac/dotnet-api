using System.Data.SqlTypes;
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserJobInfoEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;
    public UserJobInfoEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
        // this is mandatory to allow our mapper logging in exception cases ℹ️
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Example: Add console logging
        });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserJobInfoDTO, UserJobInfo>();
        }, loggerFactory);
        _mapper = new Mapper(mapperConfig);

    }
    [HttpGet("user-jobs-info")]
    public IEnumerable<UserJobInfo> GetUserJobsInfo()
    {
        return _entityFramework.UserJobInfo.ToList<UserJobInfo>();

    }
    [HttpGet("user-jobs-info/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {

        UserJobInfo? user = _entityFramework.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();
        if (user != null)
        {
            return user;
        }
        throw new Exception("User not found");
    }


    [HttpPut("user-jobs-info")]
    public IActionResult EditUser(UserJobInfo user)
    {
        UserJobInfo? userDb = _entityFramework.UserJobInfo.Where(u => u.UserId == user.UserId).FirstOrDefault<UserJobInfo>();
        if (userDb != null)
        {
            userDb.JobTitle = user.JobTitle;
            userDb.Department= user.Department;

            if (_entityFramework.SaveChanges() > 0)
                return Ok();
        }
        throw new Exception("User not found");
    }
    [HttpDelete("user-jobs-info")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        UserJobInfo? userDb = _entityFramework.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();
        if (userDb != null)
        {
            _entityFramework.UserJobInfo.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
                return Ok();
        }
        throw new Exception("User not found");
    }
    [HttpPost("user-jobs-info")]
    public IActionResult AddUser(CreateUserDTO user)
    {
        // with mapper ℹ️
        UserJobInfo userDb = _mapper.Map<UserJobInfo>(user);
        // manual mapping ℹ️
        // User userDb = new()
        // {
        //     Active = user.Active,
        //     FirstName = user.FirstName,
        //     LastName = user.LastName,
        //     Email = user.Email,
        //     Gender = user.Gender
        // };
        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
            return Ok(new { userId = userDb.UserId });

        return BadRequest();
    }

}
