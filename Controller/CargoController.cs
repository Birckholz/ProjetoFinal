using Microsoft.AspNetCore.Mvc;
namespace ProjetoFinal
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : Controller
    {

        // criar direto no banco o nulo
        
        [HttpPost("Cargo")]
        public IActionResult Post_Cargo([FromBody] Cargo cargo)
        {
            try{
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

        [HttpGet("Cargo/{id}")]
        public IActionResult Get_Cargo(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == id);
                    if(item == null)
                    {
                        return NotFound("O cargo não foi encontrado"); 
                    }
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Cargo/{id}")]
        public IActionResult Put_Cargo(int id,[FromBody] Cargo cargo)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == id);
                    if(item == null)
                    {
                        return NotFound("O cargo não foi encontrado"); 
                    }
                    _context.Entry(item).CurrentValues.SetValues(cargo);
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);                 
            }
        }
        
        [HttpDelete("Cargo/{id}")]
        public IActionResult Delete_Cargo(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var CargoNulo=_context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == id);
                    if(item == null)
                    {
                        return NotFound("O cargo não foi encontrado"); 
                    }
                    foreach (Funcionario funcionario in _context.funcionarios){
                        if (funcionario.idCargo==id && CargoNulo!=null){
                            funcionario.idCargo=CargoNulo.codCargo;
                        }
                    }
                    _context.cargos.Remove(item);
                    _context.SaveChanges();
                    return Ok("O cargo foi deletado"); 
                }
            }catch(Exception e ){
                return BadRequest(e.Message);
            }
        }
    }

}