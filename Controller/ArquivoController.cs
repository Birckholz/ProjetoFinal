using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;

namespace ProjetoFinal
{

    [ApiController]
    [Route("[controller]")]
    public class ArquivoController : Controller
    {

        public static void logErros(string errorMessage, string lugar)
        {
            string pasta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string txtErros = "LogErrors.txt";
            string path = Path.Combine(pasta, txtErros);
            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = System.IO.File.Create(path)) { };
            }
            DateTime horarioDoErro = DateTime.Now;
            string entredaLog = $"[{horarioDoErro}]" + $"[{lugar}]" + errorMessage + "\n";
            using (StreamWriter escritor = System.IO.File.AppendText(path))
            {
                escritor.WriteLine(entredaLog);
            };
        }

//funcao arquivo extra
        private string arquivoBalancoMes(string texto)
        {
            texto = "____________________________________EMPRESA ____________________________________" +
            "\nRelatório dos gastos e ganhos previstos para o mes:\n";
            var _context = new ProjetoFinalContext();
            float arrecadado=0;
            float gasto=0;
            float lucro=0;
            DateTime dataAtual = DateTime.Today;
            if (!_context.projetos.Any())
            {
                return "Não temos projetos cadastrados";
            }
            if (!_context.funcionarios.Any())
            {
                return "Não temos funcionarios cadastrados";
            }
            texto+="Projetos a serem finalizados nesse mês : ";
            foreach (Projeto projeto in _context.projetos)
            {
                if(projeto.dataEntregaProjeto.Month==dataAtual.Month){
                texto += "\n" + "Código do projeto: " + Convert.ToString(projeto.codProjeto) + "        "
                + "Nome do projeto: " + projeto.nomeProjeto + "        " + "Cliente: " + projeto.idCliente + "\n" + "Status: " + projeto.statusProjeto + "        " + "Data de entrega: " + Convert.ToString(projeto.dataEntregaProjeto);
                //e atualizamos o codAnterior, assim, se for de outro departamento ,fazemos um cabeçalho
                    arrecadado+=projeto.valorProjeto;
                }
            }
            var funcionariosPorCargo = _context.funcionarios.OrderBy(funcionario => funcionario.idCargo).ToList();
            texto+="\nPagamentos a serem feitos nesse mês : ";
            foreach (Funcionario funcionario in funcionariosPorCargo)
            {
                var cargo = _context.cargos.FirstOrDefault(y => y.codCargo == funcionario.idCargo);
                texto += "\n" + "Código do funcionário: "+Convert.ToString(funcionario.codFuncionario) +"        "+"Nome do cargo: "+ cargo.nomeCargo + "        "+"Salário : "+Convert.ToString(cargo.salarioBase);
                gasto+=cargo.salarioBase;
            }
            lucro=arrecadado-gasto;
            texto+="\nValor arrecadado: "+Convert.ToString(arrecadado)+"\nValor a ser pago em salários: "+Convert.ToString(gasto)+"\nLucro: "+Convert.ToString(lucro);
            return texto;

        }

        private string arquivoProjDepart(string texto)
        {
            texto = "____________________________________EMPRESA ____________________________________" +
            "\nRelatório dos projetos de cada departamento:\n";
            var _context = new ProjetoFinalContext();
            int codAnterior = -1;
            string departamentoCodNome = "";
            if (!_context.projetos.Any())
            {
                return "Não temos projetos cadastrados";
            }
            //vamos ter uma lista organizada pelo departamento
            else
            {
                var projetosDepartamento = _context.projetos.OrderBy(projeto => projeto.codDepartamento).ToList();
            }
            if (!_context.departamentos.Any())
            {
                return "Não temos departamentos cadastrados";
            }
            foreach (Projeto projeto in _context.projetos)
            {
                //se mudou o departamento,imprimimos o novo
                if (codAnterior != projeto.codDepartamento)
                {
                    var departamento = _context.departamentos.FirstOrDefault(y => y.codDepartamento == projeto.codDepartamento);
                    if (departamento == null)
                    {
                        departamentoCodNome = "Departamento não existe";
                    }
                    else
                    {
                        departamentoCodNome = "DEPARTAMENTO : " + Convert.ToString(departamento.codDepartamento) + "----->" + departamento.nomeDepartamento;
                    }
                    texto += "\n" + departamentoCodNome;
                }
                //depois imprimimos detalhes do projeto
                texto += "\n" + "Código do projeto: " + Convert.ToString(projeto.codProjeto) + "        "
                + "Nome do projeto: " + projeto.nomeProjeto + "        " + "Cliente: " + projeto.idCliente + "\n" + "Status: " + projeto.statusProjeto + "        " + "Data de entrega: " + Convert.ToString(projeto.dataEntregaProjeto);
                //e atualizamos o codAnterior, assim, se for de outro departamento ,fazemos um cabeçalho
                codAnterior = projeto.codDepartamento;
            }
            return texto;

        }

        private string arquivoProjFuncio(string texto, Funcionario funcionario)
        {
            texto = "____________________________________EMPRESA ____________________________________" +
            "\nRelatório dos projetos do funcionário de ID:" + Convert.ToString(funcionario.codFuncionario) + "\n" +
            "Nome: " + funcionario.nomeFuncionario + "        " + "Código de Cargo: " + funcionario.idCargo + "        " + "Código de Departamento: " + funcionario.idDepartamento + "\n" +
            "\nPROJETOS:\n";
            var _context = new ProjetoFinalContext();
            if (!_context.projetos.Any())
            {
                return "Não temos projetos cadastrados";
            }
            foreach (ProjetoFuncionario pf in _context.funcionariosProjeto)
            {
                //vamos achar as conexoes do nosso funcionario com outros projetos
                if (pf.idFuncionario == funcionario.codFuncionario)
                {
                    var projeto = _context.projetos.FirstOrDefault(y => y.codProjeto == pf.idProjeto);
                    if (projeto != null)
                    {//apenas para garantir que n seja nulo
                     //colocamos os detalhes do projeto
                        texto += "Código do projeto: " + Convert.ToString(projeto.codProjeto) + "        "
                        + "Nome do projeto: " + projeto.nomeProjeto + "        " + "Cliente: " + projeto.idCliente + "\n" + "Status: " + projeto.statusProjeto + "        "
                        + "Data de entrega: " + Convert.ToString(projeto.dataEntregaProjeto) + "\n\n";
                    }
                }
            }
            return texto;

        }


        [HttpGet("GetProjetosDepartamentos")]
        public IActionResult getProjetosDepartamentos()
        {
            try
            {
                string texto = "";
                // Caminho da pasta "Downloads" do usuário
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                // Nome do arquivo que será criado na pasta Downloads
                string arquivo = "Departamento_Projetos.txt";
                // garante que os caracteres de separação de diretório sejam tratados corretamente
                string pathCompleto = Path.Combine(path, arquivo);

                // Utiliza o StreamWriter para escrever no arquivo
                using (StreamWriter sw = new StreamWriter(pathCompleto))
                {
                    //Lógica para achar departamento e projetos
                    sw.Write(arquivoProjDepart(texto));
                }
                return Ok("O arquivo foi criado na pasta de Downloads");
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "ArquivoController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetProjetosFuncionario/{idFuncionario}")]
        public IActionResult getProjetosFuncionario(int idFuncionario)
        {
            try
            {
                var _context = new ProjetoFinalContext();
                Funcionario? funcionario = _context.funcionarios.FirstOrDefault(p => p.codFuncionario == idFuncionario);
                if (funcionario == null)
                {
                    return NotFound("Esse funcionário não existe");
                }
                string texto = "";
                // Caminho da pasta "Downloads" do usuário
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                // Nome do arquivo que será criado na pasta Downloads
                string arquivo = "Funcionario_Projetos.txt";
                // Caminho completo do arquivo na pasta Downloads
                string pathCompleto = Path.Combine(path, arquivo);

                // Utiliza o StreamWriter para escrever no arquivo
                using (StreamWriter sw = new StreamWriter(pathCompleto))
                {
                    sw.Write(arquivoProjFuncio(texto, funcionario));
                }
                return Ok("O arquivo foi criado na pasta de Downloads");
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "ArquivoController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BalancoMes")]
        public IActionResult getBalancoMes()
        {
            try
            {
                string texto = "";
                // Caminho da pasta "Downloads" do usuário
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                // Nome do arquivo que será criado na pasta Downloads
                string arquivo = "Balanco_Mes.txt";
                // garante que os caracteres de separação de diretório sejam tratados corretamente
                string pathCompleto = Path.Combine(path, arquivo);

                // Utiliza o StreamWriter para escrever no arquivo
                using (StreamWriter sw = new StreamWriter(pathCompleto))
                {
                    //Lógica para achar departamento e projetos
                    sw.Write(arquivoBalancoMes(texto));
                }
                return Ok("O arquivo foi criado na pasta de Downloads");
            }
            catch (Exception e)
            {
                ArquivoController.logErros(e.Message, "ArquivoController");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ToText/dasda")]
        public IActionResult getTxt()
        {
            return Ok(sqlToTxt<Cliente>(new ProjetoFinalContext()));
        }

        private string sqlToTxt<T>(DbContext context) where T : class
        {
            string test = "";
            int currentEntityIndex = 1;
            var _context = context.Set<T>();
            while (currentEntityIndex != _context.Count() + 1)
            {
                var entity = _context.Find(currentEntityIndex);
                if (entity != null)
                {
                    var props = entity.GetType().GetProperties();
                    for (int i = 0; i < props.Length - 1; i++)
                    {
                        var attributeEntity = props[i];
                        var value = attributeEntity.GetValue(entity);
                        if (value != null)
                        {
                            test += value.ToString();
                        }

                    }
                }
                test += '\n';
                currentEntityIndex++;
            }
            return test;
        }

    }

}


