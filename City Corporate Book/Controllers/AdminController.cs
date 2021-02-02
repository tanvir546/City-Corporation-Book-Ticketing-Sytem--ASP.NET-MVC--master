using City_Corporate_Book.Models;
using City_Corporate_Book.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace City_Corporate_Book.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginChecker(Login logins)
        {
            var checkEmailPassword = context.Logins.FirstOrDefault(x=> x.EmailAddress == logins.EmailAddress && x.Password==logins.Password);

            if(checkEmailPassword == null)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var RegistrationForm = context.Registrations.FirstOrDefault(x=> x.Email==logins.EmailAddress);
                HttpCookie cokie = new HttpCookie("usId");
                cokie.Value = RegistrationForm.Email;
                Response.Cookies.Add(cokie);
                

                if(RegistrationForm.LoginIdentity==0)
                {
                    HttpCookie cokie1 = new HttpCookie("role");
                    cokie1.Value = "member";
                    Response.Cookies.Add(cokie1);
                }
                else if (RegistrationForm.LoginIdentity == 1)
                {
                    HttpCookie cokie1 = new HttpCookie("role");
                    cokie1.Value = "councilor";
                    Response.Cookies.Add(cokie1);
                }
                else if (RegistrationForm.LoginIdentity == 2)
                {
                    HttpCookie cokie1 = new HttpCookie("role");
                    cokie1.Value = "mayor";
                    Response.Cookies.Add(cokie1);
                }
                else if (RegistrationForm.LoginIdentity == 3)
                {
                    HttpCookie cokie1 = new HttpCookie("role");
                    cokie1.Value = "admin";
                    Response.Cookies.Add(cokie1);
                }
                HttpCookie cokie2 = new HttpCookie("location");
                cokie2.Value = RegistrationForm.Location;
                Response.Cookies.Add(cokie2);
                return Json(new { Message = 1, Location = RegistrationForm.Location, Role = RegistrationForm.LoginIdentity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserDetails()
        {
            var Email = Request.Cookies["usId"].Value.ToString();
            var Details = context.Registrations.FirstOrDefault(x => x.Email == Email);
            var Role = "";
            if (Details.LoginIdentity == 0) { Role = "Member"; }
            else if (Details.LoginIdentity == 1) { Role = "Councilor"; }
            else if (Details.LoginIdentity == 2) { Role = "Mayor"; }
            else if (Details.LoginIdentity == 3) { Role = "Admin"; }
            RegistrationVModel UserDetails = new RegistrationVModel
            {
                Email = Details.Email,
                Location = Details.Location,
                LoginIdentity = Role,
                VoterId = Details.VoterId

            };
            return View(UserDetails);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var UserName = Request.Cookies["usId"].Value.ToString();
            var Record = context.Logins.FirstOrDefault(x => x.EmailAddress == UserName && x.Password == model.OldPassword);
            if (Record == null)
            {
                ViewBag.Message = "Old Password Doesn't Matched";
            }
            else
            {
                if (model.NewPassword == model.ConfirmPassword)
                {
                    Record.Password = model.ConfirmPassword;
                    context.Logins.AddOrUpdate(Record);
                    context.SaveChanges();
                    ViewBag.Message = "Successfully Changed";
                }
                else if (model.NewPassword != model.ConfirmPassword)
                {
                    ViewBag.Message = "Check Confirm Password and New Password";
                }
            }
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.All = context.Registrations.Where(x=>x.LoginIdentity==0 || x.LoginIdentity==1 || x.LoginIdentity==2).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            context.Locations.ToList().ForEach(x => { items.Add(x.LocationName.ToString(), x.LocationName.ToString()); });
            ViewBag.User = new SelectList(items, "Key", "Value");

            Dictionary<string, string> items1 = new Dictionary<string, string>();
            items1.Add("Member","Member");
            items1.Add("Councilor", "Councilor");
            items1.Add("Mayor", "Mayor");

            ViewBag.Identity = new SelectList(items1, "Key", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationVModel registrationVModel)
        {
            //ViewBag.Status = "";

            var Register = new Registration();

            if (registrationVModel.LoginIdentity=="Mayor")
            {
                Register.LoginIdentity = 2;
                var Avalibility = context.Registrations.FirstOrDefault(x => x.LoginIdentity == 2);
                if (Avalibility != null)
                {
                    return Json(new { Message = "Already Assigned Mayor" }, JsonRequestBehavior.AllowGet);
                }
                var Avalibility2 = context.Registrations.FirstOrDefault(x => x.Email == registrationVModel.Email || x.VoterId == registrationVModel.VoterId && x.LoginIdentity == 2 || x.VoterId == registrationVModel.VoterId && x.LoginIdentity == 1);
                if(Avalibility2!=null)
                {
                    return Json(new { Message = "Already Assigned Email or VoterID" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (registrationVModel.LoginIdentity == "Member")
            {
                Register.LoginIdentity = 0;
                var Avalibility2 = context.Registrations.FirstOrDefault(x => x.Email == registrationVModel.Email || x.VoterId==registrationVModel.VoterId && x.LoginIdentity==0);
                if (Avalibility2 != null)
                {
                    return Json(new { Message = "Already Assigned Email or VoterID" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Register.LoginIdentity = 1;
                var Avalibility = context.Registrations.FirstOrDefault(x=> x.Location==registrationVModel.Location && x.LoginIdentity==1);
                if(Avalibility!=null)
                {
                    return Json(new { Message = "Already Assigned Councilor at That Location" }, JsonRequestBehavior.AllowGet);
                }
                var Avalibility2 = context.Registrations.FirstOrDefault(x => x.Email == registrationVModel.Email || x.VoterId == registrationVModel.VoterId && x.LoginIdentity == 1 || x.VoterId == registrationVModel.VoterId && x.LoginIdentity == 2);
                if (Avalibility2 != null)
                {
                    return Json(new { Message = "Already Assigned Email or VoterID" }, JsonRequestBehavior.AllowGet);
                }
            }

            Register.Email = registrationVModel.Email;
            Register.Location = registrationVModel.Location;
            Register.VoterId = registrationVModel.VoterId;

            var Logins = new Login()
            {
                EmailAddress = registrationVModel.Email,
                Password = registrationVModel.Password
            };

            context.Registrations.Add(Register);
            context.Logins.Add(Logins);
            context.SaveChanges();

            Dictionary <string, string> items = new Dictionary<string, string>();
            context.Locations.ToList().ForEach(x => { items.Add(x.LocationName.ToString(), x.LocationName.ToString()); });
            ViewBag.User = new SelectList(items, "Key", "Value");

            Dictionary<string, string> items1 = new Dictionary<string, string>();
            items1.Add("Member", "Member");
            items1.Add("Councilor", "Councilor");
            items1.Add("Mayor", "Mayor");

            ViewBag.Identity = new SelectList(items1, "Key", "Value");
            return Json(new { Message = "Successfully Created Account" }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult DeleteUser(string Email)
        {
            try
            {
                var result = context.Registrations.FirstOrDefault(x=> x.Email== Email);
                var result1 = context.Logins.FirstOrDefault(x => x.EmailAddress == Email);
                var comments = context.MemUserCompCommants.Where(x => x.Email == Email);
                var complaints = context.MemUserLocationComplains.Where(x => x.Email == Email);
                context.MemUserLocationComplains.RemoveRange(complaints);
                context.MemUserCompCommants.RemoveRange(comments);
                context.Registrations.Remove(result);
                context.Logins.Remove(result1);
                context.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch(Exception er)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult CheckEmail(string text)
        {
            try
            {
                var result = context.Registrations.FirstOrDefault(x=> x.Email==text);

                if (result == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }

        }
    }
}