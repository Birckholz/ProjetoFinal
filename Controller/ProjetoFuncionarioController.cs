
using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class ProjetoFuncionarioController : Controller
{


    private bool projetoValido(int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        Projeto? entityCheck = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
        return entityCheck != null;
    }

    private bool funcionarioValido(int idFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityCheck = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
        return entityCheck != null;
    }

    private Funcionario? findFuncionario(int idFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityCheck = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
        return entityCheck;
    }

    private Projeto? findProjeto(int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        Projeto? entityCheck = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
        return entityCheck;
    }

    [HttpPost("Add/{idProjeto}/{idFuncionario}")]
    public IActionResult addFuncionarioProj(int idFuncionario, int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        try
        {
            if (!projetoValido(idProjeto))
            {
                throw new ExceptionCustom("Projeto não encontrado");
            }
            if (!funcionarioValido(idFuncionario))
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            ProjetoFuncionario entityAdd = new ProjetoFuncionario()
            {
                idFuncionario = idFuncionario,
                idProjeto = idProjeto
            };
            Projeto? projeto = findProjeto(idProjeto);
            Funcionario? funcionario = findFuncionario(idFuncionario);
            if (projeto != null && funcionario != null)
            {
                projeto.funcionariosProj.Add(entityAdd);
                funcionario.funcionariosProj.Add(entityAdd);
            }
            _context.funcionariosProjeto.Add(entityAdd);
            _context.SaveChanges();
            return Ok("Dados Inseridos");
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ProjetoFuncionarioController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ProjetoFuncionarioController");
            return BadRequest(t.Message);
        }

    }

    [HttpDelete("Delete/{idProjeto}/{idFuncionario}")]
    public IActionResult removerFuncProj(int idFuncionario, int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        try
        {
            if (!projetoValido(idProjeto))
            {
                throw new ExceptionCustom("Projeto não encontrado");
            }
            if (!funcionarioValido(idFuncionario))
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            ProjetoFuncionario? entityRemove = _context.funcionariosProjeto.FirstOrDefault(fj => fj.idProjeto == idProjeto && fj.idFuncionario == idFuncionario);
            if (entityRemove != null)
            {
                _context.funcionariosProjeto.Remove(entityRemove);
                _context.SaveChanges();
                return Ok("Funcionario removido do Projeto com sucesso.");
            }
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ProjetoFuncionarioController");
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ProjetoFuncionarioController");
        }
        return NotFound("Não foi possivel encontrar esse registro.");
    }
}