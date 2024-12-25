using BaiTap.Models;
using System.Data.Entity;

public class BaiTapDBContext : DbContext
{
    public BaiTapDBContext() : base("name=DefaultConnection")
    {
    }

    public DbSet<KhuyenMai> KhuyenMais { get; set; }
    public DbSet<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; }

}
