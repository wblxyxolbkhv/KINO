using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using KINO.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace KINO.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message, int page = 1)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                : message == ManageMessageId.ChangeLoginSuccess ? "Ваш логин изменен"
                : message == ManageMessageId.ChangeEmailSuccess ? "Ваш Email изменен"
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
                : "";

            var context = ApplicationDbContext.Create();
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
            };
            var History = await ApplicationDbContext.GetUserHistoryAsync(userId);
            int pageSize = 10;
            model.PageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = History.Count };
            var HistoryList = History.Skip((page - 1) * pageSize).Take(pageSize);
            //ViewBag.History = await ApplicationDbContext.GetUserHistoryAsync(userId);
            ViewBag.History = HistoryList;
            return View(model);
        }

        //
        // POST: /Manage/Index
        [HttpPost]
        public async Task<ActionResult> Index(int page = 1)
        {
            var context = ApplicationDbContext.Create();
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
            };
            var History = await ApplicationDbContext.GetUserHistoryAsync(userId);
            int pageSize = 10;
            model.PageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = History.Count };
            var HistoryList = History.Skip((page - 1) * pageSize).Take(pageSize);
            //ViewBag.History = await ApplicationDbContext.GetUserHistoryAsync(userId);
            ViewBag.History = HistoryList;
            return PartialView("HistoryPartial",model.PageInfo);
        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Создание и отправка маркера
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Ваш код безопасности: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Отправка SMS через поставщик SMS для проверки номера телефона
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Это сообщение означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Не удалось проверить телефон");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Manage/ChangeLogin
        public ActionResult ChangeLogin()
        {
            return View();
        }

        //
        // POST: /Manage/ChangeLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeLogin(ChangeLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.UserName = model.NewUserName;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeLoginSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Manage/ChangeEmail
        public ActionResult ChangeEmail()
        {
            return View();
        }

        //
        // POST: /Manage/ChangeEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.Email = model.NewEmail;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeEmailSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Это сообщение означает наличие ошибки; повторное отображение формы
            return View(model);
        }


        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Внешнее имя входа удалено."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Запрос перенаправления к внешнему поставщику входа для связывания имени входа текущего пользователя
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/FilmManage
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> FilmManage()
        {
            var context = ApplicationDbContext.Create();
            Film film = null;
            string link = Request.Params["id"];
            if (link != null)
            {
                if (!Int32.TryParse(link, out int l))
                {
                    return View("Error");
                }
                film = await context.Films.FindAsync(l);
                if (film == null)
                    return View("Error");
                if (film.Archived == true)
                    return View("Error");
            }

            SelectList genresList = new SelectList(context.Genres.ToList(), "LINK", "Name");
            ViewBag.Genres = genresList;
            SelectList directorsList = new SelectList(context.Directors.ToList(), "LINK", "Name");
            ViewBag.Directors = directorsList;
            SelectList countriesList = new SelectList(context.Countries.ToList(), "LINK", "Name");
            ViewBag.Countries = countriesList;
            SelectList ageLimitsList = new SelectList(context.AgeLimits.ToList(), "LINK", "Value");
            ViewBag.AgeLimits = ageLimitsList;

            FilmManageViewModel model = new FilmManageViewModel();
            model.Film = film;
            return View(model);
        }

        //
        //POST: Manage/FilmManage
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> FilmManage(FilmManageViewModel model)
        {
            var context = ApplicationDbContext.Create();

            if (model.Film != null)
            {
                if (!model.Film.Poster.EndsWith(".jpg") || !model.Film.Poster.EndsWith(".png"))
                    model.Film.Poster = model.Film.Poster + ".jpg";
                
                if (await context.Films.FindAsync(model.Film.LINK) != null)
                    context.Entry(model.Film).State = EntityState.Modified;
                else
                    context.Entry(model.Film).State = EntityState.Added;
            }
            //if (model.Session != null)
            // context.Entry(model.Session).State = EntityState.Added;
            if (model.UploadedFile != null)
                model.UploadedFile.SaveAs(Server.MapPath("~/Content/Images/Posters/" + model.Film.Poster));
            await context.SaveChangesAsync();
            return RedirectToAction("Affiche","Home");
            
        }

        //
        //GET: Manage/SessionManage
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SessionManage()
        {
            var context = ApplicationDbContext.Create();
            Session session = null;
            string link = Request.Params["id"];
            if (link != null)
            {
                if(!Int32.TryParse(link, out int l))
                {
                    return View("Error");
                }
                session = await context.Sessions.FindAsync(l);
                if (session == null)
                    return View("Error");
                if (session.Archived == true)
                    return View("Error");
            }

            SelectList filmsList = new SelectList(context.Films.ToList(), "LINK", "Name");
            ViewBag.Films = filmsList;
            SelectList hallsList = new SelectList(context.Halls.ToList(), "LINK", "Name");
            ViewBag.Halls = hallsList;

            SessionManageViewModel model = new SessionManageViewModel();
            model.Session = session;

            return View(model);
        }

        //
        //POST: Manage/Session/Manage
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> SessionManage(SessionManageViewModel model)
        {
            var context = ApplicationDbContext.Create();

            string dateString = Request.Params["date_field"];
            DateTime date = DateTime.Parse(dateString);
            model.Session.SessionTime = date;

            if(model.Session != null)
            {
                if (context.Sessions.Find(model.Session.LINK) != null)
                    context.Entry(model.Session).State = EntityState.Modified;
                else
                    context.Entry(model.Session).State = EntityState.Added;
            }
            await context.SaveChangesAsync();

            return RedirectToAction("Affiche", "Home");
        }

        //
        //GET: Manage/ArchiveFilm
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ArchiveFilm(int? id)
        {
            var context = ApplicationDbContext.Create();

            if (id == null)
            {
                return View("Error");
            }

            Film film = await context.Films.FindAsync(id);

            if(film == null)
            {
                return View("Error");
            }

            film.Archived = true;
            context.Entry(film).State = EntityState.Modified;
            foreach(var session in context.Sessions)
            {
                if(session.FilmLINK == film.LINK)
                {
                    session.Archived = true;
                    context.Entry(session).State = EntityState.Modified;
                }
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Affiche", "Home");
        }

        //
        //GET: Manage/ArchiveSession
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ArchiveSession(int? id)
        {
            var context = ApplicationDbContext.Create();

            if (id == null)
            {
                return View("Error");
            }

            Session session = await context.Sessions.FindAsync(id);

            if (session == null)
            {
                return View("Error");
            }

            int filmID = (int)session.FilmLINK;

            session.Archived = true;
            context.Entry(session).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return RedirectToAction("Film", "Home", new { id = filmID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            ChangeLoginSuccess,
            ChangeEmailSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}