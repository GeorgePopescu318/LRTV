using LRTV.Models;

namespace LRTV.ViewModels;

public class MatchTeamLineupsViewModel
{
    public MatchesModel Match { get; set; }
    public List<PlayerModel> LineupTeam1 { get; set; }
    public List<PlayerModel> LineupTeam2 { get; set; }
    public List<string> MapNames { get; set; }
}
