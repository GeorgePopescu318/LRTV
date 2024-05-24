using LRTV.Models;

namespace LRTV.ViewModels;

public class ModifyPlayerViewModel
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int TeamID { get; set; }
    public TeamModel? CurrentTeam { get; set; }
    public string Achievements { get; set; }
    public float Rating { get; set; }
    public float Headshots { get; set; }
    public float KD { get; set; }
    public int MapsPlayed { get; set; }
    public string? Url { get; set; }
    public IFormFile? Image { get; set; }
}
