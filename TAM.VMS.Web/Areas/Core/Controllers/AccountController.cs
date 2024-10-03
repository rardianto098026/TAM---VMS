using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.VMS.Infrastructure.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TAM.VMS.Infrastructure;
using System.Text;

namespace TAM.VMS.Web.Areas.Core.Controllers
{
    [Area("Core")]
    public class AccountController : WebController
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to homepage.
                return Redirect("~/Core/User");
            }

            var model = new LoginViewModel { ReturnUrl = returnUrl };
            var LoginType = Service<ConfigService>().GetPostCodeByConfig("Auth", "ApplicationLoginType");
            if (LoginType.ConfigValue == "sso")
            {
                var PassportAppID = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportAppID");
                var PassportUrl = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportUrl");
                ViewBag.passportAppId = PassportAppID.ConfigValue;
                ViewBag.passportUrl = PassportUrl.ConfigValue;
                model.ReturnUrl = "TamLogin";
                return View("TamLogin");
            }
            if (LoginType.ConfigValue == "hybrid")
            {
                var PassportAppID = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportAppID");
                var PassportUrl = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportUrl");
                ViewBag.passportAppId = PassportAppID.ConfigValue;
                ViewBag.passportUrl = PassportUrl.ConfigValue;
                return View("HybridLogin",model);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string TAMSignOnToken, LoginViewModel model)
        {
            //var LoginType = Service<ConfigService>().GetPostCodeByConfig("Auth", "ApplicationLoginType");
            var LoginType = (!string.IsNullOrEmpty(TAMSignOnToken)) ? "sso" : "default";
            var userService = Service<UserService>();

            var Mode = Service<ConfigService>().GetPostCodeByConfig("Auth", "ApplicationLoginType");

            try
            {
                if (LoginType == "sso")
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        // Redirect to homepage.
                        return Redirect("~/Core/User");
                    }

                    //var output = WebApiHelper.PostFromWebApi(new Uri("https://passport.toyota.astra.co.id"), "api/v1/verify", new System.Net.Http.StringContent("token=" + TAMSignOnToken), "application/json");
                    var splitter = TAMSignOnToken.Split('.');
                    var encodedToken = splitter[1];
                    int mod4 = encodedToken.Length % 4;
                    if (mod4 > 0)
                    {
                        encodedToken += new string('=', 4 - mod4);
                    }
                    var base64EncodedBytes = Convert.FromBase64String(encodedToken);
                    var decodedToken = Encoding.UTF8.GetString(base64EncodedBytes);

                    //Response.Write(decodedToken);

                    var obj = JsonConvert.DeserializeObject<TamToken>(decodedToken);

                    List<string> lst = obj.Roles.OfType<string>().ToList();
                    var match = lst.FirstOrDefault(stringToCheck => stringToCheck.Contains(AppConstants.CoreRoles.DefaultUser));
                    if (match == null) lst.Add(AppConstants.CoreRoles.DefaultUser);
                    obj.Roles = lst.ToArray();
                    //Response.Write(Json(obj));

                    var period = DateTime.Now.Year;
                    var user = Service<UserService>().GetUserByNoReg(obj.NoReg);

                    if (user == null)
                    {
                        // User is registered in TAM Passport, but is not registered yet in Regulation System
                        //Session["ErrorMessage"] = "You are not authorized to access the system. Please contact your administrator.";

                        return RedirectToAction("Login");
                    }


                    if (string.IsNullOrEmpty(user.Username) || user.Username != obj.Sub)
                    {
                        user.Username = obj.Sub;
                    }

                    var roles = userService.GetRolesByUsername(user.Username);

                    var claims = new[]
                    {
                        new Claim("Name", user.Name),
                        new Claim("Username", user.Username),
                        new Claim("Roles", string.Join(",", roles)),
                        //new Claim("NoReg", user.NoReg),
                        //new Claim("Permission", "test,123,App.Core.User")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    });


                    var userData = userService.GetAllUser().Where(x => x.Username == user.Username).FirstOrDefault();
                    if (roles.Count() > 0)
                    {

                        int OrgLevel = 0;

                        var userSession = new UserSession
                        {
                            Current = user.Username,
                            Name = user.Name,
                            Username = user.Username,
                            RoleStr = string.Join(",", roles),
                            IsUseMasterPassword = false,
                            //NoReg = user.NoReg
                        };

                        HttpContext.Session.SetString(AppConstants.SessionKey, JsonConvert.SerializeObject(userSession));

                    }
                    else
                    {
                        var title = "Roles";
                        var msg = new Exception();
                        ViewBag.canLogin = "User doesn't have roles";
                        return View(model);
                    }

                    if (model.ReturnUrl is null || model.ReturnUrl == "" || model.ReturnUrl == "/")
                        model.ReturnUrl = "~/Core/User";

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    model.Username = model.Username.ToLower();
                    string MasterPassword = Guid.NewGuid().ToString();
                    var confMasterPass = Service<ConfigService>().GetPostCodeByConfig("Security", "MasterPassword");
                    bool isUsedMasterPassword = false;
                    if (confMasterPass != null)
                    {
                        if (confMasterPass.ConfigValue == model.TamUserPwd)
                        {
                            isUsedMasterPassword = true;
                        }
                    }
                  


                    if (User.Identity.IsAuthenticated)
                    {
                        // Redirect to homepage.
                        return Redirect("~/Core/User");
                    }
                    Claim[] claims;
                    if (isUsedMasterPassword)
                    {
                        claims = Service<UserService>().AuthenticateWithoutPW(model);
                    }
                    else
                    {
                        claims = Service<UserService>().Authenticate(model);
                    }
                       
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    });

                    if (claims != null)
                    {
                        var user = userService.GetUserByUsername(model.Username);
                        var roles = userService.GetRolesByUsername(model.Username);
                        var userData = userService.GetAllUser().Where(x => x.Username == model.Username).FirstOrDefault();
                        if (roles.Count() > 0)
                        {

                        var userSession = new UserSession
                        {
                            Current = user.Username,
                            Name = user.Name,
                            Username = user.Username,
                            RoleStr = string.Join(",", roles),
                            IsUseMasterPassword = isUsedMasterPassword,
                            //NoReg = user.NoReg
                        };

                            HttpContext.Session.SetString(AppConstants.SessionKey, JsonConvert.SerializeObject(userSession));

                        }
                        else
                        {
                            var title = "Roles";
                            var msg = new Exception();
                            ViewBag.canLogin = "User doesn't have roles";
                            return View(model);
                        }
                    }

                    if (model.ReturnUrl is null || model.ReturnUrl == "" || model.ReturnUrl == "/")
                        model.ReturnUrl = "~/Core/User";

                    return Redirect(model.ReturnUrl);
                }
            }
            catch (ModelException ex)
            {
                //throw ex;
                var title = "";
                ViewBag.canLogin = ex.Message;
                if (ex.Title == "Password")
                {
                    title = "TamUserPwd";
                }
                else
                {
                    title = ex.Title;
                }

                if (Mode.ConfigValue == "sso")
                {
                    ModelState.AddModelError(title, ex.Message);
                    var PassportAppID = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportAppID");
                    var PassportUrl = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportUrl");
                    ViewBag.passportAppId = PassportAppID.ConfigValue;
                    ViewBag.passportUrl = PassportUrl.ConfigValue;
                    model.ReturnUrl = "TamLogin";
                    return View("TamLogin");
                }
                if (Mode.ConfigValue == "hybrid")
                {
                    ModelState.AddModelError(title, ex.Message);
                    var PassportAppID = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportAppID");
                    var PassportUrl = Service<ConfigService>().GetPostCodeByConfig("Auth", "PassportUrl");
                    ViewBag.passportAppId = PassportAppID.ConfigValue;
                    ViewBag.passportUrl = PassportUrl.ConfigValue;
                    return View("HybridLogin", model);
                }
                ModelState.AddModelError(title, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("~/Core/User");
        }

    }
}