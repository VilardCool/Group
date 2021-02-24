using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApplication
{
    public partial class GameDBContext : DbContext
    {
        public GameDBContext()
        {
        }

        public GameDBContext(DbContextOptions<GameDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<CharacterChoose> CharacterChooses { get; set; }
        public virtual DbSet<CharacterLocation> CharacterLocations { get; set; }
        public virtual DbSet<CharacterUse> CharacterUses { get; set; }
        public virtual DbSet<Comunication> Comunications { get; set; }
        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Weapon> Weapons { get; set; }
        public virtual DbSet<WeaponType> WeaponTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= VILARD\\MSSQLSERVER01; Database=GameDB; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_CI_AS");

            modelBuilder.Entity<Character>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<CharacterChoose>(entity =>
            {
                entity.ToTable("Character_choose");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterChooses)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_choose_Characters");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.CharacterChooses)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_choose_Players");
            });

            modelBuilder.Entity<CharacterLocation>(entity =>
            {
                entity.ToTable("Character_location");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterLocations)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_location_Characters");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.CharacterLocations)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_location_Maps");
            });

            modelBuilder.Entity<CharacterUse>(entity =>
            {
                entity.ToTable("Character_use");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterUses)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_use_Characters");

                entity.HasOne(d => d.Weapon)
                    .WithMany(p => p.CharacterUses)
                    .HasForeignKey(d => d.WeaponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_use_Weapons");
            });

            modelBuilder.Entity<Comunication>(entity =>
            {
                entity.ToTable("Comunication");

                entity.HasOne(d => d.Character1)
                    .WithMany(p => p.ComunicationCharacter1s)
                    .HasForeignKey(d => d.Character1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comunication_Characters");

                entity.HasOne(d => d.Character2)
                    .WithMany(p => p.ComunicationCharacter2s)
                    .HasForeignKey(d => d.Character2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comunication_Characters1");
            });

            modelBuilder.Entity<Map>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Players_Sessions");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.Server)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sessions_Maps");
            });

            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsFixedLength(true);

                entity.Property(e => e.RateOfFire).HasColumnName("Rate_of_fire");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Weapons)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weapons_Weapon_type");
            });

            modelBuilder.Entity<WeaponType>(entity =>
            {
                entity.ToTable("Weapon_type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
