using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BokhandelnDB
{
    public partial class BokhandelContext : DbContext
    {
        public BokhandelContext()
        {
        }

        public BokhandelContext(DbContextOptions<BokhandelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Butiker> Butikers { get; set; }
        public virtual DbSet<Böcker> Böckers { get; set; }
        public virtual DbSet<Författare> Författares { get; set; }
        public virtual DbSet<LagerSaldo> LagerSaldos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-J3GN1M3;Database=Bokhandel;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Butiker>(entity =>
            {
                entity.HasKey(e => e.IdentityId);

                entity.ToTable("Butiker");

                entity.Property(e => e.IdentityId)
                    .ValueGeneratedNever()
                    .HasColumnName("identity-ID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Butiksnamn).HasMaxLength(50);
            });

            modelBuilder.Entity<Böcker>(entity =>
            {
                entity.HasKey(e => e.Isbn13);

                entity.ToTable("Böcker");

                entity.Property(e => e.Isbn13)
                    .HasMaxLength(13)
                    .HasColumnName("ISBN13");

                entity.Property(e => e.FörfattareId).HasColumnName("FörfattareID");

                entity.Property(e => e.Pris).HasColumnType("smallmoney");

                entity.Property(e => e.Språk).HasMaxLength(50);

                entity.Property(e => e.Titel).HasMaxLength(50);

                entity.Property(e => e.Utgivningsdatum).HasMaxLength(10);

                entity.Property(e => e.ViktGram)
                    .HasMaxLength(50)
                    .HasColumnName("Vikt gram");

                entity.HasOne(d => d.Författare)
                    .WithMany(p => p.Böckers)
                    .HasForeignKey(d => d.FörfattareId)
                    .HasConstraintName("FK_Böcker_Författare");
            });

            modelBuilder.Entity<Författare>(entity =>
            {
                entity.HasKey(f => f.Id); //här
                entity.ToTable("Författare");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Efternamn).HasMaxLength(50);

                entity.Property(e => e.Födelsedatum).HasMaxLength(10);

                entity.Property(e => e.Förnamn).HasMaxLength(50);

                entity.Property(e => e.KönId).HasColumnName("KönID");
            });

            modelBuilder.Entity<LagerSaldo>(entity =>
            {
                entity.HasKey(e => new { e.ButikId, e.Isbn });

                entity.ToTable("LagerSaldo");

                entity.Property(e => e.ButikId).HasColumnName("ButikID");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .HasColumnName("ISBN");

                entity.HasOne(d => d.Butik)
                    .WithMany(p => p.LagerSaldos)
                    .HasForeignKey(d => d.ButikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LagerSaldo_Butiker1");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.LagerSaldos)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LagerSaldo_Böcker1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
