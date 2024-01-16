using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspShopPhone2.Models;
using PagedList;
using PagedList.Mvc;
namespace AspShopPhone2.Controllers
{
    public class PhoneStoreController : Controller
    {
        // GET: PhoneStore
        int pageSize = 8; // số sản phẩm mặc định mỗi trang

        QLDienThoaiEntities data = new QLDienThoaiEntities();

        public ActionResult Index(int ? page)
        {
            int pageNum = (page ?? 1); // giá trị trang hiện tại
           
            return View(querySearch().ToList().ToPagedList(pageNum, pageSize));
        }


       
        // Render ra danh mục thương hiệu
        public ActionResult RenderThuongHieu()
        {
            var dsThuongHieu = data.THUONGHIEUx.ToList();
            ViewBag.thuonghieu = dsThuongHieu;
            return PartialView("_ThuongHieu");
        }
        private List<ChiTietSanPham> querySearch(/*string searchName, bool isUpPrice*/)
        {
            //List<ChiTietSanPham> products = new List<ChiTietSanPham>();
            // string searchName = Session["SearchName"] as string;
            // string searchPrice = Session["searchPrice"] as string;

            //if (!string.IsNullOrEmpty(searchName))
            //{
            //    if (!string.IsNullOrEmpty(searchPrice))
            //    {
            //        bool isUpPrice = bool.Parse(searchPrice);
            //        if (isUpPrice)
            //        {
            //            products = (from a in data.DIENTHOAIs
            //                        join b in data.THUONGHIEUx
            //                        on a.MaTH equals b.MaTH
            //                        where a.TenDienThoai.ToUpper().Contains(searchName.ToUpper())
            //                        orderby a.GiaBan ascending
            //                        select new ChiTietSanPham
            //                        {
            //                            id = a.MaDT,
            //                            tenDienThoai = a.TenDienThoai,
            //                            giaBan = (decimal)a.GiaBan,
            //                            giaGiam = (int)a.GiamGia,
            //                            codeTH = b.CodeThuongHieu,
            //                            tenTH = b.TenThuongHieu,
            //                            anhBia = a.AnhBia,
            //                            moTa = a.MoTa,
            //                            soLuongTon = (int)a.SoLuongTon
            //                        }).ToList();
            //        }
            //        else
            //        {
            //            products = (from a in data.DIENTHOAIs
            //                        join b in data.THUONGHIEUx
            //                        on a.MaTH equals b.MaTH
            //                        where a.TenDienThoai.ToUpper().Contains(searchName.ToUpper())
            //                        orderby a.GiaBan descending
            //                        select new ChiTietSanPham
            //                        {
            //                            id = a.MaDT,
            //                            tenDienThoai = a.TenDienThoai,
            //                            giaBan = (decimal)a.GiaBan,
            //                            giaGiam = (int)a.GiamGia,
            //                            codeTH = b.CodeThuongHieu,
            //                            tenTH = b.TenThuongHieu,
            //                            anhBia = a.AnhBia,
            //                            moTa = a.MoTa,
            //                            soLuongTon = (int)a.SoLuongTon
            //                        }).ToList();
            //        }
            //    }
            //    else products = querySearchByName(searchName);


            //} else
            //{
            //    // Trường hợp không có giá trị tìm kiếm từ ViewBag, trả về tất cả sản phẩm
            //    if (!string.IsNullOrEmpty(searchPrice))
            //    {
            //        bool isUpPrice = bool.Parse(searchPrice);
            //        if (isUpPrice)
            //        {
            //            products = (from a in data.DIENTHOAIs
            //                        join b in data.THUONGHIEUx
            //                        on a.MaTH equals b.MaTH
            //                        orderby a.GiaBan ascending
            //                        select new ChiTietSanPham
            //                        {
            //                            id = a.MaDT,
            //                            tenDienThoai = a.TenDienThoai,
            //                            giaBan = (decimal)a.GiaBan,
            //                            giaGiam = (int)a.GiamGia,
            //                            codeTH = b.CodeThuongHieu,
            //                            tenTH = b.TenThuongHieu,
            //                            anhBia = a.AnhBia,
            //                            moTa = a.MoTa,
            //                            soLuongTon = (int)a.SoLuongTon
            //                        }).ToList();
            //        }
            //        else
            //        {
            //            products = (from a in data.DIENTHOAIs
            //                        join b in data.THUONGHIEUx
            //                        on a.MaTH equals b.MaTH
            //                        orderby a.GiaBan descending
            //                        select new ChiTietSanPham
            //                        {
            //                            id = a.MaDT,
            //                            tenDienThoai = a.TenDienThoai,
            //                            giaBan = (decimal)a.GiaBan,
            //                            giaGiam = (int)a.GiamGia,
            //                            codeTH = b.CodeThuongHieu,
            //                            tenTH = b.TenThuongHieu,
            //                            anhBia = a.AnhBia,
            //                            moTa = a.MoTa,
            //                            soLuongTon = (int)a.SoLuongTon
            //                        }).ToList();
            //        }
            //    } else
            //    {
            //        products = (from a in data.DIENTHOAIs
            //                    join b in data.THUONGHIEUx
            //                    on a.MaTH equals b.MaTH
            //                    orderby a.NgayCapNhat descending
            //                    select new ChiTietSanPham
            //                    {
            //                        id = a.MaDT,
            //                        tenDienThoai = a.TenDienThoai,
            //                        giaBan = (decimal)a.GiaBan,
            //                        giaGiam = (int)a.GiamGia,
            //                        codeTH = b.CodeThuongHieu,
            //                        tenTH = b.TenThuongHieu,
            //                        anhBia = a.AnhBia,
            //                        moTa = a.MoTa,
            //                        soLuongTon = (int)a.SoLuongTon
            //                    }).ToList();
            //    }

            //}

            //return products;
        string searchName = Session["SearchName"] as string;
        string searchPrice = Session["SearchPrice"] as string;
        string searchPriceRange = Session["SearchPriceRange"] as string;
        var query = from a in data.DIENTHOAIs
                    join b in data.THUONGHIEUx
                    on a.MaTH equals b.MaTH
                    where (string.IsNullOrEmpty(searchName) || a.TenDienThoai.ToUpper().Contains(searchName.ToUpper())) &&
                  (string.IsNullOrEmpty(searchPriceRange) ||
                   (searchPriceRange == "less3" && a.GiaBan < 3000000) ||
                   (searchPriceRange == "3and10" && a.GiaBan >= 3000000 && a.GiaBan <= 10000000) ||
                   (searchPriceRange == "more10" && a.GiaBan > 10000000))
                    select new ChiTietSanPham
                    {
                        id = a.MaDT,
                        tenDienThoai = a.TenDienThoai,
                        giaBan = (decimal)a.GiaBan,
                        giaGiam = (int)a.GiamGia,
                        codeTH = b.CodeThuongHieu,
                        tenTH = b.TenThuongHieu,
                        anhBia = a.AnhBia,
                        moTa = a.MoTa,
                        soLuongTon = (int)a.SoLuongTon,
                        NgayCapNhat = (DateTime)a.NgayCapNhat
                    };

        if (!string.IsNullOrEmpty(searchPrice))
        {
            bool isUpPrice = bool.Parse(searchPrice);
            query = isUpPrice ? query.OrderBy(a => a.giaBan) : query.OrderByDescending(a => a.giaBan);
        }
        else
        {
            query = query.OrderByDescending(a => a.NgayCapNhat);
        }
            return query.ToList();

        }

        
        // Hàm kiểm tra khoảng giá
        private bool CheckPriceRange(decimal price, string searchPriceRange)
        {
            if (searchPriceRange == "less3")
            {
                return price < 3000000;
            }
            else if (searchPriceRange == "3and10")
            {
                return price >= 3000000 && price <= 10000000;
            }
            else if (searchPriceRange == "more10")
            {
                return price > 10000000;
            }
            return true;
        }
        //Tìm kiếm sản phẩm theo tên
        [HttpPost]
        public ActionResult SearchByName(string searchName, int? page)
        {
            int pageNumber = page ?? 1;
            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrEmpty(searchName))
            {
                Session["SearchName"] = searchName;
                
                return View("Index", querySearch().ToPagedList(pageNumber, pageSize));
            } else return RedirectToAction("Index");
        }
        //sắp xếp sản phẩm theo giá
        public ActionResult SearchByPrice(bool priceUp, int ? page)
        {
            int pageNumber = page ?? 1;
            Session["SearchPrice"] = priceUp.ToString();

            return View("Index", querySearch().ToPagedList(pageNumber, pageSize));
            

            
        }

