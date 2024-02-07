
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class ContaBancariaController : Controller
{
    private bool funcionarioValido(int idFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityCheck = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
        return entityCheck != null;
    }

    [HttpPost("Add/{codFuncionario}/{agenciaContaB}/{numeroContaB}/{tipoContaB}")]
    public IActionResult addContaBancaria(int codFuncionario, string agenciaContaB, string numeroContaB, string tipoContaB)
    {
        var _context = new ProjetoFinalContext();
        try
        {
            if (!funcionarioValido(codFuncionario))
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            if (agenciaContaB.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Conta bancaria invalida");
            }
            if (numeroContaB.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Numero de Conta invalida");
            }
            if (tipoContaB.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Tipo de Conta bancaria invalida");
            }

            ContaBancaria conta= new ContaBancaria(){
                codFuncionario = codFuncionario,
                agenciaContaB = agenciaContaB,
                numeroContaB = numeroContaB,
                tipoContaB = tipoContaB  
            };
            _context.contasBancarias.Add(conta);
            _context.SaveChanges();
            return new ObjectResult(conta);
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ContaBancariaController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ContaBancariaController");
            return BadRequest(t.Message);
        }

    }

    [HttpGet("Get")]
    public IActionResult getContasBancarias()
    {
        try
        {
            var _context = new ProjetoFinalContext();
            DbSet<ContaBancaria> retorno = _context.contasBancarias;
            if (!retorno.Any())
            {
                throw new ExceptionCustom("Não há nenhuma conta cadastrada");
            }
            return Ok(retorno);
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ContaBancariaController");
            return BadRequest(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ContaBancariaController");
            return BadRequest(t.Message);
        }
    }

    [HttpGet("GetById/{codContaB}")]
    public IActionResult getContaBancById(int codContaB)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            ContaBancaria? entityGet = _context.contasBancarias.FirstOrDefault(bc => bc.codContaB == codContaB);
            if (entityGet == null)
            {
                throw new ExceptionCustom("Conta Bancaria não encontrado");
            }
            return Ok(entityGet);
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ContaBancariaController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ContaBancariaController");
            return BadRequest(t.Message);
        }
    }

    [HttpDelete("Delete/{codContaB}")]
    public IActionResult removerContaBanc(int codContaB)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            ContaBancaria? entityRemove = _context.contasBancarias.FirstOrDefault(bc => bc.codContaB == codContaB);
            if (entityRemove == null)
            {
                throw new ExceptionCustom("Conta Bancaria não encontrada");
            }
            _context.contasBancarias.Remove(entityRemove);
            _context.SaveChanges();
            return Ok("Conta Bancaria removida com sucesso.");
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ContaBancariaController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ContaBancariaController");
            return NotFound(t.Message);
        }
    }
    
    [HttpPut("Update/{codFuncionario}")]
    public IActionResult updateContaBanc(int codFuncionario, string? agenciaContaB, string? numeroContaB, string? tipoContaB)
    {
        var _context = new ProjetoFinalContext();
        ContaBancaria? entityUpdate = _context.contasBancarias.FirstOrDefault(cb => cb.codFuncionario == codFuncionario);
        try
        {
            if (entityUpdate == null)
            {
                throw new ExceptionCustom("Conta bancaria não encontrada");
            }
            if (agenciaContaB != null)
            {
                entityUpdate.agenciaContaB = agenciaContaB;
            }
            if (numeroContaB != null)
            {
                entityUpdate.numeroContaB = numeroContaB;
            }
            if (tipoContaB != null)
            {
                entityUpdate.tipoContaB = tipoContaB;
            }
            _context.SaveChanges();
            return new ObjectResult(entityUpdate);
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ContaBancariaController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ContaBancariaController");
            return BadRequest(t.Message);
        }

    }
}