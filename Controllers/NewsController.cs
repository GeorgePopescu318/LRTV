﻿using LRTV.ContextModels;
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
    private readonly NewsContext _newsContext;
    private readonly IPhotoService _photoService;
    public List<NewsModel>? ListNews { get; set; }
    public NewsModel? CurrentNews { get; set; }
    public  NewsController(NewsContext newsContext, IPhotoService photoService)
    {
        _newsContext = newsContext;
        _photoService = photoService;
    }
    [HttpGet]
    public IActionResult Index()
    {
		ListNews = _newsContext.News.Include(news => news.Cathegory).ToList();
        if (ListNews == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(ListNews);
    }

    [HttpGet]
    public IActionResult News(int NewsId)
    {
		CurrentNews = _newsContext.News.Where(news => news.Id == NewsId).Include(news => news.Cathegory).FirstOrDefault();
        if (CurrentNews == null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(CurrentNews);
    }

    [HttpGet]
    public IActionResult AddNews()
    {
        List<SelectListItem> cathegories = _newsContext.Cathegories
            .Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

        ViewBag.Cathegories = cathegories;
        return View(CurrentNews);
    }
 //   [HttpPost]
 //   public IActionResult AddNews(NewsModel newNews) { 

 //       if (!ModelState.IsValid)
 //       {
	//		List<SelectListItem> cathegories = _newsContext.Cathegories
	//		.Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

	//		ViewBag.Cathegories = cathegories;
	//		return View(newNews);
 //       }

 //       newNews.Cathegory = _newsContext.Cathegories.Where(cathegories => cathegories.Id== newNews.CathegoryID).FirstOrDefault();
 //       _newsContext.Add(newNews);
 //       _newsContext.SaveChanges();
 //       return RedirectToAction("Index");
	//}

    public async Task<IActionResult> AddNews(CreateNewsViewModel newNews)
    {
        
        if (ModelState.IsValid)
        {
            newNews.Cathegory = _newsContext.Cathegories.Where(cathegories => cathegories.Id == newNews.CathegoryID).FirstOrDefault();
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
            
            _newsContext.News.Add(newsVM);
            _newsContext.SaveChanges();
            return RedirectToAction("Index");

        }
        else
        {
            List<SelectListItem> cathegories = _newsContext.Cathegories
            .Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();
            ViewBag.Cathegories = cathegories;
            ModelState.AddModelError("", "Photo s a dus drq");
        }
        
        return View(newNews);

    }


    [HttpGet]
    public IActionResult ModifyNews(int NewsId)
    {
		List<SelectListItem> cathegories = _newsContext.Cathegories
			.Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

		ViewBag.Cathegories = cathegories;

		NewsModel? news = _newsContext.News.Where(news => news.Id == NewsId).Include(news => news.Cathegory).FirstOrDefault();
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
			List<SelectListItem> cathegories = _newsContext.Cathegories
			.Select(cathegories => new SelectListItem { Text = cathegories.Name, Value = cathegories.Id.ToString() }).ToList();

			ViewBag.Cathegories = cathegories;
			return View(news);
		}
		news.Cathegory = _newsContext.Cathegories.Where(cathegories => cathegories.Id == news.CathegoryID).FirstOrDefault();
        _newsContext.Update(news);
        _newsContext.SaveChanges();
        return View("News",news);
	}

    [HttpGet]
    public IActionResult DeleteNews(int NewsId)
    {
		NewsModel? news = _newsContext.News.Where(news => news.Id == NewsId).Include(news => news.Cathegory).FirstOrDefault();
		if (news == null)
        {
			return RedirectToAction("Error", "Home");
		}
        _newsContext.Remove(news);
        _newsContext.SaveChanges();
        return RedirectToAction("Index");
	}
}
