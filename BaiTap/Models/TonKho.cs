namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TonKho")]
    public partial class TonKho
    {
        public int TonKhoID { get; set; }

        public int? SanPhamID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhap so luong!")]
        public int? SoLuongTon { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayCapNhat { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
