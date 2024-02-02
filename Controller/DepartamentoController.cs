using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{
    //refazer para vir por link , n por body e terminar validação

    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : Controller
    {
        [HttpPost("Add")]
        public IActionResult postDepartamento([FromBody] Departamento departamento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departamento.nomeDepartamento))
                {
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(departamento.responsavelDepartamento))
                {
                    return BadRequest("O responsável não pode ser nulo ou vazio");
                }
                using (var _context = new ProjetoFinalContext())
                {
                    _context.departamentos.Add(departamento);
                    _context.SaveChanges();
                    return new ObjectResult(departamento);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult getDepartamentos()
        {
            var _context = new ProjetoFinalContext();
            DbSet<Departamento> retorno = _context.departamentos;
            return Ok(retorno);
        }

        [HttpGet("GetById/{idDepartamento}")]
        public IActionResult getDepartamento(int id)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == id);
                    if (item == null)
                    {
                        return NotFound("Não foi possivel encontrar o departamento.");
                    }
                    return new ObjectResult(item);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{idDepartamento}")]
        public IActionResult deleteDepartamento(int id)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var DepartamentoNulo = _context.departamentos.FirstOrDefault(x => x.nomeDepartamento == "Departamento nao definido");
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == id);
                    if (item == null)
                    {
                        return NotFound("Não foi possivel encontrar o departamento.");
                    }
                    foreach (Projeto projeto in _context.projetos)
                    {
                        if (projeto.codDepartamento == id && DepartamentoNulo != null)
                        {
                            projeto.codDepartamento = DepartamentoNulo.codDepartamento;
                        }
                    }
                    foreach (Funcionario funcionario in _context.funcionarios)
                    {
                        if (funcionario.idDepartamento == id && DepartamentoNulo != null)
                        {
                            funcionario.idDepartamento = DepartamentoNulo.codDepartamento;
                        }
                    }
                    _context.departamentos.Remove(item);
                    _context.SaveChanges();
                    return Ok("Departamento removido com sucesso.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{idDepartamento}")]
        public IActionResult putDepartamento(int id, [FromBody] Departamento departamento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departamento.nomeDepartamento))
                {
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(departamento.responsavelDepartamento))
                {
                    return BadRequest("O responsável não pode ser nulo ou vazio");
                }
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == id);
                    if (item == null)
                    {
                        return NotFound("Não foi possivel encontrar o departamento.");
                    }
                    _context.Entry(item).CurrentValues.SetValues(departamento);
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }

}