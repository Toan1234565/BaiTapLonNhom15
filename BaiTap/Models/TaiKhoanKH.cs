namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoanKH")]
    public partial class TaiKhoanKH
    {
        [Key]
        public int TaiKhoanID { get; set; }

        public int? KhachHangID { get; set; }

        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
