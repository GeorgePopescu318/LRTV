using LRTV.Models;
using Microsoft.EntityFrameworkCore;

namespace LRTV.ContextModels;




public class UsersContext : DbContext
{ 
    

    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

    public DbSet<UserModel> User { get; set; }
}


