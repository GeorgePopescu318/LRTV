using LRTV.Models;

namespace LRTV.ViewModels;

public class TeamViewModel
{
    public TeamModel Team { get; set; }
    public List<PlayerModel> Players { get; set; }

}
