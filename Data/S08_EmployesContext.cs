using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using S08_Labo.Models;

namespace S08_Labo.Data
{
    public partial class S08_EmployesContext : DbContext
    {
        public S08_EmployesContext()
        {
        }

        public S08_EmployesContext(DbContextOptions<S08_EmployesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artiste> Artistes { get; set; } = null!;
        public virtual DbSet<Employe> Employes { get; set; } = null!;
        public virtual DbSet<VwListeArtiste> VwListeArtistes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=BDEmployee");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artiste>(entity =>
            {
                entity.HasOne(d => d.Employe)
                    .WithMany(p => p.Artistes)
                    .HasForeignKey(d => d.EmployeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Artiste_EmployeID");
            });

            modelBuilder.Entity<Employe>(entity =>
            {
                entity.Property(e => e.NoTel).IsFixedLength();
            });

            modelBuilder.Entity<VwListeArtiste>(entity =>
            {
                entity.ToView("VW_ListeArtistes", "Employes");

                entity.Property(e => e.NoTel).IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
