//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eCommerce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThongBao
    {
        public int MaThongBao { get; set; }
        public int MaDauGia { get; set; }
        public int MaNguoiDung { get; set; }
        public System.DateTime ThoiGian { get; set; }
        public string TinNhan { get; set; }
    
        public virtual DauGia DauGia { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
