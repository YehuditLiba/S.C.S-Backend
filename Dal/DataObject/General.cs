using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.DataObject;

public partial class General : DbContext
{
    public General()
    {
    }

    public General(DbContextOptions<General> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Car { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Neighborhood> Neighborhoods { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<StationToCar> StationToCars { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Rentals> Rentals { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer(ServiceControllerExtensions.GetConnectionString("General"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3214EC07E3AAC4F1");

            entity.Property(e => e.Name)
                .HasMaxLength(1)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            //Add Parse Type Status
            entity.Property(e => e.Status)
                .HasConversion
                (
                v => v.ToString(),
                 //   v => (CarStatus)Enum.Parse(typeof(CarStatus), v)
                 v => Enum.Parse<CarStatus>(v)
          );
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC070CEA71B0");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Neighborhood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Neighbor__3214EC078DB110FA");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.City).WithMany(p => p.Neighborhoods)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Neighborh__CityI__756D6ECB");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stations__3214EC076CF53474");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Street).WithMany(p => p.Stations)
                .HasForeignKey(d => d.StreetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stations__Street__68D28DBC");
        });

        modelBuilder.Entity<StationToCar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StationT__3214EC0794A39FF7");

            entity.ToTable("StationToCar");

            entity.HasIndex(e => e.CarId, "UQ__StationT__68A0342FF13A454D").IsUnique();

            entity.HasOne(d => d.Car).WithOne(p => p.StationToCar)
                .HasForeignKey<StationToCar>(d => d.CarId)
                .HasConstraintName("FK__StationTo__CarId__7CD98669");

            entity.HasOne(d => d.Station).WithMany(p => p.StationToCars)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StationTo__Stati__7BE56230");
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Streets__3214EC075BF33E44");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Neigborhood).WithMany(p => p.Streets)
                .HasForeignKey(d => d.NeigborhoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Streets__Neigbor__0880433F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Users__A25C5AA6947FDD6F");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(16)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });
        modelBuilder.Entity<Rentals>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rentals__3214EC07E3AAC4F1");

            entity.HasOne(d => d.Car)
                  .WithMany(p => p.Rentals) // אם Car מכילה ICollection<Rental> 
                  .HasForeignKey(d => d.CarId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Rentals__CarId__7CD98669");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Rentals) // אם User מכילה ICollection<Rental> 
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Rentals__UserId__7CD98669");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
