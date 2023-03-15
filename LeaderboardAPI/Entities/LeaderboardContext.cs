using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LeaderboardAPI.Entities
{
    public partial class LeaderboardContext : DbContext
    {
        public LeaderboardContext()
        {
        }

        public LeaderboardContext(DbContextOptions<LeaderboardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FileUpload> FileUploads { get; set; }
        public virtual DbSet<Leaderboard> Leaderboards { get; set; }
        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<WholesellerMapping> WholesellerMappings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;port=3306;user=root;database=leaderboardhms", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.27-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeCode)
                    .HasName("PRIMARY");

                entity.ToTable("employees");

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(50)
                    .HasColumnName("employee_code");

                entity.Property(e => e.ChangeToken)
                    .HasMaxLength(256)
                    .HasColumnName("change_token");

                entity.Property(e => e.EmployeeArea)
                    .HasMaxLength(256)
                    .HasColumnName("employee_area");

                entity.Property(e => e.EmployeeEmail)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("employee_email");

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("employee_name");

                entity.Property(e => e.EmployeePassword)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("employee_password");

                entity.Property(e => e.EmployeePhone)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("employee_phone");

                entity.Property(e => e.FirstLogin)
                    .HasColumnType("bit(1)")
                    .HasColumnName("first_login")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.PasswordIssued)
                    .HasColumnType("bit(1)")
                    .HasColumnName("password_issued")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.RoleType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("role_type");

                entity.Property(e => e.TokenExpired).HasColumnType("datetime");
            });

            modelBuilder.Entity<FileUpload>(entity =>
            {
                entity.ToTable("file_upload");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.FinishUpload).HasColumnType("datetime");

                entity.Property(e => e.NameFile)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UploadDateTime).HasColumnType("datetime");

                entity.Property(e => e.UserUpload)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Leaderboard>(entity =>
            {
                entity.HasKey(e => new { e.WholesellerCode, e.RoundId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("leaderboards");

                entity.Property(e => e.WholesellerCode)
                    .HasMaxLength(50)
                    .HasColumnName("wholeseller_code");

                entity.Property(e => e.RoundId)
                    .HasColumnType("int(11)")
                    .HasColumnName("round_id");

                entity.Property(e => e.BaselineStock)
                    .HasColumnType("int(11)")
                    .HasColumnName("baseline_stock");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("group_name");

                entity.Property(e => e.PointA)
                    .HasColumnType("int(11)")
                    .HasColumnName("Point_A");

                entity.Property(e => e.PointB)
                    .HasColumnType("int(11)")
                    .HasColumnName("Point_B");

                entity.Property(e => e.Rank)
                    .HasColumnType("int(11)")
                    .HasColumnName("rank");

                entity.Property(e => e.SaleDate)
                    .HasColumnType("datetime")
                    .HasColumnName("sale_date")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.SalePoint)
                    .HasColumnType("int(11)")
                    .HasColumnName("sale_point");
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.ToTable("rounds");

                entity.Property(e => e.RoundId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("round_id");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.RoundName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("round_name");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");
            });

            modelBuilder.Entity<WholesellerMapping>(entity =>
            {
                entity.HasKey(e => e.WholesellerCode)
                    .HasName("PRIMARY");

                entity.ToTable("wholeseller_mapping");

                entity.Property(e => e.WholesellerCode)
                    .HasMaxLength(50)
                    .HasColumnName("wholeseller_code");

                entity.Property(e => e.SalesmanCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("salesman_code");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
