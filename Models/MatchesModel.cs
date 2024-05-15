namespace LRTV.Models;

public class MatchesModel
{
    public int Id { get; set; }
    public int? Team1Id { get; set; }

    public TeamModel? Team1 { get; set; }

    public int? Team2Id { get; set; }
    public TeamModel? Team2 { get; set; }

    public DateTime DateTime { get; set; }

    public int ScoreTeam1 { get; set; }

    public int ScoreTeam2 { get; set;}

    public int? MapId { get; set; }
    public MapsModel? Map { get; set;}
}
