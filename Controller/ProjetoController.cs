
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class ProjetoController : Controller
{
    private bool departamentoValido(int idDepartamento)
    {
        var _context = new ProjetoFinalContext();
        Departamento? entityCheck = _context.departamentos.FirstOrDefault(d => d.codDepartamento == idDepartamento);
        return entityCheck != null;
    }

    private bool clienteValido(int idCliente)
    {
        var _context = new ProjetoFinalContext();
        Cliente? entityCheck = _context.clientes.FirstOrDefault(c => c.codCliente == idCliente);
        return entityCheck != null;
    }

    private Cliente? findCliente(int idCliente)
    {
        var _context = new ProjetoFinalContext();
        Cliente? entityCheck = _context.clientes.FirstOrDefault(c => c.codCliente == idCliente);
        return entityCheck;
    }



    [HttpPost("Add/{codDepartamento}/{idCliente}/{nomeProjeto}/{valorProjeto}/{dataEntregaProjeto}")]

    public IActionResult addProjeto(int codDepartamento, int idCliente, string nomeProjeto, float valorProjeto, DateOnly dataEntregaProjeto)
    {
        var _context = new ProjetoFinalContext();

        try
        {
            if (nomeProjeto.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Nome de projeto inválido.");
            };

            if (!clienteValido(idCliente))
            {
                throw new ExceptionCustom("Cliente não existe");
            }

            if (!departamentoValido(codDepartamento))
            {
                throw new ExceptionCustom("Cliente não existe");
            }
            Cliente? cliente = findCliente(idCliente);
            Projeto entityAdd = new Projeto()
            {
                codDepartamento = codDepartamento,
                idCliente = idCliente,
                nomeProjeto = nomeProjeto,
                statusProjeto = "Iniciado",
                valorProjeto = valorProjeto,
                dataEntregaProjeto = dataEntregaProjeto
            };
            if (cliente != null)
            {
                cliente.clienteProjetos.Add(entityAdd);
            }
            _context.projetos.Add(entityAdd);
            _context.SaveChanges();
            return new ObjectResult(entityAdd);
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "ProjetoController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "ProjetoController");
            return BadRequest(t.Message);

        }


    }
    [HttpGet("Get")]
    public IActionResult getProjetos()
    {
        try
        {
            var _context = new ProjetoFinalContext();
            DbSet<Projeto> retorno = _context.projetos;
            if (!retorno.Any())
            {
                throw new ExceptionCustom("Não ha projetos cadastrados");
            }
            return Ok(retorno);
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "ProjetoController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "ProjetoController");
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetById/{idProjeto}")]
    public IActionResult getProjById(int idProjeto)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            Projeto? entityGet = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
            if (entityGet != null)
            {
                return Ok(entityGet);

            }
            throw new ExceptionCustom("Projeto não encontrado");
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "ProjetoController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "ProjetoController");
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("Delete/{idProjeto}")]
    public IActionResult removerProj(int idProjeto)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            Projeto? entityRemove = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
            if (entityRemove != null)
            {
                //vai remover as conexoes de projeto+funcionario
                /*foreach (ProjetoFuncionario pj in _context.funcionariosProjeto)
                    {
                        if (pj.idProjeto == idProjeto)
                        {
                            _context.funcionariosProjeto.Remove(pj);
                        }
                    }*/
                _context.projetos.Remove(entityRemove);
                _context.SaveChanges();
                return Ok("Projeto removido com sucesso.");
            }
            throw new ExceptionCustom("Não foi possivel encontrar o projeto");
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "ProjetoController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "ProjetoController");
            return BadRequest(e.Message);
        }



    }

    [HttpPut("Update/{idProjeto}/")]
    public IActionResult updateProj(int idProjeto, string? nomeProjeto, string? descricaoProjeto, string? statusProjeto, float? valorProjeto, DateOnly? dataEntregaProjeto)
    {
        var _context = new ProjetoFinalContext(); try
        {
            Projeto? entityUpdate = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
            if (entityUpdate == null)
            {
                throw new ExceptionCustom("Projeto não encontrado");
            }
            if (nomeProjeto != null)
            {
                entityUpdate.nomeProjeto = nomeProjeto;
            }
            if (descricaoProjeto != null)
            {
                entityUpdate.descricaoProjeto = descricaoProjeto;
            }
            if (statusProjeto != null)
            {
                entityUpdate.statusProjeto = statusProjeto;
            }
            if (valorProjeto != null)
            {
                float valorEmFloat = (float)valorProjeto;
                entityUpdate.valorProjeto = valorEmFloat;
            }
            if (dataEntregaProjeto != null)
            {
                DateOnly dataNaoNula = dataEntregaProjeto ?? DateOnly.MinValue;
                entityUpdate.dataEntregaProjeto = dataNaoNula;
            }

            _context.SaveChanges();
            return new ObjectResult(entityUpdate);

        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "ProjetoController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "ProjetoController");
            return BadRequest(e.Message);
        }
    }
}