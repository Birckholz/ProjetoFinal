
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class ProjetoController : Controller
{
    //bia: erro , não precisa de id funcionário, tirei para testar
    // [HttpPost("Add/{codDepartamento}/{idCliente}/{nomeProjeto}/{valorProjeto}/{dataEntregaProjeto}/{idFuncionario}")]
    //Guilherme: Não sei pq tava ai eu nem coloquei na chamada do metodo
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
            _context.projetos.Add(new Projeto()
            {
                codDepartamento = codDepartamento,
                idCliente = idCliente,
                nomeProjeto = nomeProjeto,
                statusProjeto = "Iniciado",
                valorProjeto = valorProjeto,
                dataEntregaProjeto = dataEntregaProjeto
            });
        }
        catch (ExceptionCustom e)
        {
            System.Console.WriteLine(e.Message);
        }

        _context.SaveChanges();
        return Ok("Dados Inseridos");
    }
    [HttpGet("Get")]
    public IActionResult getProjetos()
    {
        var _context = new ProjetoFinalContext();
        DbSet<Projeto> retorno = _context.projetos;
        return Ok(retorno);
    }

    [HttpGet("GetById/{idProjeto}")]
    public IActionResult getProjById(int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        Projeto? entityGet = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
        if (entityGet != null)
        {
            return Ok(entityGet);
        }
        return NotFound("Projeto não encontrado.");
    }

    [HttpDelete("Delete/{idProjeto}")]
    public IActionResult removerProj(int idProjeto)
    {
        var _context = new ProjetoFinalContext();
        Projeto? entityRemove = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
        if (entityRemove != null)
        {
            _context.projetos.Remove(entityRemove);
            return Ok("Projeto removido com sucesso.");
        }
        return NotFound("Não foi possivel encontrar o projeto.");
    }

    [HttpPut("Update/{idProjeto}/")]
    public IActionResult updateProj(int idProjeto, string? nomeProjeto, string? descricaoProjeto, string? statusProjeto, float? valorProjeto, DateOnly? dataEntregaProjeto)
    {
        var _context = new ProjetoFinalContext();
        Projeto? entityUpdate = _context.projetos.FirstOrDefault(p => p.codProjeto == idProjeto);
        if (entityUpdate != null)
        {
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
            return Ok("Dados Atulizados.");
        }
        return NotFound("Não foi possivel encontrar o projeto.");
    }
}