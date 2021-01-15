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

        public virtual DbSet<AmendCode> AmendCodes { get; set; }
        public virtual DbSet<AwardingBody> AwardingBodies { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<InclusionAdjustmentReason> InclusionAdjustmentReasons { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Pincl> Pincls { get; set; }
        public virtual DbSet<PinclinclusionAdjDatum> PinclinclusionAdjData { get; set; }
        public virtual DbSet<PinclinclusionAdjustment> PinclinclusionAdjustments { get; set; }
        public virtual DbSet<Prompt> Prompts { get; set; }
        public virtual DbSet<PromptResponse> PromptResponses { get; set; }
        public virtual DbSet<PromptType> PromptTypes { get; set; }
        public virtual DbSet<Senstatus> Senstatuses { get; set; }
        public virtual DbSet<YearGroup> YearGroups { get; set; }

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

                entity.Property(e => e.WasOther)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Was_Other");
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

            modelBuilder.Entity<PinclinclusionAdjDatum>(entity =>
            {
                entity.HasKey(e => new { e.PinclinclusionAdjustmentsPIncl, e.PinclinclusionAdjustmentsIncAdjReasonId, e.PromptsPromptId });

                entity.ToTable("PINCLInclusionAdjData");

                entity.Property(e => e.PinclinclusionAdjustmentsPIncl)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("PINCLInclusionAdjustments_P_INCL")
                    .IsFixedLength(true);

                entity.Property(e => e.PinclinclusionAdjustmentsIncAdjReasonId).HasColumnName("PINCLInclusionAdjustments_IncAdjReasonID");

                entity.Property(e => e.PromptsPromptId).HasColumnName("Prompts_PromptID");

                entity.HasOne(d => d.PromptsPrompt)
                    .WithMany(p => p.PinclinclusionAdjData)
                    .HasForeignKey(d => d.PromptsPromptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PINCLInclusionAdjData_Prompts");

                entity.HasOne(d => d.PinclinclusionAdjustments)
                    .WithMany(p => p.PinclinclusionAdjData)
                    .HasForeignKey(d => new { d.PinclinclusionAdjustmentsPIncl, d.PinclinclusionAdjustmentsIncAdjReasonId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PINCLInclusionAdjData_PINCLInclusionAdjustments");
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

            modelBuilder.Entity<Prompt>(entity =>
            {
                entity.Property(e => e.PromptId)
                    .ValueGeneratedNever()
                    .HasColumnName("PromptID");

                entity.Property(e => e.ColumnName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PromptShortText)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PromptText)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PromptTypesPromptTypeId).HasColumnName("PromptTypes_PromptTypeID");

                entity.HasOne(d => d.PromptTypesPromptType)
                    .WithMany(p => p.Prompts)
                    .HasForeignKey(d => d.PromptTypesPromptTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Prompts_PromptTypes");
            });

            modelBuilder.Entity<PromptResponse>(entity =>
            {
                entity.HasKey(e => new { e.PromptId, e.ListOrder });

                entity.Property(e => e.PromptId).HasColumnName("PromptID");

                entity.Property(e => e.ListValue)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.HasOne(d => d.Prompt)
                    .WithMany(p => p.PromptResponses)
                    .HasForeignKey(d => d.PromptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromptResponses_Prompts");
            });

            modelBuilder.Entity<PromptType>(entity =>
            {
                entity.Property(e => e.PromptTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("PromptTypeID");

                entity.Property(e => e.PromptTypeName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
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
