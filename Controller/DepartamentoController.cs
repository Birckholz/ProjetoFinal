using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{

    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : Controller
    {
        private bool funcionarioValido(int idFuncionario)
        {
            var _context = new ProjetoFinalContext();
            Funcionario? entityCheck = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
            return entityCheck != null;
        }

        [HttpPost("Add{nome}/{responsavel}")]
        public IActionResult postDepartamento(string nome, int responsavel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    throw new ExceptionCustom("O nome não pode ser nulo ou vazio");
                }
                if (!funcionarioValido(responsavel))
                {
                    throw new ExceptionCustom("O responsável não é válido");
                }
                Departamento departamento = new Departamento()
                {
                    nomeDepartamento = nome,
                    idResponsavel = responsavel
                };
                using (var _context = new ProjetoFinalContext())
                {
                    _context.departamentos.Add(departamento);
                    _context.SaveChanges();
                    return new ObjectResult(departamento);
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult getDepartamentos()
        {
            try
            {
                var _context = new ProjetoFinalContext();
                DbSet<Departamento> retorno = _context.departamentos;
                return Ok(retorno);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
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
                        throw new ExceptionCustom("Não foi possivel encontrar o departamento.");
                    }
                    return new ObjectResult(item);
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
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
                        throw new ExceptionCustom("Não foi possivel encontrar o departamento.");
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
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{idDepartamento}")]
        public IActionResult putDepartamento(int idDepartamento, string? nome, int? responsavel)
        {
            try
            {
                int idResponsavel = 0;
                var _context = new ProjetoFinalContext();
                var departamento = _context.departamentos.FirstOrDefault(y => y.codDepartamento == idDepartamento);
                if (departamento == null)
                {
                    throw new ExceptionCustom("Não foi possivel encontrar o departamento.");
                }
                if (nome != null)
                {
                    if (!string.IsNullOrWhiteSpace(nome))
                    {
                        departamento.nomeDepartamento = nome;
                    }
                    else
                    {
                        throw new ExceptionCustom("O nome não pode ser  nulo ou vazio");
                    }
                }
                if (responsavel != null)
                {
                    idResponsavel = Convert.ToInt32(responsavel);
                    if (funcionarioValido(idResponsavel))
                    {
                        departamento.idResponsavel = idResponsavel;
                    }
                    else
                    {
                        return BadRequest("O responsavel não pode ser vazio");
                    }
                }
                _context.SaveChanges();
                return new ObjectResult(departamento);
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "DepartamentoController");
                return BadRequest(e.Message);
            }
        }

    }

}