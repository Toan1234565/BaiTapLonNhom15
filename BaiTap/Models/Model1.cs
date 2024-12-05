using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BaiTap.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model12")
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<BaoCao> BaoCao { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual DbSet<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual DbSet<ChiTietSanPham> ChiTietSanPham { get; set; }
        public virtual DbSet<ChiTietKhachHang> ChiTietKhachHang { get; set; }
        public virtual DbSet<DanhMuc> DanhMuc { get; set; }
        public virtual DbSet<DonHang> DonHang { get; set; }
        public virtual DbSet<Hang> Hang { get; set; }
        public virtual DbSet<KhachHang> KhachHang { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMai { get; set; }
        public virtual DbSet<LichSuDonHang> LichSuDonHang { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhap { get; set; }
        public virtual DbSet<PhieuXuat> PhieuXuat { get; set; }
        public virtual DbSet<SanPham> SanPham { get; set; }
        public virtual DbSet<SanPhamKhuyenMai> SanPhamKhuyenMai { get; set; }
        public virtual DbSet<Sosanh> Sosanh { get; set; }
        public virtual DbSet<TaiKhoanKH> TaiKhoanKH { get; set; }
        public virtual DbSet<TonKho> TonKho { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<Admins>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Admins>()
                .Property(e => e.full_name)
                .IsUnicode(false);

            modelBuilder.Entity<Admins>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Admins>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<Admins>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<KhuyenMai>()
                .Property(e => e.TenKhuyenMai)
                .IsUnicode(false);

            modelBuilder.Entity<KhuyenMai>()
                .Property(e => e.Mota)
                .IsUnicode(false);

            modelBuilder.Entity<KhuyenMai>()
                .Property(e => e.LoaiKM)
                .IsUnicode(false);

            modelBuilder.Entity<KhuyenMai>()
                .Property(e => e.DieuKien)
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.Sosanh)
                .WithOptional(e => e.SanPham)
                .HasForeignKey(e => e.SPID1);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.Sosanh1)
                .WithOptional(e => e.SanPham1)
                .HasForeignKey(e => e.SPID2);
        }
    }
}
