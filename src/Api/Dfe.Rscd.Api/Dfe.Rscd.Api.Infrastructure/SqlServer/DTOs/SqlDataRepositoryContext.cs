using System;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class SqlDataRepositoryContext : DbContext
    {
        private readonly string _connectionString;

        public SqlDataRepositoryContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlDataRepositoryContext(DbContextOptions<SqlDataRepositoryContext> options)
            : base(options)
        {
        }

        internal virtual DbSet<AmendCode> AmendCodes { get; set; }
        internal virtual DbSet<AwardingBody> AwardingBodies { get; set; }
        internal virtual DbSet<Ethnicity> Ethnicities { get; set; }
        internal virtual DbSet<Language> Languages { get; set; }
        internal virtual DbSet<InclusionAdjustmentReason> InclusionAdjustmentReasons { get; set; }
        internal virtual DbSet<Pincl> Pincls { get; set; }
        internal virtual DbSet<PotentialAnswer> PotentialAnswers { get; set; }
        internal virtual DbSet<Senstatus> Senstatuses { get; set; }
        internal virtual DbSet<YearGroup> YearGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AmendCode>(entity =>
            {
                entity.HasKey(e => e.AmendCode1);

                entity.Property(e => e.AmendCode1)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("AmendCode");

                entity.Property(e => e.AmendCodeDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AwardingBody>(entity =>
            {
                entity.Property(e => e.AwardingBodyId)
                    .ValueGeneratedNever()
                    .HasColumnName("AwardingBodyID");

                entity.Property(e => e.AwardingBodyCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AwardingBodyName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AwardingBodyNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DoesGradedExams);

                entity.Property(e => e.WasOther)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Was_Other");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.LanguageCode);

                entity.Property(e => e.LanguageCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Ethnicity>(entity =>
            {
                entity.HasKey(e => e.EthnicityCode);

                entity.Property(e => e.EthnicityCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.EthnicityDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentEthnicityCode)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InclusionAdjustmentReason>(entity =>
            {
                entity.HasKey(e => e.IncAdjReasonId);

                entity.Property(e => e.IncAdjReasonId)
                    .ValueGeneratedNever()
                    .HasColumnName("IncAdjReasonID");

                entity.Property(e => e.IncAdjReasonDescription)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CanCancel);

                entity.Property(e => e.InJuneChecking);

                entity.Property(e => e.IsInclusion);

                entity.Property(e => e.ListOrder);

                entity.Property(e => e.IsNewStudentReason);
            });

            modelBuilder.Entity<Pincl>(entity =>
            {
                entity.HasKey(e => e.PIncl1);

                entity.ToTable("PINCLs");

                entity.Property(e => e.PIncl1)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("P_INCL")
                    .IsFixedLength(true);

                entity.Property(e => e.DisplayFlag)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.PIncldescription)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("P_INCLDescription");
            });

            modelBuilder.Entity<PinclinclusionAdjustment>(entity =>
            {
                entity.HasKey(e => new { e.PIncl, e.IncAdjReasonId });

                entity.ToTable("PINCLInclusionAdjustments");

                entity.Property(e => e.PIncl)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("P_INCL")
                    .IsFixedLength(true);

                entity.Property(e => e.IncAdjReasonId).HasColumnName("IncAdjReasonID");

                entity.HasOne(d => d.IncAdjReason)
                    .WithMany(p => p.PinclinclusionAdjustments)
                    .HasForeignKey(d => d.IncAdjReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PINCLNORAdjustments_NORAdjustmentReasons");

                entity.HasOne(d => d.PInclNavigation)
                    .WithMany(p => p.PinclinclusionAdjustments)
                    .HasForeignKey(d => d.PIncl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PINCLNORAdjustments_PINCLs");
            });

            modelBuilder.Entity<PotentialAnswer>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.QuestionId);
                entity.Property(e => e.AnswerValue);
                entity.Property(e => e.Rejected);
            });

            modelBuilder.Entity<Senstatus>(entity =>
            {
                entity.HasKey(e => e.SenstatusCode);

                entity.ToTable("SENStatus");

                entity.Property(e => e.SenstatusCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SENStatusCode")
                    .IsFixedLength(true);

                entity.Property(e => e.SenstatusDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SENStatusDescription");
            });

            modelBuilder.Entity<YearGroup>(entity =>
            {
                entity.HasKey(e => e.YearGroupCode);

                entity.Property(e => e.YearGroupCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.YearGroupDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
