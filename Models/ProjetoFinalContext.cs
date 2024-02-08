using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal;

public class ProjetoFinalContext : DbContext
{
    public DbSet<Cliente> clientes { get; set; } = null!;
    public DbSet<Funcionario> funcionarios { get; set; } = null!;
    public DbSet<ContaBancaria> contasBancarias { get; set; } = null!;
    public DbSet<Departamento> departamentos { get; set; } = null!;
    public DbSet<Cargo> cargos { get; set; } = null!;
    public DbSet<Projeto> projetos { get; set; } = null!;
    public DbSet<ProjetoFuncionario> funcionariosProjeto { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Não esquecer de mudar Server= ao clonar
        optionsBuilder.UseSqlServer(@"Server=GUILHERME2943\SQLEXPRESS;Database=ProjetoFinal;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        // optionsBuilder.UseSqlServer(@"Server=BSRO\SQLEXPRESS;Database=ProjetoFinal;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjetoFuncionario>()
            .HasKey(pf => new { pf.idProjeto, pf.idFuncionario })
            .HasName("pkProjetoFuncionario");

        modelBuilder.Entity<ProjetoFuncionario>()
            .HasOne(pj => pj.fkProjeto)
            .WithMany(p => p.funcionariosProj)
            .HasForeignKey(pj => pj.idProjeto)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fkProjeto");

        modelBuilder.Entity<ProjetoFuncionario>()
            .HasOne(pj => pj.fkFuncionario)
            .WithMany(f => f.funcionariosProj)
            .HasForeignKey(pj => pj.idFuncionario)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fkFuncionario");

        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.fkCodDepartamento)
            .WithMany(d => d.funcionariosDepartamento)
            .HasForeignKey(f => f.idDepartamento)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FkIdDepartamento");

        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.fkCodCargo)
            .WithMany(c => c.funcionarioCargos)
            .HasForeignKey(f => f.idCargo)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FkIdCargo");

        modelBuilder.Entity<Departamento>()
            .HasOne(d => d.fkResponsavelDepartamento)
            .WithMany()
            .HasForeignKey(d => d.idResponsavel)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FkIdResponsavel");
        modelBuilder.Entity<Projeto>()
             .HasOne(p => p.fkCodDepartamento)
             .WithMany(d => d.projetosDepartamento)
             .HasForeignKey(d => d.codDepartamento)
             .OnDelete(DeleteBehavior.NoAction)
             .HasConstraintName("FkcodDepartamento");

        base.OnModelCreating(modelBuilder);
    }


}

