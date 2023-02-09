using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestUserSQL
{
    public class Member
    {
        public int idnguoidung { get; set; }
        public string name { get; set; }
        public string tentaikhoan { get; set; }
        public string matkhau { get; set; }
        public int idquyen { get; set; }
        public Member()
        {
        }
        public Member(int idnguoidung, string name, string tentaikhoan, string pass, int idquyen)
        {
            this.idnguoidung = idnguoidung;
            this.name = name;
            this.tentaikhoan = tentaikhoan;
            this.idnguoidung = idnguoidung;
            this.idquyen = idquyen;
        }
       
    
    
    
    }
    
    

}