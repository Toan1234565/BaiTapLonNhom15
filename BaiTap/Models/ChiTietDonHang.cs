namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        
        public int ChiTietDonHangID { get; set; }
        [DisplayName("Mã đơn hàng")]
        public int? DonHangID { get; set; }
        [DisplayName("Mã sản phẩm")]
        public int? SanPhamID { get; set; }
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }
        [DisplayName("Giá")]
        public double? Gia { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
