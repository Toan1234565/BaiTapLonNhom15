namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sosanh")]
    public partial class Sosanh
    {
        public int SosanhID { get; set; }

        public int? SPID1 { get; set; }

        public int? SPID2 { get; set; }

        public string Noidung { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual SanPham SanPham1 { get; set; }
    }
}
