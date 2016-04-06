﻿using CarFuel.Models;
using CarFuel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CarFuel.DataAccess;

namespace CarFuel.Controllers
{
    public class CarsController : Controller
    {
        //private static List<Car> cars = new List<Car>();
        private ICarDb db;
        private CarService carService;

        public CarsController()
        {
            db = new CarDb();
            carService = new CarService(db);
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
    }
}