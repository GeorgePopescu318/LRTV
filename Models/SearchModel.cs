namespace LRTV.Models;

public class SearchModel
{
	public string Id { get; set; }

	public List<PlayerModel> Players { get; set; }
	public List<TeamModel> Teams { get; set; }

	public List<NewsModel> News { get; set; }


}
