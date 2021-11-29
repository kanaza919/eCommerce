//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eCommerce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            this.BienDongSoDus = new HashSet<BienDongSoDu>();
            this.DanhGias = new HashSet<DanhGia>();
            this.DanhGias1 = new HashSet<DanhGia>();
            this.DauGias = new HashSet<DauGia>();
            this.DauGias1 = new HashSet<DauGia>();
            this.Loais = new HashSet<Loai>();
            this.MucNangs = new HashSet<MucNang>();
            this.ThongBaos = new HashSet<ThongBao>();
            this.TrangThaiDauGias = new HashSet<TrangThaiDauGia>();
        }
    
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public System.DateTime NgaySinh { get; set; }
        public string SoCMND { get; set; }
        public string HinhAnh { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public System.DateTime NgayDangKy { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsApproved { get; set; }
        public int SoDuVi { get; set; }
        public bool IsRequesting { get; set; }
    
        public virtual ICollection<BienDongSoDu> BienDongSoDus { get; set; }
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        public virtual ICollection<DanhGia> DanhGias1 { get; set; }
        public virtual ICollection<DauGia> DauGias { get; set; }
        public virtual ICollection<DauGia> DauGias1 { get; set; }
        public virtual ICollection<Loai> Loais { get; set; }
        public virtual ICollection<MucNang> MucNangs { get; set; }
        public virtual ICollection<ThongBao> ThongBaos { get; set; }
        public virtual ICollection<TrangThaiDauGia> TrangThaiDauGias { get; set; }
    }
}
