namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichSuDonHang")]
    public partial class LichSuDonHang
    {
        public int LichSuDonHangID { get; set; }

        public int? KhachHangID { get; set; }

        public int? DonHangID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDatHang { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
