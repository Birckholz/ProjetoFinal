
using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class ProjetoFuncionarioController : Controller
{
    [HttpPost("Add/{idProjeto}/{idFuncionario}")]
    public IActionResult addFuncionarioProj(int idFuncionario, int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        _context.funcionariosProjeto.Add(new ProjetoFuncionario()
        {
            idFuncionario = idFuncionario,
            idProjeto = idProjeto
        });
        _context.SaveChanges();
        return Ok("Dados Inseridos");
    }

    [HttpDelete("Update/{idProjeto}/{idFuncionario}")]
    public IActionResult removerFuncProj(int idFuncionario, int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        ProjetoFuncionario? entityRemove = _context.funcionariosProjeto.FirstOrDefault(fj => fj.idProjeto == idProjeto && fj.idFuncionario == idFuncionario);
        if (entityRemove != null)
        {
            _context.funcionariosProjeto.Remove(entityRemove);
            return Ok("Funcionario removido do Projeto com sucesso.");
        }
        return NotFound("Não foi possivel encontrar esse registro.");
    }
}