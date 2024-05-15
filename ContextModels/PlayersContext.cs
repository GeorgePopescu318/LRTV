using LRTV.Models;
using Microsoft.EntityFrameworkCore;


namespace LRTV.ContextModels;

public class PlayersContext : DbContext {

    public PlayersContext(DbContextOptions<PlayersContext> options) : base(options) { 
    }

    public DbSet<PlayerModel> Players { get; set; }

    public DbSet<TeamModel> Teams { get; set; }
}
