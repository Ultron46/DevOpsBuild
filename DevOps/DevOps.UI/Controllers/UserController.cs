using DevOps.UI.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;


namespace DevOps.UI.Controllers
{
    [EnableCorsAttribute("*","*","*")]
    public class UserController : Controller
    {
        string baseUrl = "http://localhost:60969/";
        // GET: User
        [HttpGet]
        public async Task<ActionResult> Index(string sortName,int? page)
        {
            List<User> users = new List<User>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl);

            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage Res = await client.GetAsync("api/Users/GetAllUsers");

            if (Res.IsSuccessStatusCode)
            {
                var userResponse = Res.Content.ReadAsStringAsync().Result;

                users = JsonConvert.DeserializeObject<List<User>>(userResponse);
            }


            var user = from u in users select u;

            ViewBag.CurrentSort = sortName;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortName) ? "name_desc" : "";
            //var user = new List<User>();
            switch (sortName)
            {
                case "name_desc":
                    user = user.OrderByDescending(s => s.Name);
                    break;
                default:
                    user = user.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(user.ToPagedList(pageNumber, pageSize));


            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            //var xya = users.ToPagedList(pageNumber, pageSize);
            //var user = from u in xya select u;

            //ViewBag.CurrentSort = sortName;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortName) ? "name_desc" : "";
            ////var user = new List<User>();
            //switch (sortName)
            //{
            //    case "name_desc":
            //        user = user.OrderByDescending(s => s.Name);
            //        break;
            //    default:
            //        user = user.OrderBy(s => s.Name);
            //        break;
            //}

            //return View(user);

            //return View(users);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registration(User user)
        {
            List<User> users = new List<User>();
            var client = new HttpClient();
            
                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Users/GetAllUsers");

                if (Res.IsSuccessStatusCode)
                {
                    var MainMEnuResponse = Res.Content.ReadAsStringAsync().Result;

                    users = JsonConvert.DeserializeObject<List<User>>(MainMEnuResponse);
                }
            bool check = true;
            if(users.Any(x => x.Email == user.Email))
            {
                ViewBag.EmailError = "email id already exists";
                check = false;
            }
            if (users.Any(x => x.Phone == user.Phone))
            {
                ViewBag.PhoneError = "Mobile Number is already exists";
                check = false;
            }
            if(check)
            {
                client.DefaultRequestHeaders.Clear();
                //Res = await client.PostAsync("api/Users/InsertUser");

                var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var addressUri = new Uri("api/Users/InsertUser", UriKind.Relative);
                Res = client.PostAsync(addressUri, stringContent).Result;

                if (Res.IsSuccessStatusCode)
                {
                    
                    //var MainMEnuResponse = Res.Content.ReadAsStringAsync().Result;

                    //users = JsonConvert.DeserializeObject<List<User>>(MainMEnuResponse);
                    return View("Login");
                }
            }

            return View("Registration", user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(User user)
        {
            List<User> users = new List<User>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl);

            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage Res = await client.GetAsync("api/Users/GetAllUsers");

            if (Res.IsSuccessStatusCode)
            {
                var MainMEnuResponse = Res.Content.ReadAsStringAsync().Result;

                users = JsonConvert.DeserializeObject<List<User>>(MainMEnuResponse);
            }
            //bool check = true;
            if ((users.Any(x => x.Email == user.Email)) && (users.Any(x => x.Password == user.Password)))
            {
                List<MainMenu> mainMenus = new List<MainMenu>();
                List<SubMenu> subs = new List<SubMenu>();

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Res = await client.GetAsync("api/MainMEnu/GetMainMEnus");

                if (Res.IsSuccessStatusCode)
                {
                    var MainMEnuResponse = Res.Content.ReadAsStringAsync().Result;

                    mainMenus = JsonConvert.DeserializeObject<List<MainMenu>>(MainMEnuResponse);
                }

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Res = await client.GetAsync("api/SubMenu/GetSubMenus");

                if (Res.IsSuccessStatusCode)
                {
                    var MainMEnuResponse = Res.Content.ReadAsStringAsync().Result;

                    subs = JsonConvert.DeserializeObject<List<SubMenu>>(MainMEnuResponse);
                }

                Session["Menu"] = mainMenus;
                Session["Subs"] = subs;
                return View("../Home/Index");
                //ViewBag.EmailError = "Email id is Not Valid";
                //check = false;
            }
            else
            {
                ViewBag.LoginError = "Not valid";
                return View("Login", user);
            }
           

        }


        
    }
}