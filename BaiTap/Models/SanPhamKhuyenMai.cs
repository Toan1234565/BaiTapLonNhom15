namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPhamKhuyenMai")]
    public partial class SanPhamKhuyenMai
    {
        public int SanPhamKhuyenMaiID { get; set; }

        public int? SanPhamID { get; set; }

        public int? KhuyenMaiID { get; set; }

        public virtual KhuyenMai KhuyenMai { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
