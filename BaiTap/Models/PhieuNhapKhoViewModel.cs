using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaiTap.Models
{
    public class PhieuNhapKhoViewModel
    {
        public int? SanPhamID { get; set; }
        [Required(ErrorMessage = "bạn chưa nhập số lượng nhập!")]
        public int soluong { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giá cho sản phẩm nhập!")]
        public double Gia { get; set; }
        public SanPham SanPham { get; set; }
        public HttpPostedFileBase HinhAnh { get; set; }
        public ChiTietSanPham ChiTietSanPham { get; set; }
        public TonKho TonKho { get; set; }
        //public string DanhMucMoi { get; set; }
        //public string HangsxMoi { get; set; }
    }

}