namespace BaiTap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Admins
    {
        [Key]
        public int admin_id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Tên đăng nhập")]
        public string username { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Mật khẩu")]
        public string password { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Họ tên nhân vien")]
        public string full_name { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(20)]
        public string role { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; }
    }
}
