namespace QuanLyKhachSan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenMoi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DatPhongPhongs", "DatPhongId", "dbo.DatPhongs");
            DropForeignKey("dbo.DatPhongPhongs", "PhongId", "dbo.Phongs");
            CreateTable(
                "dbo.VaiTroes",
                c => new
                    {
                        VaiTroID = c.Int(nullable: false, identity: true),
                        TenVaiTro = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.VaiTroID);
            
            AddColumn("dbo.NguoiDungs", "MatKhauMaHoa", c => c.String(nullable: false));
            AddColumn("dbo.NguoiDungs", "SoDienThoai", c => c.String(nullable: false));
            AddColumn("dbo.NguoiDungs", "GioiTinh", c => c.String());
            AddColumn("dbo.NguoiDungs", "NgaySinh", c => c.DateTime());
            AddColumn("dbo.NguoiDungs", "Avatar", c => c.String());
            AddColumn("dbo.NguoiDungs", "TrangThai", c => c.Boolean(nullable: false));
            AddColumn("dbo.NguoiDungs", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.NguoiDungs", "VaiTroID", c => c.Int(nullable: false));
            AlterColumn("dbo.NguoiDungs", "TenDangNhap", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.NguoiDungs", "Email", c => c.String(nullable: false));
            CreateIndex("dbo.NguoiDungs", "VaiTroID");
            AddForeignKey("dbo.NguoiDungs", "VaiTroID", "dbo.VaiTroes", "VaiTroID", cascadeDelete: true);
            AddForeignKey("dbo.DatPhongPhongs", "DatPhongId", "dbo.DatPhongs", "DatPhongId", cascadeDelete: true);
            AddForeignKey("dbo.DatPhongPhongs", "PhongId", "dbo.Phongs", "PhongId", cascadeDelete: true);
            DropColumn("dbo.NguoiDungs", "MatKhau");
            DropColumn("dbo.NguoiDungs", "VaiTro");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NguoiDungs", "VaiTro", c => c.String());
            AddColumn("dbo.NguoiDungs", "MatKhau", c => c.String(nullable: false));
            DropForeignKey("dbo.DatPhongPhongs", "PhongId", "dbo.Phongs");
            DropForeignKey("dbo.DatPhongPhongs", "DatPhongId", "dbo.DatPhongs");
            DropForeignKey("dbo.NguoiDungs", "VaiTroID", "dbo.VaiTroes");
            DropIndex("dbo.NguoiDungs", new[] { "VaiTroID" });
            AlterColumn("dbo.NguoiDungs", "Email", c => c.String());
            AlterColumn("dbo.NguoiDungs", "TenDangNhap", c => c.String(nullable: false));
            DropColumn("dbo.NguoiDungs", "VaiTroID");
            DropColumn("dbo.NguoiDungs", "NgayTao");
            DropColumn("dbo.NguoiDungs", "TrangThai");
            DropColumn("dbo.NguoiDungs", "Avatar");
            DropColumn("dbo.NguoiDungs", "NgaySinh");
            DropColumn("dbo.NguoiDungs", "GioiTinh");
            DropColumn("dbo.NguoiDungs", "SoDienThoai");
            DropColumn("dbo.NguoiDungs", "MatKhauMaHoa");
            DropTable("dbo.VaiTroes");
            AddForeignKey("dbo.DatPhongPhongs", "PhongId", "dbo.Phongs", "PhongId");
            AddForeignKey("dbo.DatPhongPhongs", "DatPhongId", "dbo.DatPhongs", "DatPhongId");
        }
    }
}
