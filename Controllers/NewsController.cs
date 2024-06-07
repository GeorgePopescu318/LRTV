using CloudinaryDotNet;
using LRTV.ContextModels;
using LRTV.Interfaces;
using LRTV.Models;
using LRTV.Services;
using LRTV.ViewModels;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace LRTV.Controllers;

public class NewsController : Controller
{
    private readonly PlayersContext _context;
    private readonly IPhotoService _photoService;
    public List<NewsModel>? ListNews { get; set; }
    public NewsModel? CurrentNews { get; set; }
    public NewsController(PlayersContext context, IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }
    [HttpGet]
    public IActionResult Index()
    {
		ListNews = _context.News.Include(news => news.Cathegory).ToList();
        if (ListNews == null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View(ListNews);
    }

    [HttpGet]
    public IActionResult News(int NewsId)
    {

        var news = _context.News.Include(news => news.Cathegory).FirstOrDefault(news => news.Id == NewsId);
        if (news == null)
        {
            return RedirectToAction("Error", "Home");
        }

        var comments = _context.Comments.Where(comm => comm.newsId == NewsId).ToList();

        var viewModel = new News_CommentsViewModel
        {   
            News = news,
            Comments = comments
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddComment(int newsId, string commentText)
    {
        if (string.IsNullOrEmpty(commentText))
        {
            return RedirectToAction("News", new { newsId = newsId });
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Login", "Account");
        }

        if (!int.TryParse(userIdString, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _context.User.FindAsync(userId);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var news = await _context.News.FindAsync(newsId);
        if (news == null)
        {
            return NotFound();
        }

        var comment = new CommentsModel
        {
            userId = user.Id,
            userName = user.Username,
            text = commentText,
            postedDate = DateTime.Now,
            newsId = newsId,
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("News", new { newsId = newsId });
    }

    [HttpGet]
    public IActionResult AddNews()
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

        List<SelectListItem> cathegories = _context.Cathegories
            .Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

        ViewBag.Cathegories = cathegories;
        return View(CurrentNews);
    }

    public IActionResult SortCat(int? categoryId)
    {
        var news = _context.News.Include(n => n.Cathegory).AsQueryable();

        if (categoryId.HasValue)
        {
            news = news.Where(n => n.CathegoryID == categoryId);
        }

        var viewModel = news.ToList();
        ViewBag.SelectedCategoryId = categoryId;

        // Specify the name of the view explicitly
        return View("Index", viewModel);
    }

    [HttpPost]
    public IActionResult SortCatPost(int? categoryId)
    {
        return RedirectToAction("SortCat", new { categoryId });
    }


    public async Task<IActionResult> AddNews(CreateNewsViewModel newNews)
    {
        
        if (ModelState.IsValid)
        {
            newNews.Cathegory = _context.Cathegories.Where(cathegories => cathegories.Id == newNews.CathegoryID).FirstOrDefault();
            var result = await _photoService.AddPhotoAsyncNews(newNews.Image);
            var newsVM = new NewsModel
            {
                Title = newNews.Title,
                Lead = newNews.Lead,
                Body = newNews.Body,
                Author = newNews.Author,
                Data = newNews.Data,
                CathegoryID = newNews.CathegoryID,
                Cathegory = newNews.Cathegory,
                Image = result.Url.ToString()

            };
            
            _context.News.Add(newsVM);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        else
        {
            List<SelectListItem> cathegories = _context.Cathegories
            .Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();
            ViewBag.Cathegories = cathegories;
            ModelState.AddModelError("", "Photo s a dus drq");
        }
        
        return View(newNews);

    }


    [HttpGet]
    public IActionResult ModifyNews(int NewsId)
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
        List<SelectListItem> cathegories = _context.Cathegories
			.Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

		ViewBag.Cathegories = cathegories;

		NewsModel? news = _context.News.Where(news => news.Id == NewsId).Include(news => news.Cathegory).FirstOrDefault();
        if (news == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(news);
	}


    [HttpPost]
    public IActionResult ModifyNews(NewsModel news)
    {
        if (!ModelState.IsValid)
        {
            List<SelectListItem> cathegories = _context.Cathegories
                .Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

            ViewBag.Cathegories = cathegories;
            return View(news);
        }

        var existingNews = _context.News.FirstOrDefault(n => n.Id == news.Id);
        if (existingNews == null)
        {
            return RedirectToAction("Error", "Home");
        }

        // Update fields excluding the image
        existingNews.Title = news.Title;
        existingNews.Lead = news.Lead;
        existingNews.Author = news.Author;
        existingNews.Body = news.Body;
        existingNews.Data = news.Data;
        existingNews.CathegoryID = news.CathegoryID;
        existingNews.Cathegory = _context.Cathegories.FirstOrDefault(c => c.Id == news.CathegoryID);

        _context.Update(existingNews);
        _context.SaveChanges();
        return RedirectToAction("News", new { newsId = news.Id });
	}

    [HttpGet]
    public IActionResult DeleteNews(int NewsId)
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
        NewsModel? news = _context.News.Where(news => news.Id == NewsId).Include(news => news.Cathegory).FirstOrDefault();
		if (news == null)
        {
			return RedirectToAction("Error", "Home");
		}
        _context.Remove(news);
        _context.SaveChanges();
        return RedirectToAction("Index");
	}
}
