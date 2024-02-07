using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging.Abstractions;

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

        private bool documentoValido(string valor,int tipo)
        {//para validar cpf e cnpj
            foreach (char caractere in valor)
            {
                if (!char.IsDigit(caractere) &&
                    caractere != ' ' &&
                    caractere != '-' &&
                    caractere != '.')
                {
                    if(tipo==1 &&
                    caractere != '/'){
                    return false;
                    }
                    return false;
                }
            }
            return true;
        }

        [HttpPost("Add/{nome}/{telefone}/{email}/{endereco}/{descricao}/{cpf}/{cnpj}/{status}")]
        public IActionResult postCliente(string nome, string telefone, string email, string endereco, string? descricao, string? cpf, string? cnpj, string? status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    throw new ExceptionCustom("O nome não pode ser nulo ou vazio");
                }
                if (!telefoneValido(telefone))
                {
                    throw new ExceptionCustom("O telefone fornecido não é válido");
                }
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ExceptionCustom("O email não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(endereco))
                {
                    throw new ExceptionCustom("O endereco não pode ser nulo ou vazio");
                }
                if (string.IsNullOrWhiteSpace(cpf) && string.IsNullOrWhiteSpace(cnpj))
                {
                    throw new ExceptionCustom("O cliente precisa de CPF ou CNPJ");
                }
                if (cpf != null && !documentoValido(cpf,0))
                {
                    throw new ExceptionCustom("O CPF não é válido");
                }
                if (cnpj != null && !documentoValido(cnpj,1))
                {
                    throw new ExceptionCustom("O CNPJ não é válido");
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
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "ClientesController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CLientesController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult getClientes()
        {
            try{
                var _context = new ProjetoFinalContext();
                DbSet<Cliente> clientes = _context.clientes;
                if (!clientes.Any())
                {
                    throw new ExceptionCustom("Não há nenhum cliente cadastrado");
                }
                return Ok(clientes);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CLientesController");
                return BadRequest(e.Message);
            }
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
                        throw new ExceptionCustom("Não foi possivel encontrar o cliente.");
                    }
                    return new ObjectResult(item);
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "ClientesController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CLientesController");
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
                    var ClienteNulo = _context.clientes.FirstOrDefault(x => x.nomeCliente == "Cliente excluido");//se quiser tirar um cliente,não quero excluir dados de um projeto que a empresa criou, pois esses dados fazem parte do portfolio dela
                    if(ClienteNulo ==null){
                        postCliente("Cliente excluido","000", "000", "000", null,null,null,null);
                        ClienteNulo = _context.clientes.FirstOrDefault(x => x.nomeCliente == "Cliente excluido");
                    }
                    var item = _context.clientes.FirstOrDefault(y => y.codCliente == idCliente);
                    if (item == null)
                    {
                       throw new ExceptionCustom("Não foi possivel encontrar o cliente.");
                    }
                    foreach (Projeto projeto in _context.projetos)
                    {
                        if (projeto.idCliente == idCliente)
                        {
                            if(ClienteNulo!=null){
                            projeto.idCliente = ClienteNulo.codCliente;
                            projeto.descricaoProjeto+=" Cliente: "+ item.nomeCliente;//isso fará com que descrição do projeto tenha o nome do cliente para qual foi feito ,mesmo que ele seja excluido
                            }
                        }
                    }
                    _context.clientes.Remove(item);
                    _context.SaveChanges();
                    return Ok("Cliente removido com sucesso.");
                }
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "ClientesController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CLientesController");
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
                    throw new ExceptionCustom("Não foi possivel encontrar o cliente.");
                }
                if (nome != null)
                {
                    if (!string.IsNullOrWhiteSpace(nome))
                    {
                        cliente.nomeCliente = nome;
                    }
                    else
                    {
                        throw new ExceptionCustom("O nome não pode ser vazio");
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
                        throw new ExceptionCustom("O telefone não pode ser vazio");
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
                        throw new ExceptionCustom("O email não pode ser vazio");
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
                        throw new ExceptionCustom("O endereço não pode ser vazio");
                    }
                }
                if (descricao != null)
                {
                    cliente.descricaoCliente = descricao;
                }
                if (cpf != null)
                {
                    if (documentoValido(cpf,0))
                    {
                        cliente.PessFCPFCliente = cpf;
                    }
                    else
                    {
                        throw new ExceptionCustom("O CPF não é válido");
                    }
                }
                if (cnpj != null)
                {
                    if (documentoValido(cnpj,1))
                    {
                        cliente.PessJCNPJCliente = cnpj;
                    }
                    else
                    {
                        throw new ExceptionCustom("O CNPJ não é válido");
                    }
                }
                if (status != null)
                {
                    cliente.statusCliente = status;
                }
                if (string.IsNullOrWhiteSpace(cliente.PessFCPFCliente) && string.IsNullOrWhiteSpace(cliente.PessJCNPJCliente))
                {
                    throw new ExceptionCustom("O cliente precisa de CPF ou CNPJ");
                }
                _context.SaveChanges();
                return new ObjectResult(cliente);
            }
            catch (ExceptionCustom e)
            {
                ArquivoController.logErros(e.Message, "ClientesController");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "CLientesController");
                return BadRequest(e.Message);
            }
        }


    }

}