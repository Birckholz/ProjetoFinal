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
            .HasConstraintName("fkProjeto");

        modelBuilder.Entity<ProjetoFuncionario>()
            .HasOne(pj => pj.fkFuncionario)
            .WithMany(f => f.funcionariosProj)
            .HasForeignKey(pj => pj.idFuncionario)
            .HasConstraintName("fkFuncionario");

        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.fkCodDepartamento)
            .WithMany(d => d.funcionariosDepartamento)
            .HasForeignKey(f => f.idDepartamento)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FkIdDepartamento");

        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.fkCodCargo)
            .WithMany()
            .HasForeignKey(f => f.idCargo)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FkIdCargo");


        base.OnModelCreating(modelBuilder);
    }


}

