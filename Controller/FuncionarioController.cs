
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoFinal;
[ApiController]
[Route("[controller]")]
public class FuncionarioController : Controller
{

    private Departamento? findDepartamento(int idDepartamento)
    {
        var _context = new ProjetoFinalContext();
        Departamento? entityRetorno = _context.departamentos.FirstOrDefault(d => d.codDepartamento == idDepartamento);
        return entityRetorno;
    }


    private Cargo? findCargo(int idCargo)
    {
        var _context = new ProjetoFinalContext();
        Cargo? entityRetorno = _context.cargos.FirstOrDefault(c => c.codCargo == idCargo);
        return entityRetorno;
    }

    private bool validaCargo(int idCargo)
    {
        var _context = new ProjetoFinalContext();
        Cargo? entityCheck = _context.cargos.FirstOrDefault(c => c.codCargo == idCargo);
        return entityCheck != null;
    }
    private bool validaDepartamento(int? idDepartamento)
    {
        if (idDepartamento == null)
        {
            return true;
        }
        var _context = new ProjetoFinalContext();
        Departamento? entityCheck = _context.departamentos.FirstOrDefault(d => d.codDepartamento == idDepartamento);
        return entityCheck != null;
    }

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
                caractere != '.')
            {
                return false;
            }
        }
        return true;
    }



    [HttpPost("Add/{idCargo}/{idDepartamento}/{nomeFuncionario}/{telefoneFuncionario}/{emailFuncionario}/{enderecoFuncionario}/{CPFFuncionario}/{tipoContrFuncionario}/{modoTrabFuncionario}/{formacaoRelevanteFuncionario}/{statusFuncionario}")]
    public IActionResult addFuncionario(int idCargo, int? idDepartamento, string CPFFuncionario, string emailFuncionario, string enderecoFuncionario, string formacaoRelevanteFuncionario, string modoTrabFuncionario, string nomeFuncionario, string statusFuncionario, string telefoneFuncionario, string tipoContrFuncionario)
    {
        Departamento? departamentoFunc = null;
        var _context = new ProjetoFinalContext();
        try
        {

            if (!validaCargo(idCargo))
            {
                throw new ExceptionCustom("Cargo não foi encontrado");
            }
            if (!validaDepartamento(idDepartamento))
            {
                throw new ExceptionCustom("Departamento não foi encontrado");
            }
            if (nomeFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Nome Invalido");
            }
            if (telefoneFuncionario.IsNullOrEmpty() || !telefoneValido(telefoneFuncionario))
            {
                throw new ExceptionCustom("Telefone Invalido");
            }
            if (enderecoFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Endereco Invalido");
            }
            if (emailFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Email Invalido");
            }
            if (CPFFuncionario.IsNullOrEmpty() || !documentoValido(CPFFuncionario))
            {
                throw new ExceptionCustom("CPF Invalido");
            }
            if (tipoContrFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Tipo de Contrato Invalido");
            }
            if (modoTrabFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Modo de Trabalho Invalido");
            }
            if (formacaoRelevanteFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Formacao nao reconehecida");
            }
            if (statusFuncionario.IsNullOrEmpty())
            {
                throw new ExceptionCustom("Status Invalido");
            }
            if (idDepartamento != null)
            {
                departamentoFunc = findDepartamento(idDepartamento ?? -1);// Se for nulo, atribui o valor -1 a idDepartamento
            }
            Cargo? cargoFunc = findCargo(idCargo);

            Funcionario funcAdicionado = new Funcionario()
            {
                idCargo = idCargo,
                idDepartamento = idDepartamento,
                CPFFuncionario = CPFFuncionario,
                emailFuncionario = emailFuncionario,
                enderecoFuncionario = enderecoFuncionario,
                formacaoRelevanteFuncionario = formacaoRelevanteFuncionario,
                nomeFuncionario = nomeFuncionario,
                statusFuncionario = statusFuncionario,
                modoTrabFuncionario = modoTrabFuncionario,
                telefoneFuncionario = telefoneFuncionario,
                tipoContrFuncionario = tipoContrFuncionario
            };
            _context.funcionarios.Add(funcAdicionado);
            if (departamentoFunc != null)
            {
                departamentoFunc.funcionariosDepartamento.Add(funcAdicionado);
            }
            if (cargoFunc != null)
            {
                cargoFunc.funcionarioCargos.Add(funcAdicionado);
            }
            _context.SaveChanges();
            return new ObjectResult(funcAdicionado);
        }
        catch (ExceptionCustom e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return NotFound(e.Message);
        }
        catch (Exception t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return BadRequest(t.Message);
        }

    }
    [HttpGet("Get")]
    public IActionResult getFuncionarios()
    {
        try
        {
            var _context = new ProjetoFinalContext();
            DbSet<Funcionario> retorno = _context.funcionarios;
            return Ok(retorno);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetById/{idFuncionario}")]
    public IActionResult getFuncById(int idFuncionario)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            Funcionario? entityGet = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
            if (entityGet == null)
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            return Ok(entityGet);
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return NotFound(t.Message);

        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("Delete/{idFuncionario}")]
    public IActionResult removerFunc(int idFuncionario)
    {
        try
        {
            var _context = new ProjetoFinalContext();
            Funcionario? entityRemove = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);
            if (entityRemove == null)
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            //vamos checar se é responsável por departamento, se for , não vai poder mudar ,vai jogar excessao, pedindo pra mudar o responsavel do departamento
            Departamento? departamento = _context.departamentos.FirstOrDefault(f => f.idResponsavel == idFuncionario);
            if(departamento!=null){
                throw new ExceptionCustom("Funcionario é responsável por um departamento, primeiro altere o responsável pelo departamento");
            }
            foreach (ContaBancaria c in _context.contasBancarias)//como não configuramos por cascade temos que excluir
            {
                if (c.codFuncionario == idFuncionario)
                {
                    _context.contasBancarias.Remove(c);
                }
            }
            _context.funcionarios.Remove(entityRemove);
            _context.SaveChanges();
            return Ok("Funcionario removido com sucesso.");
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }
    }

    [HttpPut("Update/{idFuncionario}/")]
    public IActionResult updateFunc(int idFuncionario, int? idCargo,int? idDepartamento,string? nomeFuncionario, string? telefoneFuncionario, string? enderecoFuncionario, string? emailFuncionario, string? CPFFuncionario, string? tipoContrFuncionario, string? modoTrabFuncionario, string? formacaoRelevanteFuncionario, string? statusFuncionario)
    {
        var _context = new ProjetoFinalContext();
        Funcionario? entityUpdate = _context.funcionarios.FirstOrDefault(f => f.codFuncionario == idFuncionario);

        try
        {
            if (entityUpdate == null)
            {
                throw new ExceptionCustom("Funcionario não encontrado");
            }
            if (!nomeFuncionario.IsNullOrEmpty())
            {
                entityUpdate.nomeFuncionario = nomeFuncionario;
            }
            if (!telefoneFuncionario.IsNullOrEmpty())
            {
                if (telefoneFuncionario != null)
                {
                    if (telefoneValido(telefoneFuncionario))
                    {
                        entityUpdate.telefoneFuncionario = telefoneFuncionario;
                    }
                    else
                    {
                        throw new ExceptionCustom("Telefone não é válido");
                    }
                }
            }
            if (!enderecoFuncionario.IsNullOrEmpty())
            {
                entityUpdate.enderecoFuncionario = enderecoFuncionario;
            }
            if (!emailFuncionario.IsNullOrEmpty())
            {
                entityUpdate.emailFuncionario = emailFuncionario;
            }
            if (!CPFFuncionario.IsNullOrEmpty())
            {
                if (CPFFuncionario != null)
                {
                    if (documentoValido(CPFFuncionario))
                    {
                        entityUpdate.CPFFuncionario = CPFFuncionario;
                    }
                    else
                    {
                        throw new ExceptionCustom("Documento não é válido");
                    }
                }

            }
            if (!tipoContrFuncionario.IsNullOrEmpty())
            {
                entityUpdate.tipoContrFuncionario = tipoContrFuncionario;
            }
            if (!modoTrabFuncionario.IsNullOrEmpty())
            {
                entityUpdate.modoTrabFuncionario = modoTrabFuncionario;
            }
            if (!formacaoRelevanteFuncionario.IsNullOrEmpty())
            {
                entityUpdate.formacaoRelevanteFuncionario = formacaoRelevanteFuncionario;
            }
            if (!statusFuncionario.IsNullOrEmpty())
            {
                entityUpdate.statusFuncionario = statusFuncionario;
            }
            if(idCargo !=null){
                int idCarg=Convert.ToInt32(idCargo);
                if(findCargo(idCarg)!=null){
                    entityUpdate.idCargo=idCarg;
                }
                else
                {
                    throw new ExceptionCustom("Cargo não é válido");
                }
            }
            if(idDepartamento !=null){
                int idDepart=Convert.ToInt32(idDepartamento);
                if(findDepartamento(idDepart)!=null){
                    var departamento = _context.departamentos.FirstOrDefault(f => f.idResponsavel == entityUpdate.codFuncionario);//ver se ele não é responsavel por um departamento, pois se for, não podemos mudar
                    if(departamento==null || departamento.codDepartamento==idDepart){
                        entityUpdate.idDepartamento=idDepart;

                    }
                    else
                    {
                        throw new ExceptionCustom("O funcionário é responsável por um departamento, modifique o responsável e depois mude o funcionário de departamento");
                    }
                }
                else
                {
                    throw new ExceptionCustom("Departamento não é válido");
                }
            }
            _context.SaveChanges();
            return new ObjectResult(entityUpdate);
        }
        catch (ExceptionCustom t)
        {
            ArquivoController.logErros(t.Message, "FuncionarioController");
            return NotFound(t.Message);
        }
        catch (Exception e)
        {
            ArquivoController.logErros(e.Message, "FuncionarioController");
            return BadRequest(e.Message);
        }



    }
}
