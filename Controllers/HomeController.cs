using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using The_Wall.Models;

namespace The_Wall.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewModel viewM = new ViewModel();
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(User NewUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.User.Any(u => u.Email == NewUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already Exist!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                dbContext.User.Add(NewUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("id", NewUser.UserId);
                int? logUser = HttpContext.Session.GetInt32("id");
                ViewBag.logUser = logUser;

                return RedirectToAction("Privacy");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("LoginProcess")]
        public IActionResult LoginProcess(ViewModel userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.User.FirstOrDefault(u => u.Email == userSubmission.LoginUser.Email);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                // verify provided password against hash stored in db
                // var result = hasher.VerifyHashedPassword (userSubmission, userInDb.Password, userSubmission.Password);
                // System.Console.WriteLine ("*********" + result);
                // result can be compared to 0 for failure
                if (hasher.VerifyHashedPassword(userSubmission.LoginUser, userInDb.Password, userSubmission.LoginUser.Password) == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("Email", "Invalid email/password");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("id", userInDb.UserId);
                    int? logUser = HttpContext.Session.GetInt32("id");

                    ViewBag.logUser = logUser;
                    return RedirectToAction("Privacy");
                }

            }
            else { return View("Index"); }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            int? logUser = HttpContext.Session.GetInt32("id");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("TheWall")]
        public IActionResult Privacy()
        {
            int? logUser = HttpContext.Session.GetInt32("id");
            if (logUser == null)
            {
                return View("Index");
            }
            User live_user = dbContext.User.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("id"));
            ViewBag.live_user = live_user;
            List<User> AllUser = dbContext.User.ToList();
            List<Messages> AllMsg = dbContext.Messages.Include(u => u.MsgCreator).OrderByDescending(d => d.CreatedAt).ToList();
            ViewBag.AllMsgs = AllMsg;
            Messages NewMsg = dbContext.Messages.FirstOrDefault(u => u.MsgCreator.UserId == live_user.UserId);
            Comments newCom = dbContext.Comments.FirstOrDefault(u => u.CommentCreator.UserId == live_user.UserId);

            List<Comments> AllCommnet = dbContext.Comments.Include(m => m.CommentToMesg).ToList();

            ViewModel viewM = new ViewModel();
            viewM.NewComment = newCom;
            viewM.NewMsg = NewMsg;
            viewM.AllUsers = AllUser;
            viewM.AllMsg = AllMsg;
            viewM.AllComments = AllCommnet;
            viewM.NewUser = live_user;
            return View(viewM);
        }


        [HttpPost("PostMsg")]
        public IActionResult PostMsg(ViewModel Pmsg)
        {
            int? logUser = HttpContext.Session.GetInt32("id");
            ViewBag.logUser = logUser;
            if (logUser == null)
            {
                return View("Index");
            }
            User live_user = dbContext.User.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("id"));
            ViewBag.live_user = live_user;
            List<User> AllUser = dbContext.User.ToList();
            List<Messages> AllMsg = dbContext.Messages.Include(u => u.MsgCreator).OrderByDescending(d => d.CreatedAt).ToList();
            ViewBag.AllMsgs = AllMsg;
            List<Comments> AllCommnet = dbContext.Comments.ToList();
            Comments newCom = dbContext.Comments.FirstOrDefault(u => u.CommentCreator.UserId == live_user.UserId);
            // Messages NewMsg = dbContext.Messages.FirstOrDefault(m => m.MsgCreator.UserId == live_user.UserId);
            ViewModel viewM = new ViewModel();
            viewM.AllUsers = AllUser;
            viewM.AllMsg = AllMsg;
            viewM.AllComments = AllCommnet;
            viewM.NewUser = live_user;
            Messages NewMsg = Pmsg.NewMsg;
            if (ModelState.IsValid)
            {
                NewMsg.UserId = live_user.UserId;
                dbContext.Messages.Add(NewMsg);
                dbContext.SaveChanges();
                return RedirectToAction("Privacy");
            }
            return View("Privacy", viewM);
        }

        [HttpPost("PostComment")]
        public IActionResult PostComment(ViewModel newcom)
        {
            int? logUser = HttpContext.Session.GetInt32("id");
            ViewBag.logUser = logUser;
            if (logUser == null)
            {
                return View("Index");
            }
            User live_user = dbContext.User.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("id"));
            ViewBag.live_user = live_user;
            List<User> AllUser = dbContext.User.ToList();
            List<Messages> AllMsg = dbContext.Messages.Include(u => u.MsgCreator).OrderByDescending(d => d.CreatedAt).ToList();
            ViewBag.AllMsgs = AllMsg;
            List<Comments> AllCommnet = dbContext.Comments.Include(m => m.CommentToMesg).ToList();
            Messages NewMsg = dbContext.Messages.FirstOrDefault(m => m.MsgCreator.UserId == live_user.UserId);
            Comments newCom = dbContext.Comments.FirstOrDefault(u => u.CommentCreator.UserId == live_user.UserId);

            ViewModel viewM = new ViewModel();
            viewM.AllUsers = AllUser;
            viewM.AllMsg = AllMsg;
            viewM.NewMsg = NewMsg;
            viewM.AllComments = AllCommnet;
            viewM.NewUser = live_user;
            Comments NewComment = newcom.NewComment;
            if (ModelState.IsValid)
            {
                NewComment.UserId = live_user.UserId;
                dbContext.Comments.Add(NewComment);
                dbContext.SaveChanges();
                return RedirectToAction("Privacy");
            }
            return View("Privacy", viewM);
        }


        [HttpGet("Delete/{Mid}")]
        public IActionResult Delete(int MId)
        {
            int? logUser = HttpContext.Session.GetInt32("id");
            ViewBag.logUser = logUser;
            if (logUser == null)
            {
                return View("Index");
            }
            User live_user = dbContext.User.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("id"));
            ViewBag.live_user = live_user;
            List<User> AllUser = dbContext.User.ToList();
            List<Messages> AllMsg = dbContext.Messages.Include(u => u.MsgCreator).OrderByDescending(d => d.CreatedAt).ToList();
            ViewBag.AllMsgs = AllMsg;
            List<Comments> AllCommnet = dbContext.Comments.Include(m => m.CommentToMesg).ToList();
            Messages NewMsg = dbContext.Messages.FirstOrDefault(m => m.MsgCreator.UserId == live_user.UserId);
            Comments newCom = dbContext.Comments.FirstOrDefault(u => u.CommentCreator.UserId == live_user.UserId);

            ViewModel viewM = new ViewModel();
            viewM.AllUsers = AllUser;
            viewM.AllMsg = AllMsg;
            viewM.NewMsg = NewMsg;
            viewM.AllComments = AllCommnet;
            viewM.NewUser = live_user;
            Messages deleteM = dbContext.Messages.FirstOrDefault(w => w.MessageId == MId);

            if (deleteM.UserId == logUser)
            {
                var check = deleteM.CreatedAt - DateTime.Now;
                System.TimeSpan diff = DateTime.Now - deleteM.CreatedAt;
                double doubleMinutes = diff.TotalMinutes;
                int intMinutes = (int)doubleMinutes;
                System.Console.WriteLine("***************** " + intMinutes + " - " + diff + " - " + check + " " + deleteM.CreatedAt + " **********");

                dbContext.Remove(deleteM);
                if (intMinutes < 30)
                {
                    dbContext.SaveChanges();

                }
            }
            else
            {
                return RedirectToAction("Privacy", viewM);
            }
            return RedirectToAction("Privacy", viewM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
