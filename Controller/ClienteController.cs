using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{
    //refazer para vir por link , n por body e terminar validação
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        [HttpPost("Add")]
        public IActionResult postCliente([FromBody] Cliente cliente)
        {
            try{
                if (string.IsNullOrWhiteSpace(cliente.nomeCliente)){
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.telefoneCliente)){
                    return BadRequest("O telefone não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.emailCliente)){
                    return BadRequest("O email não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.enderecoCliente)){
                    return BadRequest("O endereco não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.PessJCNPJCliente) && string.IsNullOrWhiteSpace(cliente.PessFCPFCliente)){
                    return BadRequest("O cliente precisa de CPF ou CNPJ");
                }

                using (var _context = new ProjetoFinalContext())
                {
                    _context.clientes.Add(cliente);
                    _context.SaveChanges();
                    return new ObjectResult(cliente);
                }
            }catch(Exception e){
                return NotFound(e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult getClientes()
        {
            var _context = new ProjetoFinalContext();
            DbSet<Cliente> retorno = _context.clientes;
            return Ok(retorno);
        }

        [HttpGet("GetById/{idCliente}")]
        public IActionResult getCliente(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == id);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cliente.");
                    }
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{idCliente}")]
        public IActionResult deleteCliente(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var ClienteNulo=_context.clientes.FirstOrDefault(x => x.nomeCliente == "Cliente excluido");
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == id);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cliente."); 
                    }
                    foreach (Projeto projeto in _context.projetos){
                        if (projeto.idCliente==id && ClienteNulo!=null){
                            projeto.idCliente=ClienteNulo.codCliente;
                        }
                    }
                    _context.clientes.Remove(item);
                    _context.SaveChanges();
                    return Ok("Cliente removido com sucesso."); 
                }
            }catch(Exception e ){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{idCliente}")]
        public IActionResult putCliente(int id,[FromBody] Cliente cliente)
        {
            try{
                if (string.IsNullOrWhiteSpace(cliente.nomeCliente)){
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.telefoneCliente)){
                    return BadRequest("O telefone não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.emailCliente)){
                    return BadRequest("O email não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.enderecoCliente)){
                    return BadRequest("O endereco não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cliente.PessJCNPJCliente) && string.IsNullOrWhiteSpace(cliente.PessFCPFCliente)){
                    return BadRequest("O cliente precisa de CPF ou CNPJ");
                }
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == id);
                    if(item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cliente.");
                    }
                    _context.Entry(item).CurrentValues.SetValues(cliente);
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);                 
            }
        }


    }

}