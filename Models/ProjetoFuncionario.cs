namespace ProjetoFinal;

public class ProjetoFuncionario
{

    public int idProjeto { get; set; }
    public virtual Projeto fkProjeto { get; set; } = null!;

    public int idFuncionario { get; set; }
    public virtual Funcionario fkFuncionario { get; set; } = null!;




}