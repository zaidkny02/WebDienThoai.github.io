using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestUserSQL.Class
{
    public class GioHang
    {
        public string username { get; set; }
       // public ArrayList<SanPham> arrsp { get; set; }
        
        public List<SanPham> arrsp  { get; set; }
        public GioHang()
        {
            arrsp = new List<SanPham>();
        }
        public GioHang(int idnguoidung, List<SanPham> arrsp)
        {
        this.username = username;
        this.arrsp = arrsp;
        }
        
    }
}