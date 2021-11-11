using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SmartSATWeb.ViewModels;
using System;

namespace SmartSATWeb.Controllers
{
    public class VideosController : Controller
    {
        private readonly IVideoService _videoService;
        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }
        // GET: Videos
        public ActionResult Index(string searchText = "")
        {
            ViewData["CurrentFilter"] = searchText;

            return View(_videoService.GetList(searchText));
        }

        // GET: Videos/Details/5
        public ActionResult Details(int id)
        {
            var video = _videoService.GetById(id);

            var videoVM = new VideoVM
            {
                Id = video.Id,
                Titulo = video.Titulo,
                Descricao = video.Descricao,
                Url = video.Url,
                Categoria = video.Categoria,
                VideoId = string.Format("https://www.youtube.com/embed/{0}", video.Url.Split("v=")[1])
            };

            return View(videoVM);
        }

        // GET: Videos/Create
        public ActionResult Create()
        {
            return View(new Video());
        }

        // POST: Videos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Video video)
        {
            try
            {
                _videoService.Add(video);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Videos/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_videoService.GetById(id));
        }

        // POST: Videos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Video video)
        {
            try
            {
                _videoService.Update(video);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_videoService.GetById(id));
        }

        // POST: Videos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Video video)
        {
            try
            {
                _videoService.Remove(video);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}