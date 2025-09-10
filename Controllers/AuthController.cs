using System.Security.Cryptography;
using System.Text;
using DotnetAPI.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;


public class AuthController(IConfiguration config) : ControllerBase
{
    private readonly DataContextDapper _dapper = new(config);
    private readonly IConfiguration _config = config;
    [HttpPost("register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        if (userForRegistration.Password != userForRegistration.PasswordConfirm)
            return BadRequest("Passwords do not match");
        string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = @Email";
        IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists, new { userForRegistration.Email });
        Console.WriteLine(">>>");
        
        if (existingUsers.Any())
            return BadRequest("User already exists");
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value
        + Convert.ToBase64String(passwordSalt);

        byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

        string sqlAddAuth = @"INSERT INTO TutorialAppSchema.Auth (
            Email,
            PasswordHash,
            PasswordSalt
        ) VALUES (
            @Email,
            @PasswordHash,
            @PasswordSalt
        )";
        _dapper.ExecuteSqlWithParams(sqlAddAuth, new
        {
            Email = userForRegistration.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        });
        return Ok("register");
    }

    [HttpPost("login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        string sqlForHashAndSalt = @"SELECT 
        [PasswordHash],
        [PasswordSalt] 
        FROM TutorialAppSchema.Auth WHERE Email = @Email";
        UserForLoginConfirmationDto userForConfirmation = _dapper
            .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt, new
            {
                userForLogin.Email
            });
        byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);
        // if (passwordHash == userForConfirmation.PasswordHash) won't work
        for (int index = 0; index < passwordHash.Length; index++)
        {
            if (passwordHash[index] != userForConfirmation.PasswordHash[index])
            {
                return  StatusCode(401, "Incorrect password!");
            }
            
        }
        return Ok("login");
    }
    private byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value
        + Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        );
    }
}