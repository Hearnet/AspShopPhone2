using System;
using GoogleAuthentication.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspShopPhone2.Models;
using Facebook;

namespace AspShopPhone2.Controllers
{
    public class NguoiDungController : Controller
    {
        QLDienThoaiEntities data = new QLDienThoaiEntities();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        
        public ActionResult DangNhap()
        {
            var ClientID = "963268577536-gh1trta1euqpupilmadfba6ltbchdldo.apps.googleusercontent.com";
            var url = "http://localhost:56215/Login/GoogleLoginCallback";
            var response = GoogleAuth.GetAuthUrl(ClientID, url);
            ViewBag.response = response;

          
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Remove("Taikhoan");
            Session.Remove("TaikhoanGG");
            Session.Remove("FacebookUserData");
            return RedirectToAction("Index","PhoneStore");
        }

        [HttpPost]
        public ActionResult DangKy(KHACHHANG model)
        {
            try
            {
                data.KHACHHANGs.Add(model);
                data.SaveChanges();
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

            }
            return View(model);
        }


        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
           
            var taikhoan = form["TaiKhoan"];
            var matkhau = form["MatKhau"];
            

            
                KHACHHANG kh = data.KHACHHANGs.FirstOrDefault(k => k.TaiKhoan == taikhoan && k.MatKhau == matkhau);

                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công <3 !";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "PhoneStore");
                }
                else
                {
                    ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không khớp :(";

                }
            return View();
           
        }
    }
}