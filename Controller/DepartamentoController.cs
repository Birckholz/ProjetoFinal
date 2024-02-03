using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{

    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : Controller
    {
        [HttpPost("Add{nome}/{responsavel}")]
        public IActionResult postDepartamento(string nome,string responsavel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(responsavel))
                {
                    return BadRequest("O responsável não pode ser nulo ou vazio");
                }
                Departamento departamento=new Departamento(){
                    nomeDepartamento=nome,
                    responsavelDepartamento=responsavel
                };
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
        public IActionResult getDepartamento(int idDepartamento)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == idDepartamento);
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
        public IActionResult deleteDepartamento(int idDepartamento)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var DepartamentoNulo = _context.departamentos.FirstOrDefault(x => x.nomeDepartamento == "Departamento nao definido");
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == idDepartamento);
                    if (item == null)
                    {
                        return NotFound("Não foi possivel encontrar o departamento.");
                    }
                    foreach (Projeto projeto in _context.projetos)
                    {
                        if (projeto.codDepartamento == idDepartamento && DepartamentoNulo != null)
                        {
                            projeto.codDepartamento = DepartamentoNulo.codDepartamento;
                        }
                    }
                    foreach (Funcionario funcionario in _context.funcionarios)
                    {
                        if (funcionario.idDepartamento == idDepartamento && DepartamentoNulo != null)
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
        public IActionResult putDepartamento(int idDepartamento, string? nome, string? responsavel)
        {
            try
            {
                var _context= new ProjetoFinalContext();
                var departamento=_context.departamentos.FirstOrDefault(y => y.codDepartamento == idDepartamento);
                if (departamento == null)
                {
                    return NotFound("Não foi possivel encontrar o departamento.");
                }
                if (nome!=null)
                {
                    if(!string.IsNullOrWhiteSpace(nome)){
                        departamento.nomeDepartamento=nome;
                    }else{
                        return BadRequest("O nome não pode ser vazio");
                    }
                }
                if (responsavel!=null)
                {
                    if(!string.IsNullOrWhiteSpace(responsavel)){
                        departamento.responsavelDepartamento=responsavel;
                    }else{
                       return BadRequest("O responsavel não pode ser vazio");
                    }
                }
                _context.SaveChanges();
                return new ObjectResult(departamento);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }

}