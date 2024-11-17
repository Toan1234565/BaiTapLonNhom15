namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuNhap")]
    public partial class ChiTietPhieuNhap
    {
        public int ChiTietPhieuNhapID { get; set; }

        public int? PhieuNhapID { get; set; }

        public int? SanPhamID { get; set; }

        public int? SoLuong { get; set; }

        public double? DonGia { get; set; }

        public double? ThanhTien { get; set; }

        public virtual PhieuNhap PhieuNhap { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
