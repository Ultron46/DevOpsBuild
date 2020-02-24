using DevOps.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevOps.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        string baseUrl = "http://localhost:60969/";
        //public ActionResult Index()
        //{

        //    return View();
        //}

        public async Task<ActionResult> Index()
        {
            List<MainMenu> mainMenus = new List<MainMenu>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/MainMEnu/GetMainMEnus");

                if (Res.IsSuccessStatusCode)
                {
                    var MainMEnuResponse = Res.Content.ReadAsStringAsync().Result;

                    mainMenus = JsonConvert.DeserializeObject<List<MainMenu>>(MainMEnuResponse);
                }

                Session["Menu"] = mainMenus;
            }

            return View();
        }
    }
}
