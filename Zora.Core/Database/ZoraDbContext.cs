using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zora.Core.Database.Models;

namespace Zora.Core.Database;

internal class ZoraDbContext(DbContextOptions<ZoraDbContext> options) : DbContext(options)
{
    public const string SchemaName = "zora";

    public DbSet<UserModel> Users => Set<UserModel>();

    public DbSet<DestinationModel> Destinations => Set<DestinationModel>();

    public DbSet<TourModel> Tours => Set<TourModel>();

    public DbSet<CheckListItemModel> UserCheckLists => Set<CheckListItemModel>();

    public DbSet<DiaryNoteModel> DiaryNotes => Set<DiaryNoteModel>();

    public DbSet<EquipmentModel> Equipments { get; set; }

    public DbSet<AttractionModel> Attractions => Set<AttractionModel>();

    public DbSet<GalleryModel> Galleries => Set<GalleryModel>();

    public DbSet<StoryModel> Stories => Set<StoryModel>();

    public DbSet<StoryImageModel> StoryImages => Set<StoryImageModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaName);

        ConfigureUser(modelBuilder.Entity<UserModel>());
        ConfigureDestination(modelBuilder.Entity<DestinationModel>());
        ConfigureTour(modelBuilder.Entity<TourModel>());
        ConfigureCheckListItem(modelBuilder.Entity<CheckListItemModel>());
        ConfigureDiaryNote(modelBuilder.Entity<DiaryNoteModel>());
        ConfigureEquipment(modelBuilder.Entity<EquipmentModel>());
        ConfigureAttraction(modelBuilder.Entity<AttractionModel>());
        ConfigureGallery(modelBuilder.Entity<GalleryModel>());
        ConfigureStory(modelBuilder.Entity<StoryModel>());
        ConfigureStoryImage(modelBuilder.Entity<StoryImageModel>());
    }

    private void ConfigureUser(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("User");
        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.PasswordHash).HasMaxLength(256);
        builder.Property(u => u.Role).IsRequired();

        builder
            .HasMany(u => u.GuideTours)
            .WithOne(t => t.Guide)
            .HasForeignKey(t => t.GuideId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(u => u.UserTours)
            .WithMany(t => t.Participants)
            .UsingEntity<Dictionary<string, object>>(
                "UserTour",
                j =>
                    j.HasOne<TourModel>()
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.NoAction),
                j =>
                    j.HasOne<UserModel>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction),
                j =>
                {
                    j.HasKey("UserId", "TourId");
                    j.ToTable("UserTour");
                }
            );
    }

    private void ConfigureDestination(EntityTypeBuilder<DestinationModel> builder)
    {
        builder.ToTable("Destination");
        builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(d => d.Name).IsUnique();
        builder.Property(d => d.Description).HasMaxLength(4000);

        builder
            .HasMany(d => d.Tours)
            .WithOne(t => t.Destination)
            .HasForeignKey(t => t.DestinationId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureTour(EntityTypeBuilder<TourModel> builder)
    {
        builder.ToTable("Tour");
        builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Name);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(500);
        builder.Property(t => t.Distance).IsRequired();
        builder.Property(t => t.Duration).IsRequired();
        builder.Property(t => t.ElevationGain).IsRequired();
        builder.Property(t => t.AvailableSpots).IsRequired();
        builder.Property(t => t.ScheduledAt).IsRequired();
    }

    private void ConfigureCheckListItem(EntityTypeBuilder<CheckListItemModel> builder)
    {
        builder.ToTable("CheckListItem");
        builder.Property(ucl => ucl.IsChecked).IsRequired();

        builder
            .HasOne(ucl => ucl.User)
            .WithMany(u => u.UserCheckList)
            .HasForeignKey(ucl => ucl.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(ucl => ucl.Tour)
            .WithMany()
            .HasForeignKey(ucl => ucl.TourId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(ucl => ucl.Equipment)
            .WithMany()
            .HasForeignKey(ucl => ucl.EquipmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private void ConfigureDiaryNote(EntityTypeBuilder<DiaryNoteModel> builder)
    {
        builder.ToTable("DiaryNote");
        builder.Property(dn => dn.Title).IsRequired().HasMaxLength(50);
        builder.Property(dn => dn.Content).IsRequired().HasMaxLength(500);
        builder.Property(dn => dn.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(dn => dn.User)
            .WithMany(u => u.DiaryNotes)
            .HasForeignKey(dn => dn.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(dn => dn.Tour)
            .WithMany(t => t.DiaryNotes)
            .HasForeignKey(dn => dn.TourId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private void ConfigureEquipment(EntityTypeBuilder<EquipmentModel> builder)
    {
        builder.ToTable("Equipment");
        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(e => e.Name).IsUnique();

        builder
            .HasMany(e => e.Tours)
            .WithMany(t => t.Equipment)
            .UsingEntity<Dictionary<string, object>>(
                "TourEquipment",
                j =>
                    j.HasOne<TourModel>()
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.NoAction),
                j =>
                    j.HasOne<EquipmentModel>()
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.NoAction),
                j =>
                {
                    j.HasKey("TourId", "EquipmentId");
                    j.ToTable("TourEquipment");
                }
            );
    }

    private void ConfigureAttraction(EntityTypeBuilder<AttractionModel> builder)
    {
        builder.ToTable("Attraction");
        builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(a => a.Name).IsUnique();

        builder
            .HasMany(a => a.Tours)
            .WithMany(t => t.Attractions)
            .UsingEntity<Dictionary<string, object>>(
                "TourAttraction",
                j =>
                    j.HasOne<TourModel>()
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.NoAction),
                j =>
                    j.HasOne<AttractionModel>()
                        .WithMany()
                        .HasForeignKey("AttractionId")
                        .OnDelete(DeleteBehavior.NoAction),
                j =>
                {
                    j.HasKey("TourId", "AttractionId");
                    j.ToTable("TourAttraction");
                }
            );
    }

    private void ConfigureGallery(EntityTypeBuilder<GalleryModel> builder)
    {
        builder.ToTable("Gallery");

        builder.Property(g => g.FileName).IsRequired().HasMaxLength(100);

        builder.Property(g => g.FilePath).IsRequired().HasMaxLength(500);

        builder
            .HasOne(g => g.Tour)
            .WithMany(t => t.GalleryImages)
            .HasForeignKey(g => g.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureStory(EntityTypeBuilder<StoryModel> builder)
    {
        builder.ToTable("Story");
        builder.Property(s => s.Content).IsRequired().HasMaxLength(4000);
        builder.Property(s => s.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.Tour)
            .WithMany()
            .HasForeignKey(s => s.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureStoryImage(EntityTypeBuilder<StoryImageModel> builder)
    {
        builder.ToTable("StoryImage");

        builder.Property(si => si.FileName).IsRequired().HasMaxLength(100);
        builder.Property(si => si.FilePath).IsRequired().HasMaxLength(500);

        builder
            .HasOne(si => si.Story)
            .WithMany(s => s.Images)
            .HasForeignKey(si => si.StoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
