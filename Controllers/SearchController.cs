using LRTV.ContextModels;
using LRTV.Interfaces;
using LRTV.Models;
using Microsoft.AspNetCore.Mvc;

namespace LRTV.Controllers;

public class SearchController : Controller
{
	private readonly PlayersContext _context;

	public SearchController(PlayersContext context)
	{
		_context = context;
	}
	[HttpGet]
	public async Task<IActionResult> Index(string SearchString)
	{
		ViewData["CurrentFlilter"] = SearchString;
		SearchModel search = new SearchModel();
		var players = from player in _context.Players select player;
		var teams = from team in _context.Teams select team;
		var news = from article in _context.News select article;
		if (!String.IsNullOrEmpty(SearchString) ) { 
			players = players.Where(player => player.Nickname.Contains(SearchString));
			search.Players = players.ToList();
			teams = teams.Where(team => team.Name.Contains(SearchString));
			search.Teams = teams.ToList();
			news = news.Where(article => article.Title.Contains(SearchString));
			search.News = news.ToList();
		}
		else
		{
			search.Teams = teams.ToList();
			search.Players = players.ToList();
			search.News = news.ToList();
		}
		return View(search);
	}
}
