namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCao")]
    public partial class BaoCao
    {
        [Key]
        public int BCID { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày thực hiện báo cáo")]
        public DateTime? NgayBaoCao { get; set; }
        [DisplayName("Doanh số ")]
        public double DanhSo { get; set; }
        [DisplayName("Tổng số đơn hàng")]
        public int TongDonHang { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày cập nhật")]
        public DateTime? NgayCapNhatCC { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual DonHang DonHang { get; set; }
    }
}
