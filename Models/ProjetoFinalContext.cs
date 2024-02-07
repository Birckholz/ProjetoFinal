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
        optionsBuilder.UseSqlServer(@"Server=COMPUTADORDEGUI\SQLEXPRESS;Database=ProjetoFinal;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        //  optionsBuilder.UseSqlServer(@"Server=BSRO\SQLEXPRESS;Database=ProjetoFinal;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
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
            .OnDelete(DeleteBehavior.NoAction)//mudança aqui também, pois o funcionário deve ter cargo
            .HasConstraintName("FkIdCargo");

        modelBuilder.Entity<Departamento>()
            .HasOne(d => d.fkResponsavelDepartamento)
            .WithMany()
            .HasForeignKey(d => d.idResponsavel)
            //recomendação do erro: A introdução da restrição FOREIGN KEY 'FkIdResponsavel' na tabela 'departamentos' pode causar ciclos ou vários caminhos em cascata. Especifique ON DELETE NO ACTION ou ON UPDATE NO ACTION, ou modifique outras restrições FOREIGN KEY.
            //Não foi possível criar a restrição ou o índice. Consulte os erros anteriores.
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FkIdResponsavel");
        //deu um erro de duplicar o idDepartamento , tirando fica normal
        modelBuilder.Entity<Projeto>()
             .HasOne(p => p.fkCodDepartamento)
             .WithMany(d => d.projetosDepartamento)
             .HasForeignKey(d => d.codDepartamento)
             .OnDelete(DeleteBehavior.NoAction)
             .HasConstraintName("FkcodDepartamento");

        base.OnModelCreating(modelBuilder);
    }


}

