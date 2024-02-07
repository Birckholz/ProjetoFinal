using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{

    [ApiController]
    [Route("[controller]")]
    public class CargoController : Controller
    {

        [HttpPost("Add/{nome}/{salario}")]
        public IActionResult postCargo(string nome, float salario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    throw new ExceptionCustom("O nome não pode ser nulo ou vazio");
                }
                if (salario <= 0)
                {
                    throw new ExceptionCustom("O salário precisa ser maior que zero");
                }
                Cargo cargo = new Cargo()
                {
                    nomeCargo = nome,
                    salarioBase = salario
                };
                using (var _context = new ProjetoFinalContext())
                {
                    _context.cargos.Add(cargo);
                    _context.SaveChanges();
                    return new ObjectResult(cargo);
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult getCargos()
        {
            try
            {
                var _context = new ProjetoFinalContext();
                DbSet<Cargo> cargos = _context.cargos;
                if (!cargos.Any())
                {
                    throw new ExceptionCustom("Não há nenhum cargo cadastrado");
                }
                return Ok(cargos);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetById/{idCargo}")]
        public IActionResult getCargo(int idCargo)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == idCargo);
                    if (item == null)
                    {
                        throw new ExceptionCustom("Não foi possivel encontrar o cargo.");
                    }
                    return new ObjectResult(item);
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("Delete/{idCargo}")]
        public IActionResult deleteCargo(int idCargo)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var cargoNulo = _context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");//se quiser tirar um cargo, não quero excluir funcionarios com esse cargo, então caso um cargo deixe de existir,
                    //quero que os funcionarios fiquem com um cargo nulo, depois pode-se modificar para colocar o novo cargo ao qual vão pertencer
                    if (cargoNulo == null)
                    {//se não existe ou foi excluido
                        postCargo("Cargo nao definido", 0);
                        cargoNulo = _context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");//procura novamente
                    }
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == idCargo);
                    if (item == null)
                    {
                        throw new ExceptionCustom("Não foi possivel encontrar o cargo.");
                    }
                    foreach (Funcionario funcionario in _context.funcionarios)
                    {
                        if (cargoNulo != null)
                        {
                            if (funcionario.idCargo == idCargo)
                            {
                                funcionario.idCargo = cargoNulo.codCargo;
                            }
                        }
                    }
                    _context.cargos.Remove(item);
                    _context.SaveChanges();
                    return Ok("Cargo removido com sucesso.");
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{idCargo}")]
        public IActionResult putCargo(int idCargo, string? nome, float? salario)
        {
            try
            {
                var _context = new ProjetoFinalContext();
                var cargo = _context.cargos.FirstOrDefault(y => y.codCargo == idCargo);
                if (cargo == null)
                {
                    throw new ExceptionCustom("Não foi possivel encontrar o cargo.");
                }
                if (nome != null)
                {
                    if (!string.IsNullOrWhiteSpace(nome))
                    {
                        cargo.nomeCargo = nome;
                    }
                    else
                    {
                        throw new ExceptionCustom("O nome não pode ser vazio");
                    }
                }
                if (salario != null)
                {
                    if (salario > 0)
                    {
                        float salario1 = Convert.ToSingle(salario);
                        cargo.salarioBase = salario1;
                    }
                    else
                    {
                        throw new ExceptionCustom("O salário precisa ser maior que zero");
                    }
                }
                _context.SaveChanges();
                return new ObjectResult(cargo);
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CargoController");
                return BadRequest(e.Message);
            }
        }


    }

}