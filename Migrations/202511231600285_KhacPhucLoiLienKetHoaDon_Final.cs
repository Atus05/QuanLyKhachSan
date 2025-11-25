namespace QuanLyKhachSan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KhacPhucLoiLienKetHoaDon_Final : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatPhongPhongs",
                c => new
                    {
                        DatPhongId = c.Int(nullable: false),
                        PhongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DatPhongId, t.PhongId })
                .ForeignKey("dbo.DatPhongs", t => t.DatPhongId)
                .ForeignKey("dbo.Phongs", t => t.PhongId)
                .Index(t => t.DatPhongId)
                .Index(t => t.PhongId);
            
            CreateTable(
                "dbo.DatPhongs",
                c => new
                    {
                        DatPhongId = c.Int(nullable: false, identity: true),
                        NgayCheckIn = c.DateTime(nullable: false),
                        NgayCheckOut = c.DateTime(nullable: false),
                        NgayDat = c.DateTime(nullable: false),
                        TienDatCoc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrangThai = c.String(nullable: false),
                        NguoiDungId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DatPhongId)
                .ForeignKey("dbo.NguoiDungs", t => t.NguoiDungId, cascadeDelete: true)
                .Index(t => t.NguoiDungId);
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        DatPhongId = c.Int(nullable: false),
                        HoaDonId = c.Int(nullable: false),
                        TongTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PhuongThucThanhToan = c.String(),
                        NgayXuat = c.DateTime(nullable: false),
                        TrangThaiThanhToan = c.String(),
                    })
                .PrimaryKey(t => t.DatPhongId)
                .ForeignKey("dbo.DatPhongs", t => t.DatPhongId)
                .Index(t => t.DatPhongId);
            
            CreateTable(
                "dbo.NguoiDungs",
                c => new
                    {
                        NguoiDungId = c.Int(nullable: false, identity: true),
                        TenDangNhap = c.String(nullable: false),
                        MatKhau = c.String(nullable: false),
                        HoTen = c.String(nullable: false),
                        Email = c.String(),
                        VaiTro = c.String(),
                    })
                .PrimaryKey(t => t.NguoiDungId);
            
            CreateTable(
                "dbo.Phongs",
                c => new
                    {
                        PhongId = c.Int(nullable: false, identity: true),
                        SoPhong = c.String(nullable: false),
                        GiaPhong = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MoTa = c.String(),
                        TinhTrang = c.String(nullable: false),
                        LoaiPhongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhongId)
                .ForeignKey("dbo.LoaiPhongs", t => t.LoaiPhongId, cascadeDelete: true)
                .Index(t => t.LoaiPhongId);
            
            CreateTable(
                "dbo.LoaiPhongs",
                c => new
                    {
                        LoaiPhongId = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(nullable: false),
                        MoTa = c.String(),
                    })
                .PrimaryKey(t => t.LoaiPhongId);
            
            CreateTable(
                "dbo.DichVus",
                c => new
                    {
                        DichVuId = c.Int(nullable: false, identity: true),
                        TenDichVu = c.String(nullable: false),
                        Gia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DichVuId);
            
            CreateTable(
                "dbo.DichVuSuDungs",
                c => new
                    {
                        DichVuSuDungId = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        DatPhongId = c.Int(nullable: false),
                        DichVuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DichVuSuDungId)
                .ForeignKey("dbo.DatPhongs", t => t.DatPhongId, cascadeDelete: true)
                .ForeignKey("dbo.DichVus", t => t.DichVuId, cascadeDelete: true)
                .Index(t => t.DatPhongId)
                .Index(t => t.DichVuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DichVuSuDungs", "DichVuId", "dbo.DichVus");
            DropForeignKey("dbo.DichVuSuDungs", "DatPhongId", "dbo.DatPhongs");
            DropForeignKey("dbo.DatPhongPhongs", "PhongId", "dbo.Phongs");
            DropForeignKey("dbo.Phongs", "LoaiPhongId", "dbo.LoaiPhongs");
            DropForeignKey("dbo.DatPhongPhongs", "DatPhongId", "dbo.DatPhongs");
            DropForeignKey("dbo.DatPhongs", "NguoiDungId", "dbo.NguoiDungs");
            DropForeignKey("dbo.HoaDons", "DatPhongId", "dbo.DatPhongs");
            DropIndex("dbo.DichVuSuDungs", new[] { "DichVuId" });
            DropIndex("dbo.DichVuSuDungs", new[] { "DatPhongId" });
            DropIndex("dbo.Phongs", new[] { "LoaiPhongId" });
            DropIndex("dbo.HoaDons", new[] { "DatPhongId" });
            DropIndex("dbo.DatPhongs", new[] { "NguoiDungId" });
            DropIndex("dbo.DatPhongPhongs", new[] { "PhongId" });
            DropIndex("dbo.DatPhongPhongs", new[] { "DatPhongId" });
            DropTable("dbo.DichVuSuDungs");
            DropTable("dbo.DichVus");
            DropTable("dbo.LoaiPhongs");
            DropTable("dbo.Phongs");
            DropTable("dbo.NguoiDungs");
            DropTable("dbo.HoaDons");
            DropTable("dbo.DatPhongs");
            DropTable("dbo.DatPhongPhongs");
        }
    }
}
