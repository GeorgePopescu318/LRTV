namespace LRTV.ViewModels;

public class CreateTeamViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Ranking { get; set; }

    public string Region { get; set; }

    public IFormFile Image { get; set; }
}
