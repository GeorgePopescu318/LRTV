﻿using LRTV.ContextModels;
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
using CloudinaryDotNet;
using System.Security.Cryptography.Xml;


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


    [HttpGet]
    public IActionResult Index()
    {

        ListMatches = _context.Matches.Include(match => match.Team1).Include(match => match.Team2).Include(match => match.Map).ToList();
        if (ListMatches == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(ListMatches);
    }

    [HttpGet]
    public IActionResult Match(int matchId)
    {
        var match = _context.Matches
            .Include(match => match.Team1)
            .Include(match => match.Team2)
            .Include(match => match.Map)
            .FirstOrDefault(match => match.Id == matchId);

        if (match == null)
        {
            return NotFound();
        }

        var team1Lineup = ViewLineup(match.Team1.Id);
        var team2Lineup = ViewLineup(match.Team2.Id);

        var mapNames = _context.Maps.Select(map => map.Name).ToList();


        var viewModel = new MatchTeamLineupsViewModel
        {
            Match = match,
            LineupTeam1 = team1Lineup,
            LineupTeam2 = team2Lineup,
            MapNames = mapNames
        };

        return View(viewModel);
    }

    public List<PlayerModel> ViewLineup(int teamID)
    {
        var players = _context.Players.Where(player => player.TeamID == teamID).ToList();
        return players;
    }


    [HttpGet]
    public IActionResult AddMatch()
    {
        var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
        if (User.Identity.IsAuthenticated)
        {
            if (userRole.ToLower() == "member")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
        }
        else
            return RedirectToAction("AccessForbidden", "Home");

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
    public IActionResult AddMatch(MatchesModel newMatch)
    {

        if (!ModelState.IsValid)
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
            return View(newMatch);
        }

        newMatch.Team1 = _context.Teams.Where(team1 => team1.Id == newMatch.Team1Id).FirstOrDefault();
        newMatch.Team2 = _context.Teams.Where(team2 => team2.Id == newMatch.Team2Id).FirstOrDefault();
        newMatch.Map = _context.Maps.Where(map => map.Id == newMatch.MapId).FirstOrDefault();
        if (newMatch.Team1.Id == newMatch.Team2.Id)
        {
            ModelState.AddModelError("", "Pick different Teams");
            AddMatch();
            return View(newMatch);
        }
        else
        {
            _context.Add(newMatch);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult ModifyMatch(int matchId)
    {
        var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
        if (User.Identity.IsAuthenticated)
        {
            if (userRole.ToLower() == "member")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
        }
        else
            return RedirectToAction("AccessForbidden", "Home");
        List<SelectListItem> team1 = _context.Teams
            .Select(team1 => new SelectListItem { Text = team1.Name, Value = team1.Id.ToString() }).ToList();
        ViewBag.Teams1 = team1;

        List<SelectListItem> team2 = _context.Teams
            .Select(team2 => new SelectListItem { Text = team2.Name, Value = team2.Id.ToString() }).ToList();
        ViewBag.Teams2 = team2;

        List<SelectListItem> map = _context.Maps
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

            ViewBag.Teams2 = team2;

            List<SelectListItem> map = _context.Maps
            .Select(map => new SelectListItem { Text = map.Name, Value = map.Id.ToString() }).ToList();

            ViewBag.Maps = map;
            return View(matches);
        }
        matches.Team1 = _context.Teams.Where(teams => teams.Id == matches.Team1Id).FirstOrDefault();
        matches.Team2 = _context.Teams.Where(teams => teams.Id == matches.Team2Id).FirstOrDefault();
        matches.Map = _context.Maps.Where(maps => maps.Id == matches.MapId).FirstOrDefault();
        if (matches.Team1.Id == matches.Team2.Id)
        {
            ModelState.AddModelError("", "Pick different Teams");
            //AddMatch();
            return View(matches);
        }
        else
        {
            _context.Update(matches);
            _context.SaveChanges();
            return RedirectToAction("Match", new { matchId = matches.Id });
        }
    }

    [HttpGet]
    public IActionResult DeleteMatch(int matchId)
    {
        var userRole = User?.Claims?.FirstOrDefault(claim => claim.Type == "Role")?.Value ?? "";
        if (User.Identity.IsAuthenticated)
        {
            if (userRole.ToLower() == "member")
            {
                return RedirectToAction("AccessForbidden", "Home");
            }
        }
        else
            return RedirectToAction("AccessForbidden", "Home");
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