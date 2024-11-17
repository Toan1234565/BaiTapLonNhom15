namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuXuat")]
    public partial class PhieuXuat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuXuat()
        {
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
        }

        public int PhieuXuatID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayXuat { get; set; }

        [StringLength(100)]
        public string Kho { get; set; }

        public int? KhachHangID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
