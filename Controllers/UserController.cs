using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Services.Context;
using api.Models;
using api.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userServices;

        public UserController(UserService userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]UserDTO user)
        {
            bool success = await _userServices.CreateAccount(user);

            if(success) return Ok(new {Success = true, Message = "User Created."});

            return BadRequest(new {Success = false, Message = "User Creation failed Email is already in use."});
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            var success = await _userServices.Login(user);

            if(success != null) return Ok(new {Token = success});

            return Unauthorized(new {Message = "Login was unsuccesful"});
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userServices.GetUserInfoByUsernameAsync(username);

            if(user != null) return Ok(user);

            return BadRequest(new {Message = "No user Found"});
        }


    }
}