using System;
using System.Linq;
using System.Web.Mvc;
using BaiTap.Models;

public class BaoCaoController : Controller
{
   private Model1 db = new Model1();

    public ActionResult Index()
    {
        // Lấy dữ liệu từ bảng BaoCao và DonHang
        var donHangs = db.DonHang.ToList();

        // Tính tổng doanh thu theo ngày
        var doanhThuTheoNgay = donHangs.GroupBy(dh => dh.NgayDatHang.Value.Date)
                                        .Select(g => new DoanhThuTheoNgay
                                        {
                                            Ngay = g.Key,
                                            TongTien = g.Sum(dh => dh.TongTien)
                                        }).ToList();

        // Truyền dữ liệu vào View
        return View(doanhThuTheoNgay);
    }
}

public class DoanhThuTheoNgay
{
    public DateTime Ngay { get; set; }
    public double? TongTien { get; set; }
}
