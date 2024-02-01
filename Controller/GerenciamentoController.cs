using Microsoft.AspNetCore.Mvc;
namespace ProjetoFinal
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoFinalController : Controller
    {
        //CRUD CARGO
        [HttpGet("Cargo/{id}")]
        public IActionResult Get_cargo(int id)
        {

        }
        //CRUD CLIENTE
        //CRUD DEPARTAMENTO

    }

}