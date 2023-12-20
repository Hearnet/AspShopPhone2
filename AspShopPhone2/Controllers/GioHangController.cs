using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspShopPhone2.common;
using AspShopPhone2.Models;
using PayPal.Api;

namespace AspShopPhone2.Controllers
{
    public class GioHangController : Controller
    {
        QLDienThoaiEntities data = new QLDienThoaiEntities();

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            // nếu chưa có giỏ hàng thì khỏi tạo
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }

        //Thêm giỏ hàng
        public ActionResult ThemGioHang(int dMaDT, string strURL)
        {

            // lấy ra session giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sanpham = lstGioHang.Find(n => n.dMaDT == dMaDT);
            if (sanpham == null)
            {
                sanpham = new GioHang(dMaDT);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.dSoLuong++;
                return Redirect(strURL);
            }
        }

        // Tính tổng số lượng sản phẩm
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.dSoLuong);
            }
            return iTongSoLuong;
        }

        //Tính tổng tiền
        private double TongTien()
        {
            double TongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                ViewBag.DSGioHang = lstGiohang;
                TongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return TongTien;
        }

        private double TongTienUSD()
        {
            double TongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                ViewBag.DSGioHang = lstGiohang;
                TongTien = lstGiohang.Sum(n => Math.Round( n.dThanhTien / 23500,2));
            }
            return TongTien;
        }

        // Xử lí view giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "PhoneStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }

        // Giỏ hàng Partial
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        // Xoá sản phẩm khỏi giỏ hàng
        public ActionResult XoaGioHang(int? dMaSP)
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (dMaSP.HasValue)
            {
                GioHang sanpham = lstGioHang.SingleOrDefault(n => n.dMaDT == dMaSP);
                if (sanpham != null)
                {
                    lstGioHang.RemoveAll(n => n.dMaDT == dMaSP);
                    return RedirectToAction("GioHang");
                }
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "PhoneStore");
                }
            }
            return RedirectToAction("GioHang");
        }

        // Cập nhập số lượng sản phẩm tại giỏ hàng
        public ActionResult CapNhatGioHang(int dMaDT, FormCollection form)
        {
            List<GioHang> lstGiohang = LayGioHang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.dMaDT == dMaDT);
            if (sanpham != null)
            {
                sanpham.dSoLuong = int.Parse(form["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        // Xoá tất cả sản phẩm khỏi giỏ hàng
        public ActionResult XoaTatCa()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "PhoneStore");
        }

        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public void guiGmail(KHACHHANG kh, List<GioHang> gh, string PhuongThucTT) {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"));
            string strSanPham = "";
            foreach (var item in gh)
            {
                strSanPham += "<tr><td class=\"front-break\">" +
                      item.dTenDT +
                     "</td>" +
                     "<td class=\"inline-td\">" +
                      item.dSoLuong +
                     "</td>" +
                     "<td class=\"inline-td\">" +
                     "<span>" + item.dThanhTien.ToString("N0") + "&nbsp;<span>₫</span></span>" +
                     "</td></tr>";
            }

            DONDATHANG donhang = data.DONDATHANGs
    .Where(dh => dh.MaKH == kh.MaKH)
    .OrderByDescending(dh => dh.MaDonHang)
    .FirstOrDefault();
            content = content.Replace("{{SanPham}}", strSanPham);
            content = content.Replace("{{TenKhachHang}}", kh.HoTen);
            content = content.Replace("{{MaDonHang}}", donhang.MaDonHang.ToString());
            content = content.Replace("{{NgayDatHang}}", donhang.NgayDat.ToString());
            content = content.Replace("{{TongTien}}", TongTien().ToString("N0"));
            content = content.Replace("{{DiaChi}}", kh.DiaChiKH);
            content = content.Replace("{{SoDienThoai}}", kh.DienThoaiKH);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{PhuongThucThanhToan}}", PhuongThucTT);



            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            // Để Gmail cho phép SmtpClient kết nối đến server SMTP của nó với xác thực 
            //là tài khoản gmail của bạn, bạn cần thiết lập tài khoản email của bạn như sau:
            //Vào địa chỉ https://myaccount.google.com/security  Ở menu trái chọn mục Bảo mật, sau đó tại mục Quyền truy cập 
            //của ứng dụng kém an toàn phải ở chế độ bật
            //  Đồng thời tài khoản Gmail cũng cần bật IMAP
            //Truy cập địa chỉ https://mail.google.com/mail/#settings/fwdandpop

            new MailHelper().SendMail(kh.Email, "Đơn hàng mới từ ShopPhone", content);
        }
        // Xử lí đặt hàng
        [HttpPost]
        public ActionResult DatHang(FormCollection form)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            

            List<GioHang> gh = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", form["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DONDATHANGs.Add(ddh);
            data.SaveChanges();

            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaDT = item.dMaDT;
                ctdh.Soluong = item.dSoLuong;
                ctdh.DonGia = (decimal)item.dDongGia;
                data.CHITIETDONTHANGs.Add(ctdh);
            }
            data.SaveChanges();

            // gửi mail
            guiGmail(kh, gh, "Thanh toán khi nhận hàng");
            //Xóa hết giỏ hàng

            Session["GioHang"] = null;
           
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        
        // view xác nhận đơn hàng
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public ActionResult FailureView()
        {
            return View();
        }

        // thanh toán PAYPAL

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return View("FailureView");
            }
            // xử lí csdl sau khi thanh toán
            {
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
                List<GioHang> gh = LayGioHang();
                ddh.MaKH = kh.MaKH;
                ddh.NgayDat = DateTime.Now;
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                ddh.NgayGiao = DateTime.Parse(ngaygiao);
                ddh.TinhTrangGiaoHang = false;
                ddh.DaThanhToan = true;
                data.DONDATHANGs.Add(ddh);
                data.SaveChanges();

                foreach (var item in gh)
                {
                    CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaDT = item.dMaDT;
                    ctdh.Soluong = item.dSoLuong;
                    ctdh.DonGia = (decimal)item.dDongGia;
                    data.CHITIETDONTHANGs.Add(ctdh);
                }
                data.SaveChanges();
                guiGmail(kh, gh, "Thanh toán qua Paypal");
                Session["GioHang"] = null;
            }

            //on successful payment, show success page to user.  
            return View("XacNhanDonHang");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //Lấy danh sách đơn hàng từ giỏ hàng vừa đặt
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            if (lstGiohang != null)
            {
                foreach (var item in lstGiohang)
                {
                    //Adding Item Details like name, currency, price etc  
                    itemList.items.Add(new Item()
                    {
                        name = item.dTenDT,
                        currency = "USD",
                        price = Math.Round(item.dDongGia / 23500,2).ToString(),
                        quantity = item.dSoLuong.ToString(),
                        sku = item.dMaDT.ToString()
                    });
                }
            }
            
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = TongTienUSD().ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = TongTienUSD().ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }



        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
    }
}