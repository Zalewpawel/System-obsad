using Microsoft.EntityFrameworkCore;
using Sedziowanie.Models;
namespace Sedziowanie.Data
   
{
    public class DBObsadyContext :DbContext
    {
        public DBObsadyContext(DbContextOptions options) :
base(options)
        { }
       public DbSet<Mecz> Mecze { get; set; }
        public DbSet<Sedzia> Sedziowie { get; set; }
        public DbSet<Niedyspozycja> Niedyspozycje { get; set; }
        public DbSet<Rozgrywki> Rozgrywki { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mecz>()
                .HasOne(m => m.SedziaI)
                .WithMany(s => s.MeczeJakoSedzia1)
                .HasForeignKey(m => m.SedziaIId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mecz>()
                .HasOne(m => m.SedziaII)
                .WithMany(s => s.MeczeJakoSedzia2)
                .HasForeignKey(m => m.SedziaIIId)
                .OnDelete(DeleteBehavior.Restrict);

       
            modelBuilder.Entity<Mecz>()
                .HasOne(m => m.SedziaSekretarz)
                .WithMany(s => s.MeczeJakoSedziaSekretarz)
                .HasForeignKey(m => m.SedziaSekretarzId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Niedyspozycja>()
               .HasOne(n => n.Sedzia)
               .WithMany(s => s.Niedyspozycje)
               .HasForeignKey(n => n.SedziaId);

            modelBuilder.Entity<Mecz>()
          .HasOne(m => m.Rozgrywki) 
          .WithMany(r => r.Mecze)   
          .HasForeignKey(m => m.RozgrywkiId) 
          .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
