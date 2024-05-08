namespace LRTV.Models;

public class PlayerModel {
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string? CurrentTeam { get; set; }
    public string Achievements { get; set; }

    public float Rating { get; set; }
    public float Headshots { get; set; }
    public float KD { get; set; }
    public int MapsPlayed { get; set; }


}
