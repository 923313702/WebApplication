using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication3.Framework.IRepositorys;
using WebApplication3.Framework.Models;
using WebApplication3.Framework.Repositorys;
using WebApplication3.Models;
using WebApplication3.Unit;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private   ApplicationDb db;
        IUserRepository uRepository;
        IRoleRepository rRepository;
        IUserRoleRepository urRepository;
        public HomeController(ApplicationDb _db)
        {
            this.db = _db;
           uRepository = new UserRepository(db);
           rRepository = new RoleRepository(db);
           urRepository = new UserRoleRepository(db);
       
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (name == "admin" && password == "123456")
            {
                //用户标识
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name,name));
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                var userPrincipal = new ClaimsPrincipal(identity);
                var pro = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false
                };

               await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,pro);
                return RedirectToAction("Index", "Home");
            }
            return View ();
        }
        [Authorize(Roles ="admin,other")]
        public IActionResult Index()
        {

            return View();
        }
        public async Task< IActionResult> LoginOut()
        {
             await  HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult BootStrapTable()
        {
            return View();
        }

        public IActionResult GetTree(string userId)
        {
            List<Role> query;
            if (string.IsNullOrEmpty(userId))
            {
                query = rRepository.Query().ToList();
            }
            else {
                var uId = Convert.ToInt32(userId);
                query = urRepository.QueryWhere(p=>p.UserId==uId).Select(p=>p.Role).ToList();
            }
         
            var tree = Tree.RawCollectionToTree(query).ToList();

            string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            return Content(json);

        }
        public IActionResult AddAuthorize(List<UserRole> urs)
        {
            JsonResult jr;
            var userId = urs[0].UserId;
            var urList = urRepository.QueryWhere(p => p.UserId == userId).ToList();
            var list = urs.Except(urList, new UserRoleListEquality()).ToList();
            if (list.Count <= 0) { return Json(new { success = 0, msg = "添加成功" }); }
            foreach (var i in urs)
            {
                urRepository.Add(i);
            }
            try
            {
                urRepository.SaverChanges();
                jr= Json(new { success = 0, msg = "添加成功" });
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "添加失败" });
            }
            return jr;
        }

        public IActionResult RemoveAuthorize(List<UserRole> urs)
        {
            JsonResult jr;
            foreach (var i in urs)
            {
                urRepository.Delete(i, true);
            }
            try
            {
                urRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" });
            }
            catch (Exception)
            {

                jr = Json(new { success = -1, msg = "删除失败" });
            }
            return jr;
        }
       
        public IActionResult GetData(int limit,int offset)
        {
            var total = uRepository.Query().Count();
            var query = uRepository.Query().Skip (offset).Take(limit).Select(p=>new { p.UserId ,p.Name ,p.Birth ,p.Gender ,p.Password }).ToList();
            return Json(new {total=total,rows=query });
        }

        public IActionResult Add(User u)
        {
            JsonResult jr = null;
            uRepository.Add(u);
            try
            {
                uRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "添加成功" });
            }
            catch (Exception)
            {

                jr = Json(new { success = -1, msg = "添加失败" });
            }
            return jr;
        }
        public IActionResult Edit(User u)
        {
            JsonResult jr = null;
            uRepository.Edit(u);
            try
            {
                uRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" });
            }
            catch (Exception)
            {

                jr = Json(new { success = -1, msg = "修改失败" });
            }
            return jr;
        }


        public IActionResult Remove(List<User> ulist)
        {
            JsonResult jr = null;
            foreach (var i in ulist) {
                uRepository.Delete(i,true);
            }
            try
            {
                uRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" });
            }
            catch (Exception)
            {

                jr = Json(new { success = -1, msg = "删除失败" });
            }
            return jr;
        }


        public IActionResult SetAuthorize()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


      
    }
}