        //tìm kiếm sản phẩm theo giá
        public ActionResult SearchByPriceRange(string priceRange, int? page)
        {
            int pageNumber = page ?? 1;
            Session["SearchPriceRange"] = priceRange.ToString();

            return View("Index", querySearch().ToPagedList(pageNumber, pageSize));



        }
        // Xử lí url 
        public ActionResult ViewUrlThuongHieu(int id)
        {
            var query = (from a in data.THUONGHIEUx
                         where a.MaTH == id
                         select new
                         {
                             Name = a.TenThuongHieu
                         }).First();
            if (query != null)
            {
                string NewName = query.Name.ToFriendlyUrl();
                string friendlyUrl = $"/phone-store/thuong-hieu/{NewName}/{id}";
                // Thực hiện chuyển hướng (redirect) đến URL thân thiện
                return Redirect(friendlyUrl);
            }
            else
            {
                // Xử lý khi không tìm thấy sản phẩm
                return HttpNotFound();
            }
        }

        // Lọc sản phẩm theo thương hiệu
        [Route("phone-store/thuong-hieu/{NewName}/{id}")]
        public ActionResult SanPhamTheoThuongHieu(string NewName, int? id, int ? page)
        {
            int pageSize = 20;
            int pageNum = (page ?? 1);
            if (id.HasValue)
            {
                var query = from a in data.DIENTHOAIs
                            join b in data.THUONGHIEUx
                            on a.MaTH equals b.MaTH
                            where a.MaTH == id
                            orderby a.NgayCapNhat descending
                            select new ChiTietSanPham
                            {
                                tenDienThoai = a.TenDienThoai,
                                giaBan = (decimal)a.GiaBan,
                                giaGiam = (int)a.GiamGia,
                                codeTH = b.CodeThuongHieu,
                                tenTH = b.TenThuongHieu,
                                anhBia = a.AnhBia,
                                moTa = a.MoTa,
                                soLuongTon = (int)a.SoLuongTon,
                                id = a.MaDT
                            };
                return View(query.ToList().ToPagedList(pageNum, pageSize));
            }
            else
            {
                return View();
            }

        }

