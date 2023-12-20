using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspShopPhone2.Models;
namespace AspShopPhone2.Areas.admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: admin/SanPham
        QLDienThoaiEntities data = new QLDienThoaiEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachSanPham()
        {
            ViewBag.MaTH = new SelectList(data.THUONGHIEUx.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            return View();
        }

        [HttpGet]
        public JsonResult DsDienThoai(string name, int page, int pageSize = 3)
        {
            try
            {
                List<ChiTietSanPham> DSdienthoai = (from d in data.DIENTHOAIs.Where(x => x.DaXoa != 1)
                                   select new ChiTietSanPham
                                   {
                                       id = d.MaDT,
                                       tenDienThoai = d.TenDienThoai,
                                       idTH = (int)d.MaTH,
                                       giaBan = (decimal)d.GiaBan,
                                       moTa = d.MoTa,
                                       anhBia = d.AnhBia,
                                       NgayCapNhat = (DateTime)d.NgayCapNhat
                                   }).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                     DSdienthoai = DSdienthoai.Where(item => item.tenDienThoai.ToUpper().Contains(name.ToUpper())).ToList();
                };

                int totalRow = DSdienthoai.Count();
                DSdienthoai = DSdienthoai.OrderByDescending(x => x.NgayCapNhat) // Sắp xếp
                       .Skip((page - 1) * pageSize) // Bỏ qua mục
                       .Take(pageSize) // Lấy số lượng mục
                       .ToList(); // Chuyển danh sách kết quả sang danh sách đã phân trang
                return Json(new
                {
                    code = 200,
                    total = totalRow,
                    data = DSdienthoai,
                    msg = "Lấy sản phẩm thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy sản phẩm thất bại: "+ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        

       // Thêm mới
        [HttpPost]
        public ActionResult ThemSanPham(DIENTHOAI model)
        {
            if (model.FileAnhBia != null && model.FileAnhBia.ContentLength>0)
            {

                var fileName = Path.GetFileNameWithoutExtension(model.FileAnhBia.FileName);
                string extension = Path.GetExtension(model.FileAnhBia.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                string rootPath = Server.MapPath("~"); // Lấy đường dẫn thư mục gốc của ứng dụng
                string imagePath = Path.Combine(rootPath, "assets/img/sanPham", fileName);
                model.FileAnhBia.SaveAs(imagePath);
                model.AnhBia = fileName;
            }
            try
            {
                model.DaXoa = 0;
                data.DIENTHOAIs.Add(model);
                data.SaveChanges();
                return Json(new { code = 200, msg = "Thêm thành công"},JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm thất bại " + ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        // Cập Nhật
        [HttpPost]
        public ActionResult CapNhat(DIENTHOAI model)
        {

            

            try {
                var dt = data.DIENTHOAIs.SingleOrDefault(x => x.MaDT == model.MaDT);
                dt.TenDienThoai = model.TenDienThoai;
                dt.MaTH = model.MaTH;
                dt.MoTa = model.MoTa;
                dt.SoLuongTon = model.SoLuongTon;
                dt.GiaBan = model.GiaBan;
                dt.GiamGia = model.GiamGia;
                dt.NgayCapNhat = model.NgayCapNhat;

                // kiểm tra input img có tồn tại không
                if (model.FileAnhBia != null && model.FileAnhBia.ContentLength > 0)
                {

                    var fileName = Path.GetFileNameWithoutExtension(model.FileAnhBia.FileName);
                    string extension = Path.GetExtension(model.FileAnhBia.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                    string rootPath = Server.MapPath("~"); // Lấy đường dẫn thư mục gốc của ứng dụng
                    string imagePath = Path.Combine(rootPath, "assets/img/sanPham", fileName);
                    model.FileAnhBia.SaveAs(imagePath);  // Lưu ảnh

                    // tạo đường dẫn đến file AnhBia cần thay thế
                    string fileOldImage = Path.Combine(rootPath, "assets/img/sanPham", dt.AnhBia);
                    if (System.IO.File.Exists(fileOldImage))
                    {
                        // Nếu tệp tồn tại, thực hiện xóa
                        System.IO.File.Delete(fileOldImage);
                    }
                    // gán giá trị mới cho dt.AnhBia
                    dt.AnhBia = fileName;
                }

                data.SaveChanges();
                return Json(new { code = 200, msg = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                // Lưu file ảnh vào server
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

           
        }

        // xoá
        [HttpPost]
        public ActionResult XoaSanPham(int id)
        {



            try
            {
                var dt = data.DIENTHOAIs.Find(id);
                dt.DaXoa = 1;
                data.SaveChanges();
                return Json(new { code = 200, msg = "Xoá thành công" }, JsonRequestBehavior.AllowGet);
                // Lưu file ảnh vào server
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xoá thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult ChiTiet(int id)
        {
            try
            {


                var sp = data.DIENTHOAIs.Where(x => x.MaDT == id).FirstOrDefault();
                var ketqua =  new
                {
                    MaDT = sp.MaDT,
                    TenDienThoai = sp.TenDienThoai,
                    MaTH = sp.MaTH,
                    GiaBan = sp.GiaBan,
                    MoTa = sp.MoTa,
                    AnhBia = sp.AnhBia,
                    SoLuongTon = sp.SoLuongTon,
                    GiamGia = sp.GiamGia,

                };
                return Json(new { code = 200, msg = "Lấy thông tin ok ", dt =ketqua }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại "+ ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}