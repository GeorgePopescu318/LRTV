using LRTV.Models;
using Microsoft.EntityFrameworkCore;


namespace LRTV.ContextModels;

public class PlayersContext : DbContext 
{

    public PlayersContext(DbContextOptions<PlayersContext> options) : base(options) { 
    }

    public DbSet<PlayerModel> Players { get; set; }

    public DbSet<TeamModel> Teams { get; set; }
    public DbSet<MatchesModel> Matches { get; set; }

    public DbSet<MapsModel> Maps { get; set; }
	public DbSet<NewsModel> News { get; set; }
	public DbSet<CathegoryModel> Cathegories { get; set; }
}
