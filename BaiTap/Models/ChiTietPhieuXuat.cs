namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuXuat")]
    public partial class ChiTietPhieuXuat
    {
        public int ChiTietPhieuXuatID { get; set; }

        public int? PhieuXuatID { get; set; }

        public int? SanPhamID { get; set; }

        public int? SoLuong { get; set; }

        public double? DonGia { get; set; }

        public double? ThanhTien { get; set; }

        public virtual PhieuXuat PhieuXuat { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
