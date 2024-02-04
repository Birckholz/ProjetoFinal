
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class FuncionarioController : Controller
{
    [HttpPost("Add/{idCargo}/{idDepartamento}/{nomeFuncionario}/{telefoneFuncionario}/{emailFuncionario}/{enderecoFuncionario}/{CPFFuncionario}/{tipoContrFuncionario}/{modoTrabFuncionario}/{formacaoRelevanteFuncionario}/{statusFuncionario}")]
    public IActionResult addFuncionario(int idCargo, int idDepartamento, string CPFFuncionario, string emailFuncionario, string enderecoFuncionario, string formacaoRelevanteFuncionario, string modoTrabFuncionario, string nomeFuncionario, string statusFuncionario, string telefoneFuncionario, string tipoContrFuncionario)
    {
        var _context = new ProjetoFinalContext();
        try
        {

            if (nomeFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Nome Invalido");
            }
            if (telefoneFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Telefone Invalido");
            }
            if (enderecoFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Endereco Invalido");
            }
            if (emailFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Email Invalido");
            }
            if (CPFFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("CPF Invalido");
            }
            if (tipoContrFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Tipo de Contrato Invalido");
            }
            if (modoTrabFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Modo de Trabalho Invalido");
            }
            if (formacaoRelevanteFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Formacao nao reconehecida");
            }
            if (statusFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Status Invalido");
            }
            _context.funcionarios.Add(new Funcionario()
            {
                idCargo = idCargo,
                idDepartamento = idDepartamento,
                CPFFuncionario = CPFFuncionario,
                emailFuncionario = emailFuncionario,
                enderecoFuncionario = enderecoFuncionario,
                formacaoRelevanteFuncionario = formacaoRelevanteFuncionario,
                nomeFuncionario = nomeFuncionario,
                statusFuncionario = statusFuncionario,
                modoTrabFuncionario = modoTrabFuncionario,
                telefoneFuncionario = telefoneFuncionario,
                tipoContrFuncionario = tipoContrFuncionario
            });
        }
        catch (ExceptionCustom e)
        {
            System.Console.WriteLine(e.Message);
        };
        _context.SaveChanges();
        return Ok("Dados Inseridos");
    }
    [HttpGet("Get")]
    public IActionResult getFuncionarios()
    {
        var _context = new ProjetoFinalContext();
        DbSet<Funcionario> retorno = _context.funcionarios;
        return Ok(retorno);
    }

    [HttpGet("GetById/{idFuncionario}")]
    public IActionResult getFuncById(int idFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityGet = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
        if (entityGet != null)
        {
            return Ok(entityGet);
        }
        return NotFound("Projeto não encontrado.");
    }

    [HttpDelete("Delete/{idFuncionario}")]
    public IActionResult removerFunc(int idFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityRemove = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
        if (entityRemove != null)
        {
            _context.funcionarios.Remove(entityRemove);
            return Ok("Projeto removido com sucesso.");
        }
        return NotFound("Não foi possivel encontrar o projeto.");
    }

    [HttpPut("Update/{idFuncionario}/")]
    public IActionResult updateFunc(int idFuncionario, string? nomeFuncionario, string? telefoneFuncionario, string? enderecoFuncionario, string? emailFuncionario, string? CPFFuncionario, string? tipoContrFuncionario, string? modoTrabFuncionario, string? formacaoRelevanteFuncionario, string? statusFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityUpdate = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
        if (entityUpdate != null)
        {
            if (!nomeFuncionario.IsNullOrEmpty())
            {
                entityUpdate.nomeFuncionario = nomeFuncionario;
            }
            if (!telefoneFuncionario.IsNullOrEmpty())
            {
                entityUpdate.telefoneFuncionario = telefoneFuncionario;
            }
            if (!enderecoFuncionario.IsNullOrEmpty())
            {
                entityUpdate.enderecoFuncionario = enderecoFuncionario;
            }
            if (!emailFuncionario.IsNullOrEmpty())
            {
                entityUpdate.emailFuncionario = emailFuncionario;
            }
            if (!CPFFuncionario.IsNullOrEmpty())
            {
                entityUpdate.CPFFuncionario = CPFFuncionario;
            }
            if (!tipoContrFuncionario.IsNullOrEmpty())
            {
                entityUpdate.tipoContrFuncionario = tipoContrFuncionario;
            }
            if (!modoTrabFuncionario.IsNullOrEmpty())
            {
                entityUpdate.modoTrabFuncionario = modoTrabFuncionario;
            }
            if (!formacaoRelevanteFuncionario.IsNullOrEmpty())
            {
                entityUpdate.formacaoRelevanteFuncionario = formacaoRelevanteFuncionario;
            }
            if (!statusFuncionario.IsNullOrEmpty())
            {
                entityUpdate.statusFuncionario = statusFuncionario;
            }

            _context.SaveChanges();
            return Ok("Dados Atulizados.");
        }
        return NotFound("Não foi possivel encontrar o Funcionario.");
    }
}