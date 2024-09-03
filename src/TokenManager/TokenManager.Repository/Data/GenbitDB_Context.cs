using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using TokenManager.Repository.Models;

namespace TokenManager.Repository.Data
{
    public partial class GenbitDB_Context : DbContext
    {
        public GenbitDB_Context()
        {
        }

        public GenbitDB_Context(DbContextOptions<GenbitDB_Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CodigoDescricao> CodigoDescricao { get; set; }
        public virtual DbSet<Bitcoin> Bitcoin { get; set; }
        public virtual DbSet<Carteira> Carteira { get; set; }
        public virtual DbSet<Configuracao> Configuracao { get; set; }
        public virtual DbSet<Conta> Conta { get; set; }

        public virtual DbSet<Moeda> Moeda { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=10.0.0.4;Database=GenbitDB;User Id=genbitprod;Password=_@g3nb!t##19K");
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=191.235.92.208;Database=GenbitDB;User Id=adm.gerson;Password=v6E.\^w)`[a:Ydeq9jL(e&");
                //optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=KOMXOMPAX;Database=GenbitDB;Trusted_Connection=True;");
                //optionsBuilder.UseLoggerFactory(new LoggerFactory().AddConsole((category, level) => level == LogLevel.Information && category == DbLoggerCategory.Database.Command.Name, true));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bitcoin>(entity =>
            {
                entity.HasIndex(e => e.Address)
                    .HasName("Unic_Address")
                    .IsUnique();

                entity.HasIndex(e => e.ContaId)
                    .HasName("IX_ContaId");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Callback)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cotacao).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DataGeracao).HasColumnType("datetime");

                entity.Property(e => e.DataVinculoConta).HasColumnType("datetime");

                entity.Property(e => e.Json)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TotalReceived)
                    .HasColumnName("Total_Received")
                    .HasColumnType("decimal(20, 8)");

                entity.HasOne(d => d.Conta)
                    .WithMany(p => p.Bitcoins)
                    .HasForeignKey(d => d.ContaId)
                    .HasConstraintName("FK_dbo.Bitcoin_dbo.Conta_ContaId");
            });

            modelBuilder.Entity<Carteira>(entity =>
            {
                entity.HasIndex(e => e.ContaId)
                    .HasName("IX_ContaId");

                entity.HasIndex(e => e.MoedaId)
                    .HasName("IX_MoedaId");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataConfirmacao).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Conta)
                    .WithMany(p => p.Carteiras)
                    .HasForeignKey(d => d.ContaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Carteira_dbo.Conta_ContaId");

                entity.HasOne(d => d.Moeda)
                    .WithMany(p => p.Carteira)
                    .HasForeignKey(d => d.MoedaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Carteira_dbo.Moeda_MoedaId");
            });
                       
            modelBuilder.Entity<Configuracao>(entity =>
            {
                entity.Property(e => e.ComissaoCompraBtcAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ComissaoVendaBtcAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.IndiceCotacao).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IndiceCotacaoAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LimiteCompraAutomaticaBtc)
                    .HasColumnName("LimiteCompraAutomaticaBTC")
                    .HasColumnType("decimal(20, 8)");

                entity.Property(e => e.TarifaSaqueBrl)
                    .HasColumnName("TarifaSaqueBRL")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TarifaSaqueBrlnaoConveniado)
                    .HasColumnName("TarifaSaqueBRLNaoConveniado")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TarifaSaqueMoedaFisicaAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaCotacaoCripto).HasColumnType("decimal(20, 8)");

                entity.Property(e => e.TaxaDepositoBrl)
                    .HasColumnName("TaxaDepositoBRL")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaDepositoBrlnaoConveniado)
                    .HasColumnName("TaxaDepositoBRLNaoConveniado")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaDepositoBtc)
                    .HasColumnName("TaxaDepositoBTC")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaDepositoBtcAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaOa)
                    .HasColumnName("TaxaOA")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaOp)
                    .HasColumnName("TaxaOP")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaPagamento).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaSaqueBrl)
                    .HasColumnName("TaxaSaqueBRL")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaSaqueBrlnaoConveniado)
                    .HasColumnName("TaxaSaqueBRLNaoConveniado")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaSaqueBtc)
                    .HasColumnName("TaxaSaqueBTC")
                    .HasColumnType("decimal(20, 4)");

                entity.Property(e => e.TaxaSaqueBtcAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaSaqueMoedaFisicaAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaTransferenciaExterna).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaTransferenciaExternaAtm).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxaTransferenciaInterna).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TigerMoney).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorTransferenciaAutomaticaBtc)
                    .HasColumnName("ValorTransferenciaAutomaticaBTC")
                    .HasColumnType("decimal(20, 8)");
            });

            modelBuilder.Entity<Conta>(entity =>
            {
                //entity.HasIndex(e => e.Documento)
                //    .HasName("UQ_Documento")
                //    .IsUnique()
                //    .HasFilter("([Documento] IS NOT NULL)");

                //entity.HasIndex(e => e.Email)
                //    .HasName("IX_EmailUnico")
                //    .IsUnique();

                //entity.HasIndex(e => e.GrupoId)
                //    .HasName("IX_GrupoId");

                //entity.Property(e => e.Apelido)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                //entity.Property(e => e.Culture)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                //entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                //entity.Property(e => e.DataConfirmacaoTelefone).HasColumnType("datetime");

                //entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                //entity.Property(e => e.Documento)
                //    .HasMaxLength(255)
                //    .IsUnicode(false);

                //entity.Property(e => e.DocumentoRg)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                //entity.Property(e => e.Email)
                //    .IsRequired()
                //    .HasMaxLength(255)
                //    .IsUnicode(false);

                //entity.Property(e => e.EmailSecundario)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                //entity.Property(e => e.MotivoDesativado)
                //    .HasMaxLength(255)
                //    //.IsUnicode(false);

                //entity.Property(e => e.Nome)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                //entity.Property(e => e.NovoTelefone)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                //entity.Property(e => e.Senha)
                //    .IsRequired()
                //    .HasMaxLength(255)
                //    .IsUnicode(false);

                //entity.Property(e => e.Telefone)
                //    .HasMaxLength(255)
                //    .IsUnicode(false);

               
            });

            modelBuilder.Entity<Moeda>(entity =>
            {
                entity.Property(e => e.CodigoAlfabetico)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Simbolo)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });
                                 
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
