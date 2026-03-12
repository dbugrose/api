using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Models.DTO;
using api.Services.Context;
using Microsoft.AspNetCore.Identity;
namespace api.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;
        public UserService(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public async Task<bool> CreateAccount(UserDTO newUser)
        {
            if (await DoesUserExist(newUser.Username))
                return false;

            PasswordDTO encryptedPassword = HashPassword(newUser.Password);

            UserModel user = new UserModel
            {
                Username = newUser.Username,
                Hash = encryptedPassword.Hash,
                Salt = encryptedPassword.Salt,

                Health = new HealthModel
                {
                    Username = newUser.Username,
                    Health = 100
                },

                Stats = new StatsModel
                {
                    Username = newUser.Username,
                    MonstersSlain = 0,
                    TasksCompleted = 0,
                    EasyTasks = 0,
                    MedTasks = 0,
                    HardTasks = 0
                }

            };

            await _dataContext.UserInfo.AddAsync(user);

            return await _dataContext.SaveChangesAsync() > 0;
        }

        private async Task<bool> DoesUserExist(string username)
        {
            return await _dataContext.UserInfo.SingleOrDefaultAsync(user => user.Username == username) != null;
        }

        private static PasswordDTO HashPassword(string password)
        {
            byte[] SaltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(SaltBytes);

            string hash;

            using (var derivedBytes = new Rfc2898DeriveBytes(password, SaltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(derivedBytes.GetBytes(32));
            }

            return new PasswordDTO
            {
                Salt = salt,
                Hash = hash
            };
        }

        public async Task<string> Login(UserDTO user)
        {
            UserModel currentUser = await GetUserInfoByUsernameAsync(user.Username);

            if (currentUser == null) return null;

            if (!VerifyPassword(user.Password, currentUser.Salt, currentUser.Hash)) return null;

            return GenerateJWT(new List<Claim>());
        }

        private string GenerateJWT(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "slaythemonster2526dor-ghhnbvgkercbd0gx.westus3-01.azurewebsites.net/",
                audience: "slaythemonster2526dor-ghhnbvgkercbd0gx.westus3-01.azurewebsites.net/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltByte = Convert.FromBase64String(salt);

            string checkHash;

            using (var derivedBytes = new Rfc2898DeriveBytes(password, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                checkHash = Convert.ToBase64String(derivedBytes.GetBytes(32));
                return hash == checkHash;
            }
        }



        public async Task<UserModel> GetUserInfoByUsernameAsync(string username) => await _dataContext.UserInfo.SingleOrDefaultAsync(user => user.Username == username);

        public async Task<UserInfoDTO> GetUserByUsername(string username)
        {
            var currentUser = await _dataContext.UserInfo.SingleOrDefaultAsync(user => user.Username == username);

            UserInfoDTO user = new();
            user.Id = currentUser.Id;
            user.Username = currentUser.Username;
            return user;
        }

    }
}