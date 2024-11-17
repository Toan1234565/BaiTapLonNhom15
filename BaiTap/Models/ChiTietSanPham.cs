namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietSanPham")]
    public partial class ChiTietSanPham
    {
        public int ChiTietSanPhamID { get; set; }
        [DisplayName("Mã sản phẩm")]
        public int? SanPhamID { get; set; }
        [DisplayName("Màn hình")]
        [StringLength(1000)]
        public string ManHinh { get; set; }
        [DisplayName("Hệ điều hành")]
        [StringLength(1000)]
        public string HeDieuHanh { get; set; }
        [DisplayName("Camera trước")]
        [StringLength(1000)]
        public string CameraTruoc { get; set; }
        
        [StringLength(1000)]
        public string CameraSau { get; set; }

        [StringLength(1000)]
        public string Chip { get; set; }

        [StringLength(1000)]
        public string RAM { get; set; }
        [DisplayName("Bộ nhớ trong")]
        [StringLength(1000)]
        public string BoNhoTrong { get; set; }

        [StringLength(1000)]
        public string Sim { get; set; }

        [StringLength(1000)]
        public string Pin { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
