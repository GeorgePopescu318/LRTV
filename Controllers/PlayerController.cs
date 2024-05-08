using LRTV.ContextModels;
using LRTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LRTV.Controllers;
public class PlayerController : Controller {
    private readonly PlayersContext _context;
    public List<PlayerModel>? Players { get; set; }
    public PlayerModel? playerCurent { get; set; }
    public PlayerController(PlayersContext context) {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index() {
        Players = _context.Players.ToList();
        if (Players == null) {
            return RedirectToAction("Error", "Home");
        }
        return View(Players);
    }

    [HttpGet]
    public IActionResult Player(int playerId)
    {
        playerCurent = _context.Players.Where(player => player.Id == playerId).FirstOrDefault();
        if (playerCurent == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(playerCurent);
    }

    [HttpGet]
    public IActionResult AddPlayer()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPlayer(PlayerModel newPlayer) {
        if (!ModelState.IsValid)
        {
            return View(newPlayer);
        }
        _context.Add(newPlayer);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModifyPlayer(int playerId) { 
        PlayerModel? player = _context.Players.Where(player => player.Id == playerId).FirstOrDefault();
            
        if (player == null) {
            return RedirectToAction("Error", "Home");
        }

        return View(player);
    }

    [HttpPost]
    public IActionResult ModifyPlayer(PlayerModel player)
    {
        if (!ModelState.IsValid)
        {
            return View(player);
        }
        _context.Update(player);
        _context.SaveChanges();
        return View("Player", player);
    }

    [HttpGet]
    public IActionResult DeletePlayer(int playerId) 
    { 
        PlayerModel? player = _context.Players.Where(player => player.Id == playerId).FirstOrDefault();
        if (player == null)
        {
            return RedirectToAction("Error", "Home");
        }
        _context.Remove(player);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
