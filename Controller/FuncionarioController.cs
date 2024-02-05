
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class FuncionarioController : Controller
{

    private Departamento? findDepartamento(int idDepartamento)
    {
        var _context = new ProjetoFinalContext();
        Departamento? entityRetorno = _context.departamentos.FirstOrDefault(d => d.codDepartamento == idDepartamento);
        return entityRetorno;
    }

    private bool validaCargo(int idCargo)
    {
        var _context = new ProjetoFinalContext();
        Cargo? entityCheck = _context.cargos.FirstOrDefault(c => c.codCargo == idCargo);
        return entityCheck != null;
    }
    private bool validaDepartamento(int idDepartamento)
    {
        var _context = new ProjetoFinalContext();
        Departamento? entityCheck = _context.departamentos.FirstOrDefault(d => d.codDepartamento == idDepartamento);
        return entityCheck != null;
    }




    [HttpPost("Add/{idCargo}/{idDepartamento}/{nomeFuncionario}/{telefoneFuncionario}/{emailFuncionario}/{enderecoFuncionario}/{CPFFuncionario}/{tipoContrFuncionario}/{modoTrabFuncionario}/{formacaoRelevanteFuncionario}/{statusFuncionario}")]
    public IActionResult addFuncionario(int idCargo, int idDepartamento, string CPFFuncionario, string emailFuncionario, string enderecoFuncionario, string formacaoRelevanteFuncionario, string modoTrabFuncionario, string nomeFuncionario, string statusFuncionario, string telefoneFuncionario, string tipoContrFuncionario)
    {
        var _context = new ProjetoFinalContext();
        try
        {
            if (!validaCargo(idCargo))
            {
                throw new ExceptionCustom("Cargo não foi encontrado");
            }
            if (!validaDepartamento(idDepartamento))
            {
                throw new ExceptionCustom("Departamento não foi encontrado");
            }

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
            Departamento? departamentoFunc = findDepartamento(idDepartamento);
            Funcionario funcAdicionado = new Funcionario()
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
            };
            _context.funcionarios.Add(funcAdicionado);
            if (departamentoFunc != null)
            {
                departamentoFunc.funcionariosDepartamento.Add(funcAdicionado);
            }
            _context.SaveChanges();
            return Ok("Dados Inseridos");
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return BadRequest(t.Message);
        }

    }
    [HttpGet("Get")]
    public IActionResult getFuncionarios()
    {
        try
        {
            var _context = new ProjetoFinalContext();
            DbSet<Funcionario> retorno = _context.funcionarios;
            return Ok(retorno);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetById/{idFuncionario}")]
    public IActionResult getFuncById(int idFuncionario)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            Funcionario? entityGet = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
            if (entityGet == null)
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            return Ok(entityGet);
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return NotFound(t.Message);

        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("Delete/{idFuncionario}")]
    public IActionResult removerFunc(int idFuncionario)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            Funcionario? entityRemove = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
            if (entityRemove == null)
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            _context.funcionarios.Remove(entityRemove);
            return Ok("Funcionario removido com sucesso.");
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }
    }

    [HttpPut("Update/{idFuncionario}/")]
    public IActionResult updateFunc(int idFuncionario, string? nomeFuncionario, string? telefoneFuncionario, string? enderecoFuncionario, string? emailFuncionario, string? CPFFuncionario, string? tipoContrFuncionario, string? modoTrabFuncionario, string? formacaoRelevanteFuncionario, string? statusFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityUpdate = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);

        try
        {
            if (entityUpdate == null)
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
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
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }



    }
}
