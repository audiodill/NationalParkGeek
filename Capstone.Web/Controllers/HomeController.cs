using Capstone.Web.DAL;
using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        private IParkDAL dal;

        public HomeController(IParkDAL ParkDal)
        {
            this.dal = ParkDal;
        }

        // GET: Home
        public ActionResult Home()
        {
            List<Park> model = dal.GetAllParks();
            return View("Home", model);
        }

        public ActionResult Farenheit(string id)
        {
            string degree = "farenheit";
            if (!string.IsNullOrEmpty(degree))
            {
                Session["degree"] = degree;
            }
            degree = Session["degree"] as string;
            Session["degree"] = degree;
            Park park = dal.GetPark(id);
            List<Weather> weather = dal.GetFiveDayForecast(id, degree);
            Dictionary<Park, List<Weather>> model = new Dictionary<Park, List<Weather>>();
            model.Add(park, weather);
            return View("Detail", model);
        }

        public ActionResult Celsius(string id)
        {
            string degree = "Celsius";
            Session["degree"] = degree;
            
            Park park = dal.GetPark(id);
            List<Weather> weather = dal.GetFiveDayForecast(id, degree);
            Dictionary<Park, List<Weather>> model = new Dictionary<Park, List<Weather>>();
            model.Add(park, weather);
            return View("Detail", model);
        }
    }
}