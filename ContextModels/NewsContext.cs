using LRTV.Models;
using Microsoft.EntityFrameworkCore;

namespace LRTV.ContextModels;

public class NewsContext : DbContext
{
    public NewsContext(DbContextOptions<NewsContext> options): base(options) { 

    }

    public DbSet<NewsModel> News { get; set; }
    public DbSet<CathegoryModel> Cathegories { get; set;}
}
