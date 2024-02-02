using Microsoft.AspNetCore.Mvc;
namespace ProjetoFinal
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : Controller
    {
        [HttpPost("Cliente")]
        public IActionResult Post_Cliente([FromBody] Cliente cliente)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    _context.clientes.Add(cliente);
                    _context.SaveChanges();
                    return new ObjectResult(cliente);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Cliente/{id}")]
        public IActionResult Get_Cliente(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == id);
                    if(item == null)
                    {
                        return NotFound("O cliente não foi encontrado");
                    }
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Cliente/{id}")]
        public IActionResult Put_Cliente(int id,[FromBody] Cliente cliente)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == id);
                    if(item == null)
                    {
                        return NotFound("O cliente não foi encontrado");
                    }
                    _context.Entry(item).CurrentValues.SetValues(cliente);
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);                 
            }
        }

        [HttpDelete("Cliente/{id}")]
        public IActionResult Delete_Cliente(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var ClienteNulo=_context.clientes.FirstOrDefault(x => x.nomeCliente == "Cliente excluido");
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == id);
                    if(item == null)
                    {
                        return NotFound("O cliente não foi encontrado"); 
                    }
                    foreach (Projeto projeto in _context.projetos){
                        if (projeto.idCliente==id && ClienteNulo!=null){
                            projeto.idCliente=ClienteNulo.codCliente;
                        }
                    }
                    _context.clientes.Remove(item);
                    _context.SaveChanges();
                    return Ok("O cliente foi deletado"); 
                }
            }catch(Exception e ){
                return BadRequest(e.Message);
            }
        }

    }

}