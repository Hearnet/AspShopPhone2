using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShopPhone2.Models
{
    public class ChiTietSanPham
    {
        public decimal giaBan { get; set; }
        public int giaGiam { get; set; }
        public string tenDienThoai { get; set; }
        public int soLuongTon { get; set; }
        public string tenTH { get; set; }
        public string codeTH { get; set; }
        public string moTa { get; set; }
        public string anhBia { get; set; }
        public int id { get; set; }
        public int idTH { get; set; }
        public double GiaMoi { get; private set; }
        public DateTime NgayCapNhat { get; set; }

        public double TinhGiaMoi()
        {
           
            double giaMoi = (double)giaBan - ((double)giaGiam * (double)giaBan) / 100;
            return Math.Floor(giaMoi / 10000) * 10000;
        }
       

    }

   
}