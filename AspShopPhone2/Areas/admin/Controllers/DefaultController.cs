using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspShopPhone2.Models;

namespace AspShopPhone2.Areas.admin.Controllers
{
    public class DefaultController : Controller
    {

        QLDienThoaiEntities data = new QLDienThoaiEntities();
        // GET: admin/Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            var taikhoan = form["UserAdmin"];
            var matkhau = form["PassAdmin"];


            ADMIN ad = data.ADMINs.FirstOrDefault(k => k.UserAdmin == taikhoan && k.PassAdmin == matkhau);

            if (ad != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công <3 !";
                Session["admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không khớp :(";

            }
            return View();

        }
    }
}