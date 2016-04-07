using CarFuel.Models;
using CarFuel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CarFuel.DataAccess;
using System.Net;

namespace CarFuel.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = new Guid(User.Identity.GetUserId());
            IEnumerable<Car> cars = carService.GetCarsByMember(userId);
            return View(cars);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Car item)
        {
            var userId = new Guid(User.Identity.GetUserId());

            try
            {
                carService.AddCar(item, userId);
            }
            catch (OverQuotaException ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            var userId = new Guid(User.Identity.GetUserId());
            var c = carService.GetCarsByMember(userId).SingleOrDefault(x => x.Id == Id);

            return View(c);
        }
    }
}