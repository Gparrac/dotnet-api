using System.Data.SqlTypes;
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;
    IUserRepository _userRepository;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _entityFramework = new DataContextEF(config);
        _userRepository = userRepository;
        // this is mandatory to allow our mapper logging in exception cases ℹ️
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Example: Add console logging
        });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserDTO, User>();
        }, loggerFactory);
        _mapper = new Mapper(mapperConfig);

    }
    [HttpGet("users")]
    public IEnumerable<User> GetUsers()
    {
        return _entityFramework.Users.ToList<User>();

    }
    [HttpGet("users/{userId}")]
    public User GetSingleUser(int userId)
    {

        User? user = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
        if (user != null)
        {
            return user;
        }
        throw new Exception("User not found");
    }


    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users.Where(u => u.UserId == user.UserId).FirstOrDefault<User>();
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_userRepository.SaveChanges())
                return Ok();
        }
        throw new Exception("User not found");
    }
    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_userRepository.SaveChanges())
                return Ok();
        }
        throw new Exception("User not found");
    }
    [HttpPost("AddUser")]
    public IActionResult AddUser(CreateUserDTO user)
    {
        // with mapper ℹ️
        User userDb = _mapper.Map<User>(user);
        // manual mapping ℹ️
        // User userDb = new()
        // {
        //     Active = user.Active,
        //     FirstName = user.FirstName,
        //     LastName = user.LastName,
        //     Email = user.Email,
        //     Gender = user.Gender
        // };
        /* 
            by repository is worth if we have more coplex logic but
            but in this cases is more straightfoward to call the EF ⚠️
        */
        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
            return Ok(new { userId = userDb.UserId });

        return BadRequest();
    }

}
