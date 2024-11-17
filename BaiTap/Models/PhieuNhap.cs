namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuNhap")]
    public partial class PhieuNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhap()
        {
            ChiTietPhieuNhap = new HashSet<ChiTietPhieuNhap>();
        }

        public int PhieuNhapID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayNhap { get; set; }

        [StringLength(100)]
        public string NhaCungCap { get; set; }

        [StringLength(100)]
        public string Kho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
    }
}
