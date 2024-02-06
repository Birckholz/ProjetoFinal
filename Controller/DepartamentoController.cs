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

        private bool funcionarioResponsavelDepartamento(int idFuncionario)
        {
            var _context = new ProjetoFinalContext();
            Departamento? entityCheck = _context.departamentos.FirstOrDefault(f => f.idResponsavel == idFuncionario);
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
                if (!funcionarioValido(responsavel) && !funcionarioResponsavelDepartamento(responsavel))
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
                    var funcRespon = _context.funcionarios.FirstOrDefault(y => y.codFuncionario == responsavel);
                    funcRespon.idDepartamento=departamento.codDepartamento;//como precisamos ter adicionado um responsavel que ja foi checado e não pode ser nulo, funcRespon não será nulo.
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
                    var DepartamentoNulo = _context.departamentos.FirstOrDefault(x => x.nomeDepartamento == "Departamento nao definido");//se quiser tirar um departamento, não quero ter que excluir funcionários e refazer cadastro                 
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == idDepartamento);
                    if (item == null)
                    {
                        throw new ExceptionCustom("Não foi possivel encontrar o departamento.");
                    }
                    //se formos excluir
                    if(DepartamentoNulo ==null){
                        Funcionario? funcNulo= _context.funcionarios.FirstOrDefault(x => x.nomeFuncionario == "Funcionario Nulo");
                        if (funcNulo == null){//se não existe, cria o nulo
                            var cargoNulo = _context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");
                            if(cargoNulo ==null){//se não existe ou foi excluido
                                Cargo cargo = new Cargo()
                                {
                                    nomeCargo = "Cargo nao definido",
                                    salarioBase = 0
                                };
                                _context.cargos.Add(cargo);
                                _context.SaveChanges();
                                cargoNulo = _context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");//procura novamente
                            }
                            funcNulo = new Funcionario()
                            {
                                idCargo = cargoNulo.codCargo,
                                idDepartamento = null,
                                CPFFuncionario = "000",
                                emailFuncionario = "000",
                                enderecoFuncionario = "000",
                                formacaoRelevanteFuncionario = null,
                                nomeFuncionario = "Funcionario Nulo",
                                statusFuncionario = "000",
                                modoTrabFuncionario = null,
                                telefoneFuncionario = "000",
                                tipoContrFuncionario = null
                            };
                            _context.funcionarios.Add(funcNulo);
                            _context.SaveChanges();
                            funcNulo= _context.funcionarios.FirstOrDefault(x => x.nomeFuncionario == "Funcionario Nulo");
                        }
                        postDepartamento("Departamento nao definido",funcNulo.codFuncionario);
                        DepartamentoNulo = _context.departamentos.FirstOrDefault(x => x.nomeDepartamento == "Departamento nao definido");
                    }

                    foreach (Projeto projeto in _context.projetos)
                    {
                        if (projeto.codDepartamento == idDepartamento)
                        {
                            projeto.codDepartamento = DepartamentoNulo.codDepartamento;
                            projeto.descricaoProjeto+=" Departamento: "+ item.nomeDepartamento;//isso fará com que descrição do projeto tenha o nome do departamento que o fez
                        }
                    }
                    foreach (Funcionario funcionario in _context.funcionarios)
                    {
                        if (funcionario.idDepartamento == idDepartamento)
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

        [HttpPut("Update/{idDepartamento}")]//para modificações query
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
                    if (funcionarioValido(idResponsavel) && funcionarioResponsavelDepartamento(idResponsavel))
                    {
                        departamento.idResponsavel = idResponsavel;
                        var funcRespon = _context.funcionarios.FirstOrDefault(y => y.codFuncionario == responsavel);
                        funcRespon.idDepartamento=departamento.codDepartamento;
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