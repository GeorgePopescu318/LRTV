using LRTV.Models;
using Microsoft.EntityFrameworkCore;

namespace LRTV.ContextModels;

public class TeamsContext : DbContext { 

    public TeamsContext(DbContextOptions<TeamsContext> options) : base(options)
    {
    }

    public DbSet<TeamModel> Teams { get; set; }
}
