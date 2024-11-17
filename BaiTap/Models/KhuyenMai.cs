namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhuyenMai")]
    public partial class KhuyenMai
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhuyenMai()
        {
            SanPhamKhuyenMai = new HashSet<SanPhamKhuyenMai>();
        }

        public int KhuyenMaiID { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhuyenMai { get; set; }

        [Column(TypeName = "text")]
        public string Mota { get; set; }


        [Column(TypeName = "date")]
        public DateTime NgayBD { get; set; }


        [Column(TypeName = "date")]
        public DateTime NgayKT { get; set; }


        [Required]
        [StringLength(50)]
        public string LoaiKM { get; set; }
        [Required]
        public decimal GiaTri {  get; set; }// ty le giam gia 

        [Column(TypeName = "text")]
        public string DieuKien { get; set; }

        public int? Soluong {  get; set; }
        
        public double GiaTriDonHangToiThieu { get; set; } // Giá trị đơn hàng tối thiểu để được giảm giá

        public int DiemTichLuyToiThieu { get; set; } // Điểm tích lũy tối thiểu để được giảm giá
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPhamKhuyenMai> SanPhamKhuyenMai { get; set; }
    }
}