        [Route("phone-store/san-pham/{id}")]
        public ActionResult ViewDetail(int id)
        {
            var query = (from a in data.DIENTHOAIs where a.MaDT == id select new {
                Name = a.TenDienThoai
            }).First();
            if (query != null)
            {
                string productName = query.Name.ToFriendlyUrl();
                string friendlyUrl = $"{productName}/{id}";
                // Thực hiện chuyển hướng (redirect) đến URL thân thiện
                return Redirect(friendlyUrl);
            }
            else
            {
                // Xử lý khi không tìm thấy sản phẩm
                return HttpNotFound();
            }
        }


        //Xử lí trang chi tiết sản phẩm
        [Route("phone-store/san-pham/{productName}/{id}")]
        public ActionResult Details(string productName, int id)
        {
            var query = from a in data.DIENTHOAIs
                        join b in data.THUONGHIEUx
                        on a.MaTH equals b.MaTH
                        where a.MaDT == id
                        select new ChiTietSanPham
                        {
                            id = a.MaDT,
                            tenDienThoai = a.TenDienThoai,
                            giaBan = (decimal)a.GiaBan,
                            giaGiam = (int)a.GiamGia,
                            codeTH = b.CodeThuongHieu,
                            tenTH = b.TenThuongHieu,
                            anhBia = a.AnhBia,
                            moTa = a.MoTa,
                            soLuongTon = (int)a.SoLuongTon,
                            idTH = b.MaTH
                        };

            ChiTietSanPham product = query.First();

            // Đọc danh sách sản phẩm đã xem từ cookie
            List<ChiTietSanPham> viewedProducts = GetViewedProductsFromCookie();
            var existingProduct = viewedProducts.FirstOrDefault(p => p.id == id);
            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách chưa
            if (existingProduct != null)
            {
                // Thêm sản phẩm vào danh sách đã xem

                viewedProducts.Remove(existingProduct);
                viewedProducts.Insert(0, product);
                
            } else
            {
                viewedProducts.Insert(0, product);
            }
            // Lưu danh sách sản phẩm đã xem vào cookie
            SaveViewedProductsToCookie(viewedProducts);
            return View(product);
        }


