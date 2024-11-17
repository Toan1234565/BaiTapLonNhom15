using Antlr.Runtime.Misc;
using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        private Model1 db = new Model1();

        // GET: QuanLyDonHang
        public ActionResult Index()
        {
            List<DonHang> dsDonHang = db.DonHang.ToList();
            return View(dsDonHang);
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var chitiet = db.ChiTietDonHang.Where( x => x.DonHangID == id ).ToList();
            if (chitiet == null)
            {
                return HttpNotFound();
            }
            return View(chitiet);
        }
        
        // GET: QuanLyDonHang/TaoDonHang
        public ActionResult TaoDonHang()
        {
            ViewBag.KhachHangID = new SelectList(db.KhachHang, "KhachHangID", "TenKhachHang");
            ViewBag.SanPhamID = new SelectList(db.SanPham, "SanPhamID", "TenSanPham");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoDonHang(DonHang donHang, List<int?> SanPhamID, List<int?> SoLuong)
        {
            if (ModelState.IsValid)
            {
                var khachhang = db.KhachHang.FirstOrDefault(kh => kh.SoDienThoai == donHang.KhachHang.SoDienThoai);
                if (khachhang == null)
                {
                    khachhang = new KhachHang()
                    {
                        HoTen = donHang.KhachHang.HoTen,
                        SoDienThoai = donHang.KhachHang.SoDienThoai
                    };
                    db.KhachHang.Add(khachhang);
                    db.SaveChanges();
                }
                else
                {
                    khachhang.DiemTichLuy += (int)(donHang.TongTien * 0.10);
                    db.Entry(khachhang).State = EntityState.Modified;
                }
               
                donHang.KhachHangID = khachhang.KhachHangID;
                donHang.NgayDatHang = DateTime.Now;
                db.DonHang.Add(donHang);
                db.SaveChanges();

                double tongtien = 0;
                for (int i = 0; i < SanPhamID.Count; i++)
                {
                    if (SanPhamID[i].HasValue && SoLuong[i].HasValue)
                    {
                        var sanpham = db.SanPham.Find(SanPhamID[i].Value);
                        
                        if (sanpham != null)
                        {
                            ChiTietDonHang ct = new ChiTietDonHang()
                            {
                                DonHangID = donHang.DonHangID,
                                SanPhamID = SanPhamID[i].Value,
                                SoLuong = SoLuong[i].Value,
                                Gia = sanpham.Gia
                            };
                            db.ChiTietDonHang.Add(ct);
                            tongtien +=SoLuong[i].Value *(double)sanpham.Gia;
                            UpdateTonKho(sanpham.SanPhamID, SoLuong[i].Value);
                        
                        }
                    }
                }
                donHang.TongTien = tongtien;
                khachhang.DiemTichLuy += (int)(tongtien * 0.10);
                db.SaveChanges();
              
                if (donHang.NgayDatHang.HasValue) {
                    UpdateBaoCaoDoanhSo(donHang.NgayDatHang.Value,(double)donHang.TongTien);
                }
                return RedirectToAction("Index");
            }

            ViewBag.KhachHangID = new SelectList(db.KhachHang, "KhachHangID", "TenKhachHang", donHang.KhachHangID);
            ViewBag.SanPhamID = new SelectList(db.SanPham, "SanPhamID", "TenSanPham");
            return View(donHang);
        }

        public ActionResult SuaDonHang(int id)
        {
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhachHangID = new SelectList(db.KhachHang, "KhachHangID", "TenKhachHang", "SoDienThoai", donHang.KhachHangID);
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDonHang(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhachHangID = new SelectList(db.KhachHang, "KhachHangID", "TenKhachHang", "SoDienThoai", donHang.KhachHangID);
            return View(donHang);
        }

        public ActionResult XoaDonHang(int id)
        {
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        [HttpPost, ActionName("XoaDonHang")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaDonHangConfirmed(int id)
        {
            ChiTietDonHang chitiet = db.ChiTietDonHang.Find(id);
            DonHang donHang = db.DonHang.Find(id);
            db.ChiTietDonHang.Remove(chitiet);
            db.DonHang.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void UpdateBaoCaoDoanhSo(DateTime ngayDatHang, double tongtien) 
        { 
            var baoCao = db.BaoCao.FirstOrDefault(b => b.NgayBaoCao == ngayDatHang);
            if (baoCao == null) 
            { 
                baoCao = new BaoCao
                { 
                    NgayBaoCao = ngayDatHang,
                    DanhSo = tongtien, 
                    TongDonHang = 1 };
              db.BaoCao.Add(baoCao);
            } 
            else { 
                baoCao.DanhSo += tongtien; 
                baoCao.TongDonHang += 1;
            } 
            db.SaveChanges();
        }
        private void UpdateTonKho(int ID, int soluong)
        {
            var tonkho = db.TonKho.FirstOrDefault(tk =>tk.SanPhamID == soluong);
            if(tonkho != null)
            {
                tonkho.SoLuongTon -= soluong;
                tonkho.NgayCapNhat = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}
