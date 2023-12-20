using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AspShopPhone2.Models
{
    public class GioHang
    {
        QLDienThoaiEntities data = new QLDienThoaiEntities();
        public int dMaDT { get; set; }
        public string dTenDT { get; set; }
        public string dAnhBia { get; set; }
        public double dDongGia { get; set; }
        public int dSoLuong { get; set; }
        public string dtenTH { get; set; }

        public double dThanhTien
        {
            get { return dSoLuong * dDongGia; }
        }

        

        public double TinhGiaMoi(double giaBan, double giaGiam)
        {

            double giaMoi = (double)giaBan - ((double)giaGiam * (double)giaBan) / 100;
            return Math.Floor(giaMoi / 10000) * 10000;
        }
        

        public GioHang(int dMaDT)
        {
            DIENTHOAI dienthoai = data.DIENTHOAIs.Single(dt => dt.MaDT == dMaDT);
            THUONGHIEU thuonghieu = data.THUONGHIEUx.Single(th => th.MaTH == dienthoai.MaTH);
            this.dMaDT = dMaDT;
            dtenTH = thuonghieu.TenThuongHieu;
            dTenDT = dienthoai.TenDienThoai;
            dAnhBia = dienthoai.AnhBia;
            dDongGia = TinhGiaMoi((double)dienthoai.GiaBan, (double)dienthoai.GiamGia);

            dSoLuong = 1;
        }

        
    }
}