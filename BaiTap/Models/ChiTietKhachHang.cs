namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietKhachHang")]
    public partial class ChiTietKhachHang
    {
        public int ChiTietKhachHangID { get; set; }
        [DisplayName("Mã sản phẩm")]
        public int? KhachHangID { get; set; }
        [DisplayName("Màn hình")]
        [StringLength(1000)]
        public string HoTen { get; set; }
        [DisplayName("Hệ điều hành")]
        [StringLength(1000)]
        public string Email { get; set; }
        [DisplayName("Camera trước")]
        [StringLength(1000)]
        public string SoDienThoai { get; set; }

        [StringLength(1000)]
        public string DiaChi { get; set; }

        [StringLength(1000)]
        public string DiemTichLuy { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
