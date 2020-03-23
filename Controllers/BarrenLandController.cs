using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using ASPNetframework2.DataControllers;
using ASPNetframework2.Models;

namespace ASPNetframework2.Controllers
{
	public class BarrenLandController : Controller
	{

		//[Route("FindFertileLand/{coordinates}")]
		public ActionResult FindFertileLand(LandInformation coordinates)
		{
			LandInformation landInformation = new LandInformation();
			//ViewBag.Submitted = false;
			if (HttpContext.Request.RequestType == "POST")
			{
				// Request is Post type; must be a submit
				var barrenLand = Request.Form["BarrenLand"];
				landInformation.BarrenLandInputString = barrenLand == null ? String.Empty : barrenLand;
				landInformation = BarrenLandDataController.GetAllFertileLand(landInformation);
				ViewBag.Submitted = true;
				ViewBag.Message = "Fertile land area search successfully completed.";
			}
			else
				ViewBag.Message = "Enter barren land coordinates to find the fertile land area.";

			return  View(landInformation) ; 
		}
	}
}