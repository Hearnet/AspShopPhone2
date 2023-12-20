using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AspShopPhone2.Models;

namespace AspShopPhone2.Areas.admin.Controllers
{
    public class ThuongHieuController : Controller
    {
        QLDienThoaiEntities data = new QLDienThoaiEntities();
        // GET: admin/ThuongHieu
        public ActionResult DanhSachThuongHieu()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsThuongHieu(string name, int page, int pageSize = 3)
        {
            try
            {
                List<ChiTietThuongHieu> DSthuonghieu = (from d in data.THUONGHIEUx.Where(x => x.DaXoa != 1)
                                                    select new ChiTietThuongHieu
                                                    {
                                                        maTH = d.MaTH,
                                                        tenThuongHieu = d.TenThuongHieu,
                                                        codeThuongHieu = d.CodeThuongHieu
                                                    }).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    DSthuonghieu = DSthuonghieu.Where(item => item.tenThuongHieu.ToUpper().Contains(name.ToUpper())).ToList();
                };

                int totalRow = DSthuonghieu.Count();
                DSthuonghieu = DSthuonghieu.OrderByDescending(x => x.tenThuongHieu) // Sắp xếp
                       .Skip((page - 1) * pageSize) // Bỏ qua mục
                       .Take(pageSize) // Lấy số lượng mục
                       .ToList(); // Chuyển danh sách kết quả sang danh sách đã phân trang
                return Json(new
                {
                    code = 200,
                    total = totalRow,
                    data = DSthuonghieu,
                    msg = "Lấy sản phẩm thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy sản phẩm thất bại: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ChiTiet(int id)
        {
            try
            {
                var th = data.THUONGHIEUx.Where(x => x.MaTH == id).FirstOrDefault();
                var ketqua = new
                {
                    MaTH = th.MaTH,
                    TenThuongHieu = th.TenThuongHieu,
                    CodeThuongHieu = th.CodeThuongHieu,

                };
                return Json(new { code = 200, msg = "Lấy thông tin ok ", th = ketqua }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }


        //Cập nhật
        [HttpPost]
        public ActionResult CapNhat(THUONGHIEU model)
        {

            try
            {
                var th = data.THUONGHIEUx.SingleOrDefault(x => x.MaTH == model.MaTH);
                th.TenThuongHieu = model.TenThuongHieu;
                th.CodeThuongHieu = model.CodeThuongHieu;

                data.SaveChanges();
                return Json(new { code = 200, msg = "Sửa thành công" }, JsonRequestBehavior.AllowGet);
                // Lưu file ảnh vào server
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }


        // Thêm mới
        [HttpPost]
        public ActionResult ThemThuongHieu(THUONGHIEU model)
        {
            try
            {
                model.DaXoa = 0;
                data.THUONGHIEUx.Add(model);
                data.SaveChanges();
                return Json(new { code = 200, msg = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // Xoá
        [HttpPost]
        public ActionResult XoaThuongHieu(int id)
        {
            try
            {
                var dt = data.THUONGHIEUx.Find(id);
                dt.DaXoa = 1;
                data.SaveChanges();
                return Json(new { code = 200, msg = "Xoá thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xoá thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}