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


namespace LRTV.Controllers;
public class PlayerController : Controller {
    private readonly PlayersContext _context;
    private readonly IPhotoService _photoService;

    public List<PlayerModel>? Players { get; set; }
    public PlayerModel? playerCurent { get; set; }
    public PlayerController(PlayersContext context, IPhotoService photoService) {
        _context = context;
        _photoService = photoService;
    }

    public IActionResult DisplayImage(string imageName)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
        var imageFileStream = System.IO.File.OpenRead(imagePath);
        return File(imageFileStream, "image/jpeg");
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
    public async Task<IActionResult> AddPlayer(CreatePlayerViewModel newPlayer) {
        ////string ceva = "ceva";
        //newPlayer.Image = true;
        ////newPlayer.Image = ceva;
        //ValidationContext vc = new ValidationContext(newPlayer);
        //ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
        //bool isValid = Validator.TryValidateProperty(newPlayer.Image, vc, results); // Validates the property using the previously created context.

        //ModelState.Remove("Image");
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsyncPlayers(newPlayer.Image);
            var playerVM = new PlayerModel
            {
                Nickname = newPlayer.Nickname,
                Name = newPlayer.Name,
                Age = newPlayer.Age,
                CurrentTeam = newPlayer.CurrentTeam,
                Achievements = newPlayer.Achievements,
                Rating = newPlayer.Rating,
                Headshots = newPlayer.Headshots,
                KD = newPlayer.KD,
                MapsPlayed = newPlayer.MapsPlayed,
                Image = result.Url.ToString()

            };
            _context.Players.Add(playerVM);
            _context.SaveChanges(); 
            return RedirectToAction("Index");

        }
        else
        {
            ModelState.AddModelError("", "Photo s a dus drq");
        }
        return View(newPlayer);

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
