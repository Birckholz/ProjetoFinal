
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class ContaBancariaController : Controller
{
    [HttpPost("Add/{codFuncionario}/{agenciaContaB}/{numeroContaB}/{tipoContaB}")]
    public IActionResult addContaBancaria(int codFuncionario, string agenciaContaB, string numeroContaB, string tipoContaB)
    {
        var _context = new ProjetoFinalContext();
        _context.contasBancarias.Add(new ContaBancaria()
        {
            codFuncionario = codFuncionario,
            agenciaContaB = agenciaContaB,
            numeroContaB = numeroContaB,
            tipoContaB = tipoContaB
        });
        _context.SaveChanges();
        return Ok("Dados Inseridos");
    }

    [HttpPut("Update/{codFuncionario}")]
    public IActionResult updateContaBanc(int codFuncionario, string agenciaContaB, string numeroContaB, string tipoContaB)
    {
        var _context = new ProjetoFinalContext();
        ContaBancaria? entityUpdate = _context.contasBancarias.FirstOrDefault(cb => cb.codFuncionario == codFuncionario);
        if (entityUpdate != null)
        {
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
            return Ok("Funcionario removido do Projeto com sucesso.");
        }
        return NotFound("Não foi possivel encontrar esse registro.");
    }
    [HttpGet("Get")]
    public IActionResult getContasBancarias()
    {
        var _context = new ProjetoFinalContext();
        DbSet<ContaBancaria> retorno = _context.contasBancarias;
        return Ok(retorno);
    }

    [HttpGet("GetById/{codContaB}")]
    public IActionResult getContaBancById(int codContaB)
    {
        var _context = new ProjetoFinalContext();
        ContaBancaria? entityGet = _context.contasBancarias.FirstOrDefault(bc => bc.codContaB == codContaB);
        if (entityGet != null)
        {
            return Ok(entityGet);
        }
        return NotFound("Projeto não encontrado.");
    }

    [HttpDelete("Delete/{codContaB}")]
    public IActionResult removerContaBanc(int codContaB)
    {
        var _context = new ProjetoFinalContext();
        ContaBancaria? entityRemove = _context.contasBancarias.FirstOrDefault(bc => bc.codContaB == codContaB);
        if (entityRemove != null)
        {
            _context.contasBancarias.Remove(entityRemove);
            return Ok("Conta Bancaria removido com sucesso.");
        }
        return NotFound("Não foi possivel encontrar o projeto.");
    }
}