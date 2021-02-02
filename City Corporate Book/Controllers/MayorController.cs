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
    public class MayorController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Mayor
        public ActionResult CouncilorPerformanceChart()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            context.Locations.ToList().ForEach(x => { items.Add(x.LocationName.ToString(), x.LocationName.ToString()); });
            ViewBag.User = new SelectList(items, "Key", "Value");
            return View();
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
            ViewBag.All = context.Registrations.Where(x=> x.LoginIdentity == 1).ToList();
            return View();
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
        public ActionResult CouncilorReview(string Location)
        {
            var Agree = context.MemUserLocationComplains.Where(x=> x.Location == Location && x.Status == 2 && x.Role==0);
            int AgreeNumber = Agree.Count();
            var DisAgree = context.MemUserLocationComplains.Where(x => x.Location == Location && x.Status == 1 && x.Role == 0);
            int DisAgreeNumber = DisAgree.Count();
            var NotSolved = context.MemUserLocationComplains.Where(x=> x.Location == Location && x.Status == 0 && x.Role == 0);
            int NotSolvedNumber = NotSolved.Count();

            return Json(new { Agree = AgreeNumber, DisAgree = DisAgreeNumber, NotSolved = NotSolvedNumber }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyPost()
        {
            return View();
        }

        public ActionResult WatchCouncilorPost(string loca)
        {
            var list = new List<PostCommentVModel>();
            if (loca == "" || loca == "--- Select Location ---")
            {
                //string sql = string.Format("select CMP.StatusNo, CMP.Email,CMP.Location, convert (int,CMP.Status) AS StatusPost, CMP.Date, CMP.Post, ISNULL(CMMENT.ID, '') AS Id,ISNULL(CMMENT.Status, '') AS Status, ISNULL(CMMENT.Comment, '') AS Comment,ISNULL(CMMENT.Email, '') AS CmmentEmail from MemberUserLocationWiseComplains CMP left Join MemberUserLocationWiseComplainCommants CMMENT On CMP.StatusNo = CMMENT.Status");
                string sql = string.Format("select * from MemberUserLocationWiseComplains where Role=1 order by StatusNo desc");
                //var items = context.Database.SqlQuery<MembersComplainList>(sql).ToList();

                //var Posts = context.MemUserLocationComplains.ToList();
                var Posts = context.Database.SqlQuery<MemberUserLocationWiseComplain>(sql).ToList();

                Posts.ForEach(x => {
                    var pm = new PostCommentVModel()
                    {
                        PostId = x.StatusNo,
                        Location = x.Location,
                        Post = x.Post,
                        StatusPost = x.Status,
                        Date = x.Date,
                        Email = x.Email,
                        Agree = x.Agree,
                        Disagree = x.Disagree
                    };
                    var comments = context.MemUserCompCommants.Where(y => y.Status == x.StatusNo).ToList();
                    var UserCookie = Request.Cookies["usId"].Value.ToString();
                    var ActionChecker = context.CommentAgreeDisagreeStatuses.FirstOrDefault(y => y.PostId == pm.PostId && y.UserEmail == UserCookie);
                    if (ActionChecker != null)
                    {
                        pm.Action = ActionChecker.Action;
                    }
                    else
                    {

                        pm.Action = 0;

                    }
                    pm.Comments = comments;
                    list.Add(pm);
                });

                //return Json(new { datas = items }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //var Posts = context.MemUserLocationComplains.Where(x => x.Location == loca).ToList();
                string sql = string.Format("select * from MemberUserLocationWiseComplains where Location = '" + loca + "' and Role = 1 order by StatusNo desc");
                //var items = context.Database.SqlQuery<MembersComplainList>(sql).ToList();

                //var Posts = context.MemUserLocationComplains.ToList();
                var Posts = context.Database.SqlQuery<MemberUserLocationWiseComplain>(sql).ToList();

                Posts.ForEach(x => {
                    var pm = new PostCommentVModel()
                    {
                        PostId = x.StatusNo,
                        Location = x.Location,
                        Post = x.Post,
                        StatusPost = x.Status,
                        Date = x.Date,
                        Email = x.Email,
                        Agree = x.Agree,
                        Disagree = x.Disagree
                    };
                    var UserCookie = Request.Cookies["usId"].Value.ToString();
                    var ActionChecker = context.CommentAgreeDisagreeStatuses.FirstOrDefault(y => y.PostId == pm.PostId && y.UserEmail == UserCookie);
                    if (ActionChecker != null)
                    {
                        pm.Action = ActionChecker.Action;
                    }
                    else
                    {

                        pm.Action = 0;

                    }
                    var comments = context.MemUserCompCommants.Where(y => y.Status == x.StatusNo).ToList();
                    pm.Comments = comments;
                    list.Add(pm);
                });

                //return Json(new { datas = items }, JsonRequestBehavior.AllowGet);
            }
            return View("~/Views/Member/Partial/PostComment.cshtml", list);
        }

        public ActionResult WatchMembersPost(string loca)
        {
            var list = new List<PostCommentVModel>();
            if (loca == "" || loca == "--- Select Location ---")
            {
                //string sql = string.Format("select CMP.StatusNo, CMP.Email,CMP.Location, convert (int,CMP.Status) AS StatusPost, CMP.Date, CMP.Post, ISNULL(CMMENT.ID, '') AS Id,ISNULL(CMMENT.Status, '') AS Status, ISNULL(CMMENT.Comment, '') AS Comment,ISNULL(CMMENT.Email, '') AS CmmentEmail from MemberUserLocationWiseComplains CMP left Join MemberUserLocationWiseComplainCommants CMMENT On CMP.StatusNo = CMMENT.Status");
                string sql = string.Format("select * from MemberUserLocationWiseComplains where Role=0 order by StatusNo desc");
                //var items = context.Database.SqlQuery<MembersComplainList>(sql).ToList();

                //var Posts = context.MemUserLocationComplains.ToList();
                var Posts = context.Database.SqlQuery<MemberUserLocationWiseComplain>(sql).ToList();

                Posts.ForEach(x => {
                    var pm = new PostCommentVModel()
                    {
                        PostId = x.StatusNo,
                        Location = x.Location,
                        Post = x.Post,
                        StatusPost = x.Status,
                        Date = x.Date,
                        Email = x.Email,
                        Agree = x.Agree,
                        Disagree = x.Disagree
                    };
                    var comments = context.MemUserCompCommants.Where(y => y.Status == x.StatusNo).ToList();
                    var UserCookie = Request.Cookies["usId"].Value.ToString();
                    var ActionChecker = context.CommentAgreeDisagreeStatuses.FirstOrDefault(y => y.PostId == pm.PostId && y.UserEmail == UserCookie);
                    if (ActionChecker != null)
                    {
                        pm.Action = ActionChecker.Action;
                    }
                    else
                    {

                        pm.Action = 0;

                    }
                    pm.Comments = comments;
                    list.Add(pm);
                });

                //return Json(new { datas = items }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //var Posts = context.MemUserLocationComplains.Where(x => x.Location == loca).ToList();
                string sql = string.Format("select * from MemberUserLocationWiseComplains where Location = '" + loca + "' and Role = 0 order by StatusNo desc");
                //var items = context.Database.SqlQuery<MembersComplainList>(sql).ToList();

                //var Posts = context.MemUserLocationComplains.ToList();
                var Posts = context.Database.SqlQuery<MemberUserLocationWiseComplain>(sql).ToList();

                Posts.ForEach(x => {
                    var pm = new PostCommentVModel()
                    {
                        PostId = x.StatusNo,
                        Location = x.Location,
                        Post = x.Post,
                        StatusPost = x.Status,
                        Date = x.Date,
                        Email = x.Email,
                        Agree = x.Agree,
                        Disagree = x.Disagree
                    };
                    var UserCookie = Request.Cookies["usId"].Value.ToString();
                    var ActionChecker = context.CommentAgreeDisagreeStatuses.FirstOrDefault(y => y.PostId == pm.PostId && y.UserEmail == UserCookie);
                    if (ActionChecker != null)
                    {
                        pm.Action = ActionChecker.Action;
                    }
                    else
                    {

                        pm.Action = 0;

                    }
                    var comments = context.MemUserCompCommants.Where(y => y.Status == x.StatusNo).ToList();
                    pm.Comments = comments;
                    list.Add(pm);
                });

                //return Json(new { datas = items }, JsonRequestBehavior.AllowGet);
            }
            return View("~/Views/Member/Partial/PostComment.cshtml", list);
        }

        [HttpGet]
        public ActionResult MemberPost()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            context.Locations.ToList().ForEach(x => { items.Add(x.LocationName.ToString(), x.LocationName.ToString()); });
            ViewBag.User = new SelectList(items, "Key", "Value");

            return View();
        }

        [HttpPost]
        public JsonResult SubmitMemberPost(string Location, string Post)
        {
            var status = context.MemUserLocationComplains.ToList();
            int counter = status.Count();
            counter++;
            DateTime now = DateTime.Now;
            // Format	ResultDateTime.Now.ToString("MM/dd/yyyy HH:mm")
            string emailadd = Request.Cookies["usId"].Value;
            string statusRole = Request.Cookies["role"].Value;
            int role = 0;
            if (statusRole == "member")
            {
                role = 0;
            }
            else if (statusRole == "councilor")
            {
                role = 1;
            }
            else
            {
                role = 2;
            }
            var result = new MemberUserLocationWiseComplain
            {
                Email = Request.Cookies["usId"].Value.ToString(),
                Location = Location,
                Status = 0,
                Post = Post,
                Date = now,
                Role = role,
                Agree = 0,
                Disagree = 0,
            };
            context.MemUserLocationComplains.Add(result);
            context.SaveChanges();
            return Json(new { Message = 1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult WritePost()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            context.Locations.ToList().ForEach(x => { items.Add(x.LocationName.ToString(), x.LocationName.ToString()); });
            ViewBag.User = new SelectList(items, "Key", "Value");
            return View();
        }

        [HttpGet]
        public ActionResult CouncilorPost()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            context.Locations.ToList().ForEach(x => { items.Add(x.LocationName.ToString(), x.LocationName.ToString()); });
            ViewBag.User = new SelectList(items, "Key", "Value");

            return View();
        }

        

        [HttpGet]
        public ActionResult WatchMyPost(string Id)
        {
            var list = new List<PostCommentVModel>();
            //var Posts = context.MemUserLocationComplains.Where(x => x.Location == loca).ToList();
            string sql = string.Format("select * from MemberUserLocationWiseComplains where Email = '" + Id + "' order by StatusNo desc");
            //var items = context.Database.SqlQuery<MembersComplainList>(sql).ToList();

            //var Posts = context.MemUserLocationComplains.ToList();
            var Posts = context.Database.SqlQuery<MemberUserLocationWiseComplain>(sql).ToList();

            Posts.ForEach(x => {
                var pm = new PostCommentVModel()
                {
                    PostId = x.StatusNo,
                    Location = x.Location,
                    Post = x.Post,
                    StatusPost = x.Status,
                    Date = x.Date,
                    Email = x.Email,
                    Agree = x.Agree,
                    Disagree = x.Disagree
                };
                var UserCookie = Request.Cookies["usId"].Value.ToString();
                var ActionChecker = context.CommentAgreeDisagreeStatuses.FirstOrDefault(y => y.PostId == pm.PostId && y.UserEmail == UserCookie);
                if (ActionChecker != null)
                {
                    pm.Action = ActionChecker.Action;
                }
                else
                {

                    pm.Action = 0;

                }
                var comments = context.MemUserCompCommants.Where(y => y.Status == x.StatusNo).ToList();
                pm.Comments = comments;
                list.Add(pm);
            });

            return View("~/Views/Mayor/Partial/PostComment.cshtml", list);
        }
    }
}