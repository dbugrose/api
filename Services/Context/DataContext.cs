using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Context
{
      public class DataContext: DbContext
    {
      public DataContext(DbContextOptions options) : base(options)
      {
        
      }  
    public DbSet<UserModel> UserInfo {get; set;}
    public DbSet<TodoModel> TodoInfo {get; set;}
    public DbSet<StatsModel> StatsInfo {get; set;}
    public DbSet<FriendRequestModel> FriendRequestInfo {get; set;}
    public DbSet<BlocksModel> BlockedInfo {get; set;}
    public DbSet<HealthModel> HealthInfo {get; set;}

    }
}