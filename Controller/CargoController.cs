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
        
        [HttpPost("Add/{nome}/{salario}")]
        public IActionResult postCargo(string nome,float salario)
        {
            try{
                if (string.IsNullOrWhiteSpace(nome)){
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (salario<=0){
                    return BadRequest("O salário precisa ser maior que zero");
                }
                Cargo cargo =new Cargo(){
                    nomeCargo=nome,
                    salarioBase=salario
                };
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
        public IActionResult getCargo(int idCargo)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == idCargo);
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

//DUVIDA

        [HttpDelete("Delete/{idCargo}")]
        public IActionResult deleteCargo(int idCargo)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
 //esses var precisam '?' pra se n achar o cargoNulo?
                    var CargoNulo=_context.cargos.FirstOrDefault(x => x.nomeCargo == "Cargo nao definido");
                    var item = _context.cargos.FirstOrDefault(y => y.codCargo == idCargo);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cargo."); 
                    }
                    foreach (Funcionario funcionario in _context.funcionarios){
                        if (funcionario.idCargo==idCargo && CargoNulo!=null){
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
        public IActionResult putCargo(int idCargo,string? nome, float?salario)
        {
            try{
                var _context = new ProjetoFinalContext();
                var cargo = _context.cargos.FirstOrDefault(y => y.codCargo == idCargo);
                if(cargo == null)
                {
                    return NotFound("Não foi possivel encontrar o cargo."); 
                }
                if (nome != null){
                    if( !string.IsNullOrWhiteSpace(nome))
                    {
                        cargo.nomeCargo=nome;
                    }
                    else{
                        return BadRequest("O nome não pode ser vazio");
                    }
                }
                if (salario !=null ){
                    if(salario>0){
                        float salario1=Convert.ToSingle(salario);
                        cargo.salarioBase= salario1;
                    }
                    else{
                        return BadRequest("O salário precisa ser maior que zero");
                    }
                }
                _context.SaveChanges();
                return new ObjectResult(cargo);
            }catch(Exception e){
                return BadRequest(e.Message);                 
            }
        }
        

    }

}