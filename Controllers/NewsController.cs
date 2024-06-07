using CloudinaryDotNet;
using LRTV.ContextModels;
using LRTV.Interfaces;
using LRTV.Models;
using LRTV.Services;
using LRTV.ViewModels;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace LRTV.Controllers;

public class NewsController : Controller
{
    private readonly PlayersContext _context;
    private readonly IPhotoService _photoService;
    public List<NewsModel>? ListNews { get; set; }
    public NewsModel? CurrentNews { get; set; }
    public  NewsController(PlayersContext context, IPhotoService photoService)
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
        
        CurrentNews = _context.News.Where(news => news.Id == NewsId).Include(news => news.Cathegory).FirstOrDefault();
        //var comments = ViewComments(NewsId);

        if (CurrentNews == null)
        {
            return RedirectToAction("Error", "Home");
        } 
        return View(CurrentNews);
    }

    [HttpGet]
    public IActionResult AddNews()
    {
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
        return View("News", existingNews);
    }

    [HttpGet]
    public IActionResult DeleteNews(int NewsId)
    {
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
