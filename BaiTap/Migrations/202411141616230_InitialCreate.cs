namespace BaiTap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        admin_id = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 50, unicode: false),
                        password = c.String(nullable: false, maxLength: 250, unicode: false),
                        full_name = c.String(nullable: false, maxLength: 100, unicode: false),
                        email = c.String(nullable: false, maxLength: 100, unicode: false),
                        role = c.String(nullable: false, maxLength: 20, unicode: false),
                        status = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.admin_id);
            
            CreateTable(
                "dbo.BaoCao",
                c => new
                    {
                        BCID = c.Int(nullable: false, identity: true),
                        NgayBaoCao = c.DateTime(storeType: "date"),
                        DanhSo = c.Double(nullable: false),
                        TongDonHang = c.Int(nullable: false),
                        NgayCapNhatCC = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.BCID);
            
            CreateTable(
                "dbo.ChiTietDonHang",
                c => new
                    {
                        ChiTietDonHangID = c.Int(nullable: false, identity: true),
                        DonHangID = c.Int(),
                        SanPhamID = c.Int(),
                        SoLuong = c.Int(),
                        Gia = c.Double(),
                    })
                .PrimaryKey(t => t.ChiTietDonHangID)
                .ForeignKey("dbo.DonHang", t => t.DonHangID)
                .ForeignKey("dbo.SanPham", t => t.SanPhamID)
                .Index(t => t.DonHangID)
                .Index(t => t.SanPhamID);
            
            CreateTable(
                "dbo.DonHang",
                c => new
                    {
                        DonHangID = c.Int(nullable: false, identity: true),
                        KhachHangID = c.Int(),
                        NgayDatHang = c.DateTime(storeType: "date"),
                        TrangThai = c.String(maxLength: 50),
                        TongTien = c.Double(),
                    })
                .PrimaryKey(t => t.DonHangID)
                .ForeignKey("dbo.KhachHang", t => t.KhachHangID)
                .Index(t => t.KhachHangID);
            
            CreateTable(
                "dbo.KhachHang",
                c => new
                    {
                        KhachHangID = c.Int(nullable: false, identity: true),
                        HoTen = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        MatKhau = c.String(maxLength: 100),
                        DiaChi = c.String(maxLength: 200),
                        SoDienThoai = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.KhachHangID);
            
            CreateTable(
                "dbo.LichSuDonHang",
                c => new
                    {
                        LichSuDonHangID = c.Int(nullable: false, identity: true),
                        KhachHangID = c.Int(),
                        DonHangID = c.Int(),
                        NgayDatHang = c.DateTime(storeType: "date"),
                        TrangThai = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LichSuDonHangID)
                .ForeignKey("dbo.DonHang", t => t.DonHangID)
                .ForeignKey("dbo.KhachHang", t => t.KhachHangID)
                .Index(t => t.KhachHangID)
                .Index(t => t.DonHangID);
            
            CreateTable(
                "dbo.PhieuXuat",
                c => new
                    {
                        PhieuXuatID = c.Int(nullable: false, identity: true),
                        NgayXuat = c.DateTime(storeType: "date"),
                        Kho = c.String(maxLength: 100),
                        KhachHangID = c.Int(),
                    })
                .PrimaryKey(t => t.PhieuXuatID)
                .ForeignKey("dbo.KhachHang", t => t.KhachHangID)
                .Index(t => t.KhachHangID);
            
            CreateTable(
                "dbo.ChiTietPhieuXuat",
                c => new
                    {
                        ChiTietPhieuXuatID = c.Int(nullable: false, identity: true),
                        PhieuXuatID = c.Int(),
                        SanPhamID = c.Int(),
                        SoLuong = c.Int(),
                        DonGia = c.Double(),
                        ThanhTien = c.Double(),
                    })
                .PrimaryKey(t => t.ChiTietPhieuXuatID)
                .ForeignKey("dbo.PhieuXuat", t => t.PhieuXuatID)
                .ForeignKey("dbo.SanPham", t => t.SanPhamID)
                .Index(t => t.PhieuXuatID)
                .Index(t => t.SanPhamID);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        SanPhamID = c.Int(nullable: false, identity: true),
                        TenSanPham = c.String(nullable: false, maxLength: 100),
                        Soluong = c.Int(),
                        MoTa = c.String(maxLength: 500),
                        Gia = c.Double(),
                        DanhMucID = c.Int(),
                        HangID = c.Int(),
                        HinhAnh = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.SanPhamID)
                .ForeignKey("dbo.Hang", t => t.HangID)
                .ForeignKey("dbo.DanhMuc", t => t.DanhMucID)
                .Index(t => t.DanhMucID)
                .Index(t => t.HangID);
            
            CreateTable(
                "dbo.ChiTietPhieuNhap",
                c => new
                    {
                        ChiTietPhieuNhapID = c.Int(nullable: false, identity: true),
                        PhieuNhapID = c.Int(),
                        SanPhamID = c.Int(),
                        SoLuong = c.Int(),
                        DonGia = c.Double(),
                        ThanhTien = c.Double(),
                    })
                .PrimaryKey(t => t.ChiTietPhieuNhapID)
                .ForeignKey("dbo.PhieuNhap", t => t.PhieuNhapID)
                .ForeignKey("dbo.SanPham", t => t.SanPhamID)
                .Index(t => t.PhieuNhapID)
                .Index(t => t.SanPhamID);
            
            CreateTable(
                "dbo.PhieuNhap",
                c => new
                    {
                        PhieuNhapID = c.Int(nullable: false, identity: true),
                        NgayNhap = c.DateTime(storeType: "date"),
                        NhaCungCap = c.String(maxLength: 100),
                        Kho = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PhieuNhapID);
            
            CreateTable(
                "dbo.ChiTietSanPham",
                c => new
                    {
                        ChiTietSanPhamID = c.Int(nullable: false, identity: true),
                        SanPhamID = c.Int(),
                        ManHinh = c.String(maxLength: 1000),
                        HeDieuHanh = c.String(maxLength: 1000),
                        CameraTruoc = c.String(maxLength: 1000),
                        CameraSau = c.String(maxLength: 1000),
                        Chip = c.String(maxLength: 1000),
                        RAM = c.String(maxLength: 1000),
                        BoNhoTrong = c.String(maxLength: 1000),
                        Sim = c.String(maxLength: 1000),
                        Pin = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.ChiTietSanPhamID)
                .ForeignKey("dbo.SanPham", t => t.SanPhamID)
                .Index(t => t.SanPhamID);
            
            CreateTable(
                "dbo.DanhMuc",
                c => new
                    {
                        DanhMucID = c.Int(nullable: false, identity: true),
                        TenDanhMuc = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DanhMucID);
            
            CreateTable(
                "dbo.Hang",
                c => new
                    {
                        HangID = c.Int(nullable: false, identity: true),
                        TenHang = c.String(maxLength: 100),
                        DanhMucID = c.Int(),
                    })
                .PrimaryKey(t => t.HangID)
                .ForeignKey("dbo.DanhMuc", t => t.DanhMucID)
                .Index(t => t.DanhMucID);
            
            CreateTable(
                "dbo.SanPhamKhuyenMai",
                c => new
                    {
                        SanPhamKhuyenMaiID = c.Int(nullable: false, identity: true),
                        SanPhamID = c.Int(),
                        KhuyenMaiID = c.Int(),
                    })
                .PrimaryKey(t => t.SanPhamKhuyenMaiID)
                .ForeignKey("dbo.KhuyenMai", t => t.KhuyenMaiID)
                .ForeignKey("dbo.SanPham", t => t.SanPhamID)
                .Index(t => t.SanPhamID)
                .Index(t => t.KhuyenMaiID);
            
            CreateTable(
                "dbo.KhuyenMai",
                c => new
                    {
                        KhuyenMaiID = c.Int(nullable: false, identity: true),
                        TenKhuyenMai = c.String(nullable: false, maxLength: 100, unicode: false),
                        Mota = c.String(unicode: false, storeType: "text"),
                        NgayBD = c.DateTime(nullable: false, storeType: "date"),
                        NgayKT = c.DateTime(nullable: false, storeType: "date"),
                        LoaiKM = c.String(nullable: false, maxLength: 50, unicode: false),
                        DieuKien = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.KhuyenMaiID);
            
            CreateTable(
                "dbo.Sosanh",
                c => new
                    {
                        SosanhID = c.Int(nullable: false, identity: true),
                        SPID1 = c.Int(),
                        SPID2 = c.Int(),
                        Noidung = c.String(),
                    })
                .PrimaryKey(t => t.SosanhID)
                .ForeignKey("dbo.SanPham", t => t.SPID1)
                .ForeignKey("dbo.SanPham", t => t.SPID2)
                .Index(t => t.SPID1)
                .Index(t => t.SPID2);
            
            CreateTable(
                "dbo.TonKho",
                c => new
                    {
                        TonKhoID = c.Int(nullable: false, identity: true),
                        SanPhamID = c.Int(),
                        SoLuongTon = c.Int(nullable: false),
                        NgayCapNhat = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.TonKhoID)
                .ForeignKey("dbo.SanPham", t => t.SanPhamID)
                .Index(t => t.SanPhamID);
            
            CreateTable(
                "dbo.TaiKhoanKH",
                c => new
                    {
                        TaiKhoanID = c.Int(nullable: false, identity: true),
                        KhachHangID = c.Int(),
                        TenDangNhap = c.String(maxLength: 50),
                        MatKhau = c.String(maxLength: 50),
                        TrangThai = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.TaiKhoanID)
                .ForeignKey("dbo.KhachHang", t => t.KhachHangID)
                .Index(t => t.KhachHangID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaiKhoanKH", "KhachHangID", "dbo.KhachHang");
            DropForeignKey("dbo.PhieuXuat", "KhachHangID", "dbo.KhachHang");
            DropForeignKey("dbo.TonKho", "SanPhamID", "dbo.SanPham");
            DropForeignKey("dbo.Sosanh", "SPID2", "dbo.SanPham");
            DropForeignKey("dbo.Sosanh", "SPID1", "dbo.SanPham");
            DropForeignKey("dbo.SanPhamKhuyenMai", "SanPhamID", "dbo.SanPham");
            DropForeignKey("dbo.SanPhamKhuyenMai", "KhuyenMaiID", "dbo.KhuyenMai");
            DropForeignKey("dbo.SanPham", "DanhMucID", "dbo.DanhMuc");
            DropForeignKey("dbo.SanPham", "HangID", "dbo.Hang");
            DropForeignKey("dbo.Hang", "DanhMucID", "dbo.DanhMuc");
            DropForeignKey("dbo.ChiTietSanPham", "SanPhamID", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietPhieuXuat", "SanPhamID", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietPhieuNhap", "SanPhamID", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietPhieuNhap", "PhieuNhapID", "dbo.PhieuNhap");
            DropForeignKey("dbo.ChiTietDonHang", "SanPhamID", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietPhieuXuat", "PhieuXuatID", "dbo.PhieuXuat");
            DropForeignKey("dbo.LichSuDonHang", "KhachHangID", "dbo.KhachHang");
            DropForeignKey("dbo.LichSuDonHang", "DonHangID", "dbo.DonHang");
            DropForeignKey("dbo.DonHang", "KhachHangID", "dbo.KhachHang");
            DropForeignKey("dbo.ChiTietDonHang", "DonHangID", "dbo.DonHang");
            DropIndex("dbo.TaiKhoanKH", new[] { "KhachHangID" });
            DropIndex("dbo.TonKho", new[] { "SanPhamID" });
            DropIndex("dbo.Sosanh", new[] { "SPID2" });
            DropIndex("dbo.Sosanh", new[] { "SPID1" });
            DropIndex("dbo.SanPhamKhuyenMai", new[] { "KhuyenMaiID" });
            DropIndex("dbo.SanPhamKhuyenMai", new[] { "SanPhamID" });
            DropIndex("dbo.Hang", new[] { "DanhMucID" });
            DropIndex("dbo.ChiTietSanPham", new[] { "SanPhamID" });
            DropIndex("dbo.ChiTietPhieuNhap", new[] { "SanPhamID" });
            DropIndex("dbo.ChiTietPhieuNhap", new[] { "PhieuNhapID" });
            DropIndex("dbo.SanPham", new[] { "HangID" });
            DropIndex("dbo.SanPham", new[] { "DanhMucID" });
            DropIndex("dbo.ChiTietPhieuXuat", new[] { "SanPhamID" });
            DropIndex("dbo.ChiTietPhieuXuat", new[] { "PhieuXuatID" });
            DropIndex("dbo.PhieuXuat", new[] { "KhachHangID" });
            DropIndex("dbo.LichSuDonHang", new[] { "DonHangID" });
            DropIndex("dbo.LichSuDonHang", new[] { "KhachHangID" });
            DropIndex("dbo.DonHang", new[] { "KhachHangID" });
            DropIndex("dbo.ChiTietDonHang", new[] { "SanPhamID" });
            DropIndex("dbo.ChiTietDonHang", new[] { "DonHangID" });
            DropTable("dbo.TaiKhoanKH");
            DropTable("dbo.TonKho");
            DropTable("dbo.Sosanh");
            DropTable("dbo.KhuyenMai");
            DropTable("dbo.SanPhamKhuyenMai");
            DropTable("dbo.Hang");
            DropTable("dbo.DanhMuc");
            DropTable("dbo.ChiTietSanPham");
            DropTable("dbo.PhieuNhap");
            DropTable("dbo.ChiTietPhieuNhap");
            DropTable("dbo.SanPham");
            DropTable("dbo.ChiTietPhieuXuat");
            DropTable("dbo.PhieuXuat");
            DropTable("dbo.LichSuDonHang");
            DropTable("dbo.KhachHang");
            DropTable("dbo.DonHang");
            DropTable("dbo.ChiTietDonHang");
            DropTable("dbo.BaoCao");
            DropTable("dbo.Admins");
        }
    }
}
