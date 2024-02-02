using Microsoft.AspNetCore.Mvc;
namespace ProjetoFinal
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : Controller
    {
        [HttpPost("Departamento")]
        public IActionResult Post_Departamento([FromBody] Departamento departamento)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    _context.departamentos.Add(departamento);
                    _context.SaveChanges();
                    return new ObjectResult(departamento);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Departamento/{id}")]
        public IActionResult Get_Departamento(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == id);
                    if(item == null)
                    {
                        return NotFound();
                    }
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Departamento/{id}")]
        public IActionResult Put_Departamento(int id,[FromBody] Departamento departamento)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == id);
                    if(item == null)
                    {
                        return NotFound();
                    }
                    _context.Entry(item).CurrentValues.SetValues(departamento);
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
            }catch(Exception e){
                return BadRequest(e.Message);                 
            }
        }

        [HttpDelete("Departamento/{id}")]
        public IActionResult Delete_Departamento(int id)
        {
            try{
                using (var _context = new ProjetoFinalContext())
                {
                    var DepartamentoNulo=_context.departamentos.FirstOrDefault(x => x.nomeDepartamento == "Departamento nao definido");
                    var item = _context.departamentos.FirstOrDefault(y => y.codDepartamento == id);
                    if(item == null)
                    {
                        return NotFound("O departamento não foi encontrado"); 
                    }
                    foreach (Projeto projeto in _context.projetos){
                        if (projeto.codDepartamento==id && DepartamentoNulo!=null){
                            projeto.codDepartamento=DepartamentoNulo.codDepartamento;
                        }
                    }
                    foreach (Funcionario funcionario in _context.funcionarios){
                        if (funcionario.idDepartamento==id && DepartamentoNulo!=null){
                            funcionario.idDepartamento=DepartamentoNulo.codDepartamento;
                        }
                    }
                    _context.departamentos.Remove(item);
                    _context.SaveChanges();
                    return Ok("O cliente foi deletado"); 
                }
            }catch(Exception e ){
                return BadRequest(e.Message);
            }
        }
    }

}