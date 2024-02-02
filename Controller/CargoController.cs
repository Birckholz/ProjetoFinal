using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{
        //refazer para vir por link , n por body e terminar validação

    [ApiController]
    [Route("[controller]")]
    public class CargoController : Controller
    {

        // criar direto no banco o nulo
        
        [HttpPost("Add")]
        public IActionResult postCargo([FromBody] Cargo cargo)
        {
            try{
                if (string.IsNullOrWhiteSpace(cargo.nomeCargo)){
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (cargo.salarioBase<=0){
                    return BadRequest("O salário precisa ser maior que zero");
                }
                using (var _context = new ProjetoFinalContext())
                {
                    _context.cargos.Add(cargo);
                    _context.SaveChanges();
                    return new ObjectResult(cargo);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult getCargos()
        {
            var _context = new ProjetoFinalContext();
            DbSet<Cargo> retorno = _context.cargos;
            return Ok(retorno);
        }

        [HttpGet("GetById/{idCargo}")]
        public IActionResult getCargo(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == id);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cargo."); 
                    }
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{idCargo}")]
        public IActionResult deleteCargo(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var CargoNulo=_context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == id);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cargo."); 
                    }
                    foreach (Funcionario funcionario in _context.funcionarios){
                        if (funcionario.idCargo==id && CargoNulo!=null){
                            funcionario.idCargo=CargoNulo.codCargo;
                        }
                    }
                    _context.cargos.Remove(item);
                    _context.SaveChanges();
                    return Ok("Cargo removido com sucesso."); 
                }
            }catch(Exception e ){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{idCargo}")]
        public IActionResult putCargo(int id,[FromBody] Cargo cargo)
        {
            try{
                if (string.IsNullOrWhiteSpace(cargo.nomeCargo)){
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (cargo.salarioBase<=0){
                    return BadRequest("O salário precisa ser maior que zero");
                }
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == id);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cargo."); 
                    }
                    _context.Entry(item).CurrentValues.SetValues(cargo);
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);                 
            }
        }
        

    }

}