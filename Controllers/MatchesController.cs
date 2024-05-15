using LRTV.ContextModels;
using LRTV.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using LRTV.Interfaces;
using LRTV.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;


namespace LRTV.Controllers;
public class MatchesController : Controller
{
    private readonly PlayersContext _context;
    public List<MatchesModel>? ListMatches { get; set; }
    public MatchesModel? matchCurent { get; set; }
    public MatchesController(PlayersContext context)
    {
        _context = context;
    }

    public IActionResult DisplayImage(string imageName)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
        var imageFileStream = System.IO.File.OpenRead(imagePath);
        return File(imageFileStream, "image/jpeg");
    }


    [HttpGet]
    public IActionResult Index()
    {

        ListMatches = _context.Matches.Include(match => match.Team1).Include(match => match.Team2).Include(match =>  match.Map).ToList();
        if (ListMatches == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(ListMatches);
    }

    [HttpGet]
    public IActionResult Match(int matchId)
    {
        matchCurent = _context.Matches.Where(match => match.Id == matchId).Include(match => match.Team1).Include(match => match.Team2).Include(match => match.Map).FirstOrDefault();
        if (matchCurent == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(matchCurent);
    }

    [HttpGet]
    public IActionResult AddMatch()
    {

        List<SelectListItem> team1 = _context.Teams
            .Select(team1 => new SelectListItem { Text = team1.Name, Value = team1.Id.ToString() }).ToList();

        List<SelectListItem> team2 = _context.Teams
            .Select(team2 => new SelectListItem { Text = team2.Name, Value = team2.Id.ToString() }).ToList();

        List<SelectListItem> map = _context.Maps
            .Select(map => new SelectListItem { Text = map.Name, Value = map.Id.ToString() }).ToList();

        ViewBag.Teams1 = team1;
        ViewBag.Teams2 = team2;
        ViewBag.Maps = map;
        return View();
    }

    [HttpPost]
    public IActionResult AddMatches(MatchesModel newMatch)
    {

        if (!ModelState.IsValid)
        {
            List<SelectListItem> team1 = _context.Teams
            .Select(team1 => new SelectListItem { Text = team1.Name, Value = team1.Id.ToString() }).ToList();

            ViewBag.Teams1 = team1;

            List<SelectListItem> team2 = _context.Teams
            .Select(team2 => new SelectListItem { Text = team2.Name, Value = team2.Id.ToString() }).ToList();

            ViewBag.Teams1 = team2;

            List<SelectListItem> map = _context.Teams
            .Select(map => new SelectListItem { Text = map.Name, Value = map.Id.ToString() }).ToList();

            ViewBag.Maps = map;
            return View(newMatch);
        }

        newMatch.Team1 = _context.Teams.Where(team1 => team1.Id == newMatch.Team1Id).FirstOrDefault();
        newMatch.Team2 = _context.Teams.Where(team2 => team2.Id == newMatch.Team2Id).FirstOrDefault();
        newMatch.Map = _context.Maps.Where(map => map.Id == newMatch.MapId).FirstOrDefault();
        _context.Add(newMatch);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModifyPlayer(int matchId)
    {

        List<SelectListItem> team1 = _context.Teams
            .Select(team1 => new SelectListItem { Text = team1.Name, Value = team1.Id.ToString() }).ToList();
        ViewBag.Teams1 = team1;

        List<SelectListItem> team2 = _context.Teams
            .Select(team2 => new SelectListItem { Text = team2.Name, Value = team2.Id.ToString() }).ToList();
        ViewBag.Teams2 = team2;

        List<SelectListItem> map = _context.Teams
            .Select(map => new SelectListItem { Text = map.Name, Value = map.Id.ToString() }).ToList();
        ViewBag.Maps = map;

        MatchesModel? matches = _context.Matches.Where(matches => matches.Id == matchId).Include(matches => matches.Team1).Include(matches => matches.Team2).Include(matches => matches.Map).FirstOrDefault();

        if (matches == null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(matches);
    }

    [HttpPost]
    public IActionResult ModifyMatch(MatchesModel matches)
    {
        if (!ModelState.IsValid)
        {
            List<SelectListItem> team1 = _context.Teams
            .Select(team1 => new SelectListItem { Text = team1.Name, Value = team1.Id.ToString() }).ToList();

            ViewBag.Teams1 = team1;

            List<SelectListItem> team2 = _context.Teams
            .Select(team2 => new SelectListItem { Text = team2.Name, Value = team2.Id.ToString() }).ToList();

            ViewBag.Teams1 = team2;

            List<SelectListItem> map = _context.Teams
            .Select(map => new SelectListItem { Text = map.Name, Value = map.Id.ToString() }).ToList();

            ViewBag.Teams1 = map;
            return View(matches);
        }
        matches.Team1 = _context.Teams.Where(teams => teams.Id == matchCurent.Team1Id).FirstOrDefault();
        matches.Team2 = _context.Teams.Where(teams => teams.Id == matchCurent.Team2Id).FirstOrDefault();
        matches.Map = _context.Maps.Where(teams => teams.Id == matchCurent.MapId).FirstOrDefault();
        _context.Update(matches);
        _context.SaveChanges();
        return View("Player", matches);
    }

    [HttpGet]
    public IActionResult DeleteMatch(int matchId)
    {
        MatchesModel? matches = _context.Matches.Where(players => players.Id == matchId).Include(matches => matches.Team1).Include(matches => matches.Team2).Include(matches => matches.Map).FirstOrDefault();
        if (matches == null)
        {
            return RedirectToAction("Error", "Home");
        }
        _context.Remove(matches);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }


}
