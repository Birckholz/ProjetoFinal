
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
            _context.funcionariosProjeto.Add(new ProjetoFuncionario()
            {
                idFuncionario = idFuncionario,
                idProjeto = idProjeto
            });
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

    [HttpDelete("Update/{idProjeto}/{idFuncionario}")]
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
                return Ok("Funcionario removido do Projeto com sucesso.");
            }
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ProjetoFuncionarioController");
            System.Console.WriteLine(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ProjetoFuncionarioController");
            System.Console.WriteLine(t.Message);
        }
        return NotFound("Não foi possivel encontrar esse registro.");
    }
}