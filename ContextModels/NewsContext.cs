using LRTV.Models;
using Microsoft.EntityFrameworkCore;

namespace LRTV.ContextModels;

public class NewsContext : DbContext
{
    public NewsContext(DbContextOptions<NewsContext> options): base(options) { 

    }

    
}
