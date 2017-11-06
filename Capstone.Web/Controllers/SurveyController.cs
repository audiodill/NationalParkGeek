using Capstone.Web.DAL;
using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Web.Controllers
{
    public class SurveyController : Controller
    {
        private ISurvey dal;

        public SurveyController(ISurvey SurveyDal)
        {
            this.dal = SurveyDal;
        }

        public ActionResult Survey()
        {
            Survey model = new Survey();
            return View("Survey", model);
        }

        [HttpPost]
        public ActionResult Survey(Survey input)
        {
            if (!ModelState.IsValid)
            {
                return View("Survey", input);
            }
            else
            {
                bool model = dal.SubmitSurvey(input);
                return RedirectToAction("ParkFavorites");
            }

        }

        public ActionResult ParkFavorites()
        {
            List<Survey> model = dal.GetFavoriteParks();
            return View("ParkFavorites", model);
        }

        // GET: Survey
        public ActionResult Index()
        {
            return View();
        }
    }
}