        // Xử Lí SẢN Phẩm ĐÃ Xem

        // Hàm đọc danh sách sản phẩm đã xem từ cookie
        private List<ChiTietSanPham> GetViewedProductsFromCookie()
        {
            // Đọc danh sách sản phẩm đã xem từ cookie
            HttpCookie viewedProductsCookie = HttpContext.Request.Cookies["ViewedProducts"];

            if (viewedProductsCookie != null)
            {
                // Parse dữ liệu từ cookie và chuyển đổi thành danh sách sản phẩm
                // Lưu ý: Trong thực tế, bạn cần xử lý phần parse dữ liệu từ cookie theo cấu trúc dữ liệu của bạn
                string[] productIds = viewedProductsCookie.Value.Split(',');

                List<ChiTietSanPham> viewedProducts = new List<ChiTietSanPham>();
                foreach (string productId in productIds)
                {
                    //int idsanpham = int.Parse(productId);
                    //var query = from a in data.DIENTHOAIs
                    //            join b in data.THUONGHIEUx
                    //            on a.MaTH equals b.MaTH
                    //            where a.MaDT == idsanpham
                    //            select new ChiTietSanPham
                    //            {
                    //                id = a.MaDT,
                    //                tenDienThoai = a.TenDienThoai,
                    //                giaBan = (decimal)a.GiaBan,
                    //                giaGiam = (int)a.GiamGia,
                    //                codeTH = b.CodeThuongHieu,
                    //                tenTH = b.TenThuongHieu,
                    //                anhBia = a.AnhBia,
                    //                moTa = a.MoTa,
                    //                soLuongTon = (int)a.SoLuongTon,
                    //                idTH = b.MaTH
                    //            };
                    //ChiTietSanPham product = query.First();
                    //if (product != null)
                    //{
                    //    viewedProducts.Add(product);
                    //}

                    if (int.TryParse(productId, out int idsanpham))
                    {
                        var query = from a in data.DIENTHOAIs
                                    join b in data.THUONGHIEUx
                                    on a.MaTH equals b.MaTH
                                    where a.MaDT == idsanpham
                                    select new ChiTietSanPham
                                    {
                                        id = a.MaDT,
                                        tenDienThoai = a.TenDienThoai,
                                        giaBan = (decimal)a.GiaBan,
                                        giaGiam = (int)a.GiamGia,
                                        codeTH = b.CodeThuongHieu,
                                        tenTH = b.TenThuongHieu,
                                        anhBia = a.AnhBia,
                                        moTa = a.MoTa,
                                        soLuongTon = (int)a.SoLuongTon,
                                        idTH = b.MaTH
                                    };
                        ChiTietSanPham product = query.First();
                        if (product != null)
                        {
                            viewedProducts.Add(product);
                        }
                    }
                }

                return viewedProducts;
            }

            return new List<ChiTietSanPham>();
        }

        // Hàm lưu danh sách sản phẩm đã xem vào cookie
        private void SaveViewedProductsToCookie(List<ChiTietSanPham> viewedProducts)
        {
            // Chuyển đổi danh sách sản phẩm thành chuỗi
            string productIds = string.Join(",", viewedProducts.Select(p => p.id.ToString()));

            // Tạo cookie và lưu vào Response
            HttpCookie cookie = new HttpCookie("ViewedProducts", productIds);
            HttpContext.Response.Cookies.Add(cookie);
        }

        public ActionResult SanPhamDaXem(int? page)
        {
            int pageNum = (page ?? 1); // giá trị trang hiện tại

            List<ChiTietSanPham> viewedProducts = GetViewedProductsFromCookie();
            return View(viewedProducts.ToList().ToPagedList(pageNum, pageSize));
        }

    }
}