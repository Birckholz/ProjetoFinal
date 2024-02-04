using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private bool telefoneValido(string telefone)
        {
            foreach (char caractere in telefone)
            {
                if (!char.IsDigit(caractere) &&
                    caractere != ' ' &&
                    caractere != '-' &&
                    caractere != '+' &&
                    caractere != '(' &&
                    caractere != ')')
                {
                    return false;
                }
            }
            return true;
        }
        private bool documentoValido(string valor)
        {//para validar cpf e cnpj
            foreach (char caractere in valor)
            {
                if (!char.IsDigit(caractere) &&
                    caractere != ' ' &&
                    caractere != '-' &&
                    caractere != '.' &&
                    caractere != '/')
                {
                    return false;
                }
            }
            return true;
        }

        //como tem atributos opcionais, será por query o Add + path os obrigatorios
        [HttpPost("Add/{nome}/{telefone}/{email}/{endereco}")]
        public IActionResult postCliente(string nome, string telefone, string email, string endereco, string? descricao, string? cpf, string? cnpj, string? status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    return BadRequest("O nome não pode ser nulo ou vazio");
                }
                if (!telefoneValido(telefone))
                {
                    return BadRequest("O telefone fornecido não é válido");
                }
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("O email não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(endereco))
                {
                    return BadRequest("O endereco não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cpf) && string.IsNullOrWhiteSpace(cnpj))
                {
                    return BadRequest("O cliente precisa de CPF ou CNPJ");
                }
                if (cpf != null && !documentoValido(cpf))
                {
                    return BadRequest("O CPF não é válido");
                }
                if (cnpj != null && !documentoValido(cnpj))
                {
                    return BadRequest("O CNPJ não é válido");
                }
                Cliente cliente = new Cliente()
                {
                    nomeCliente = nome,
                    telefoneCliente = telefone,
                    emailCliente = email,
                    enderecoCliente = endereco,
                    descricaoCliente = descricao,
                    PessFCPFCliente = cpf,
                    PessJCNPJCliente = cnpj,
                    statusCliente = status
                };
                using (var _context = new ProjetoFinalContext())
                {
                    _context.clientes.Add(cliente);
                    _context.SaveChanges();
                    return new ObjectResult(cliente);
                }
            }
            catch (Exception e)
            {
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
        public IActionResult getCliente(int idCliente)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == idCliente);
                    if (item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cliente.");
                    }
                    return new ObjectResult(item);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{idCliente}")]
        public IActionResult deleteCliente(int idCliente)
        {
            try
            {
                using (var _context = new ProjetoFinalContext())
                {
                    var ClienteNulo = _context.clientes.FirstOrDefault(x => x.nomeCliente == "Cliente excluido");
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == idCliente);
                    if (item == null)
                    {
                        return NotFound("Não foi possivel encontrar o cliente.");
                    }
                    foreach (Projeto projeto in _context.projetos)
                    {
                        if (projeto.idCliente == idCliente && ClienteNulo != null)
                        {
                            projeto.idCliente = ClienteNulo.codCliente;
                        }
                    }
                    _context.clientes.Remove(item);
                    _context.SaveChanges();
                    return Ok("Cliente removido com sucesso.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{idCliente}")]
        public IActionResult putCliente(int idCliente, string? nome, string? telefone, string? email, string? endereco, string? descricao, string? cpf, string? cnpj, string? status)
        {
            try
            {
                var _context = new ProjetoFinalContext();
                var cliente = _context.clientes.FirstOrDefault(y => y.codCliente == idCliente);
                if (cliente == null)
                {
                    return NotFound("Não foi possivel encontrar o cliente.");
                }
                if (nome != null)
                {
                    if (!string.IsNullOrWhiteSpace(nome))
                    {
                        cliente.nomeCliente = nome;
                    }
                    else
                    {
                        return BadRequest("O nome não pode ser vazio");
                    }
                }
                if (telefone != null)
                {
                    if (telefoneValido(telefone))
                    {
                        cliente.telefoneCliente = telefone;
                    }
                    else
                    {
                        return BadRequest("O telefone não pode ser vazio");
                    }
                }
                if (email != null)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        cliente.emailCliente = email;
                    }
                    else
                    {
                        return BadRequest("O email não pode ser vazio");
                    }
                }
                if (endereco != null)
                {
                    if (!string.IsNullOrWhiteSpace(endereco))
                    {
                        cliente.enderecoCliente = endereco;
                    }
                    else
                    {
                        return BadRequest("O endereço não pode ser vazio");
                    }
                }
                if (descricao != null)
                {
                    cliente.descricaoCliente = descricao;
                }
                if (cpf != null)
                {
                    if (documentoValido(cpf))
                    {
                        cliente.PessFCPFCliente = cpf;
                    }
                    else
                    {
                        return BadRequest("O CPF não é válido");
                    }
                }
                if (cnpj != null)
                {
                    if (documentoValido(cnpj))
                    {
                        cliente.PessJCNPJCliente = cnpj;
                    }
                    else
                    {
                        return BadRequest("O CNPJ não é válido");
                    }
                }
                if (status != null)
                {
                    cliente.statusCliente = status;
                }
                if (string.IsNullOrWhiteSpace(cliente.PessFCPFCliente) && string.IsNullOrWhiteSpace(cliente.PessJCNPJCliente))
                {
                    return BadRequest("O cliente precisa de CPF ou CNPJ");
                }
                _context.SaveChanges();
                return new ObjectResult(cliente);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }

}