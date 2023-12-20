using System;
using System.Threading.Tasks;
using GoogleAuthentication.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;
using System.Web.Security;
using AspShopPhone2.Models;

namespace AspShopPhone2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index() {
            
            return View();
        }

        public async Task<ActionResult> GoogleLoginCallback(string code) {
            try {
                var ClientID = "963268577536-gh1trta1euqpupilmadfba6ltbchdldo.apps.googleusercontent.com";
                var ClientSecret = "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";
                var url = "http://localhost:56215/Login/GoogleLoginCallback";
                var token = await GoogleAuth.GetAuthAccessToken(code, ClientID, ClientSecret, url);
                var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                var googleUser = JsonConvert.DeserializeObject<GoogleProfile>(userProfile);

                if (googleUser != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công <3 !";
                    Session["TaikhoanGG"] = googleUser;
                    return RedirectToAction("Index", "PhoneStore");
                }
                else
                {
                    ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không khớp :(";

                }
            } catch (Exception ex) { }

            return RedirectToAction("DangNhap", "NguoiDung");
        }
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1002779517475809",
                client_secret = "5f6002b6c747e3654924679376ad420b",
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
           
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "1002779517475809",
                    client_secret = "5f6002b6c747e3654924679376ad420b",
                    redirect_uri = RediredtUri.AbsoluteUri,
                    code = code
                });
                var accessToken = result.access_token;
               
                    fb.AccessToken = accessToken;
                    dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

                    var name = me.first_name + " " + me.last_name;

                    // Tạo đối tượng FacebookUserData và gán giá trị
                    var facebookUserData = new FacebookUserData
                    {
                        Link = me.link,
                        Name = name,
                        Email = me.email,
                        Gender = me.gender,
                        Picture = me.picture.data.url,
                        AgeRange = me.age_range
                    };

                    // Lưu đối tượng FacebookUserData vào Session hoặc nơi khác để sử dụng sau này
                    Session["FacebookUserData"] = facebookUserData;
                    return RedirectToAction("Index", "PhoneStore");



        }
            
    }
}