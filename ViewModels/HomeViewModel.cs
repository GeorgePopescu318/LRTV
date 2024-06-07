using LRTV.Models;

namespace LRTV.ViewModels;

public class HomePageViewModel
{
    public List<TeamModel> Teams { get; set; }
    public List<MatchesModel> Matches { get; set; }
    public List<NewsModel> News { get; set; }
    public PlayerModel Player { get; set; }
}