using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTap.Models
{
    public class KM
    {
    
        public float  GiaTriDonHangToiThieu { get; set; } // Giá trị đơn hàng tối thiểu để được giảm giá
        public int DiemTichLuyToiThieu { get; set; } // Điểm tích lũy tối thiểu để được giảm giá
        public KhuyenMai KhuyenMai { get; set; }
        public KhachHang KhachHang { get; set; }
        public SanPhamKhuyenMai SanPhamKhuyenMai{ get; set; }
    }
}