
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class FuncionarioController : Controller
{
    [HttpPost("Add/{idCargo}/{idDepartamento}/{nomeFuncionario}/{telefoneFuncionario}/{emailFuncionario}/{enderecoFuncionario}/{CPFFuncionario}/{tipoContrFuncionario}/{modoTrabFuncionario}/{formacaoRelevanteFuncionario}/{statusFuncionario}")]
    public IActionResult addFuncionario(int idCargo, int idDepartamento, string CPFFuncionario, string emailFuncionario, string enderecoFuncionario, string formacaoRelevanteFuncionario, string modoTrabFuncionario, string nomeFuncionario, string statusFuncionario, string telefoneFuncionario, string tipoContrFuncionario)
    {
        var _context = new ProjetoFinalContext();
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

    [HttpGet("GetById/{idProjeto}")]
    public IActionResult getFuncById(int idProjeto)
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
    public IActionResult removerFunc(int idProjeto)
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
    public IActionResult updateFunc(int idProjeto, string? nomeProjeto, string? descricaoProjeto, string? statusProjeto, float? valorProjeto, DateOnly? dataEntregaProjeto)
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