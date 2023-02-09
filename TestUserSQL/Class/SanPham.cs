using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestUserSQL.Class
{
    public class SanPham
    {
        public int idsanpham { get; set; }
        public int soluong { get; set; }
        public long dongia { get; set; }
        public string stringMau { get; set; }
        public string stringBoNho { get; set; }
        public SanPham()
        {
        }
        public SanPham(int idsanpham, int soluong, long dongia, string stringMau, string stringBoNho)
        {
            this.idsanpham = idsanpham;

            this.soluong = soluong;
            this.dongia = dongia;
            this.stringMau = stringMau;
            this.stringBoNho = stringBoNho;
        }
    }
}