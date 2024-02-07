using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
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
            using (StreamWriter escritor = System.IO.File.AppendText(path))//se ja existir apenas adicona texto, se não existe ele foi criado e agora vai receber o texto
            {
                escritor.WriteLine(entredaLog);
            };
        }

        private string arquivoBalancoMes(string texto)
        {
            texto = "____________________________________EMPRESA ____________________________________" +
            "\nRelatório dos gastos e ganhos previstos para o mes:\n";
            var _context = new ProjetoFinalContext();
            float arrecadado = 0;
            float gasto = 0;
            float lucro = 0;
            DateTime dataAtual = DateTime.Today;
            if (!_context.projetos.Any())
            {
                return "Não temos projetos cadastrados";
            }
            if (!_context.funcionarios.Any())
            {
                return "Não temos funcionarios cadastrados";
            }
            texto += "Projetos a serem finalizados nesse mês : ";
            foreach (Projeto projeto in _context.projetos)
            {
                if (projeto.dataEntregaProjeto.Month == dataAtual.Month)
                {
                    texto += "\n" + "Código do projeto: " + Convert.ToString(projeto.codProjeto) + "        "
                    + "Nome do projeto: " + projeto.nomeProjeto + "        " + "Cliente: " + projeto.idCliente + "\n" + "Status: " + projeto.statusProjeto + "        " + "Data de entrega: " + Convert.ToString(projeto.dataEntregaProjeto);
                    arrecadado += projeto.valorProjeto;
                }
            }
            var funcionariosPorCargo = _context.funcionarios.OrderBy(funcionario => funcionario.idCargo).ToList();
            texto += "\nPagamentos a serem feitos nesse mês : ";
            foreach (Funcionario funcionario in funcionariosPorCargo)
            {
                var cargo = _context.cargos.FirstOrDefault(y => y.codCargo == funcionario.idCargo);
                if (cargo != null)
                {
                    texto += "\n" + "Código do funcionário: " + Convert.ToString(funcionario.codFuncionario) + "        " + "Nome do cargo: " + cargo.nomeCargo + "        " + "Salário : " + Convert.ToString(cargo.salarioBase);
                    gasto += cargo.salarioBase;
                }
            }
            lucro = arrecadado - gasto;
            texto += "\nValor arrecadado: " + Convert.ToString(arrecadado) + "\nValor a ser pago em salários: " + Convert.ToString(gasto) + "\nLucro: " + Convert.ToString(lucro);
            return texto;

        }

        public static void escreTxtSeq(string fileName, List<string> linhas)
        {
            string pasta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string txtName = fileName;
            string path = Path.Combine(pasta, txtName);
            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = System.IO.File.Create(path)) { };
            }

            System.IO.File.WriteAllText(path, string.Empty);
            foreach (string line in linhas)
            {
                using (StreamWriter escritor = System.IO.File.AppendText(path))
                {
                    escritor.WriteLine(line);
                };
            }



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

        [HttpGet("GetBalancoMes")]
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

        [HttpGet("ToText/")]
        public IActionResult getTxt()
        {
            List<List<int>> settingEntitySpacing = new List<List<int>> { new List<int> { 02, 100, 50, 50, 100, 100, 14, 18 ,20 },                  // Cliente CPF
                                                                         new List<int> { 02, 100, 50, 50, 100, 100, 14, 18 ,20 },                  // Cliente CNPJ
                                                                         new List<int> { 02, 50, 10 },                                             // Cargo
                                                                         new List<int> { 02, 02, 50, 60, 60 },                                         // Conta
                                                                         new List<int> { 02, 50, 02 },                                             // Departamento
                                                                         new List<int> { 02, 02, 02, 100, 50, 50, 100, 14, 50, 100, 100, 20 },     // Funcionario
                                                                         new List<int> { 02, 02, 02, 100, 200, 50, 10, 10},                        // Projeto
                                                                         new List<int> { 02, 02}                                                   // ProjetoFunc
                                                                        };
            sqlToTxt<Cliente>(new ProjetoFinalContext(), settingEntitySpacing[0], "Cliente.txt", 9);
            sqlToTxt<Cargo>(new ProjetoFinalContext(), settingEntitySpacing[2], "Cargo.txt", 3);
            sqlToTxt<ContaBancaria>(new ProjetoFinalContext(), settingEntitySpacing[3], "Conta.txt", 4);
            sqlToTxt<Departamento>(new ProjetoFinalContext(), settingEntitySpacing[4], "Departamento.txt", 3);
            sqlToTxt<Projeto>(new ProjetoFinalContext(), settingEntitySpacing[6], "Projeto.txt", 8);
            sqlToTxt<Funcionario>(new ProjetoFinalContext(), settingEntitySpacing[5], "Funcionario.txt", 12);
            sqlToTxtProjFunc(settingEntitySpacing[7], "ProjFunc.txt");

            return Ok();
        }
        //pergunta, por que nao usa o [1]?
        private void sqlToTxtProjFunc(List<int> currentSetting, string fileName)
        {
            List<string> linhasTabela = new List<string>();
            string test = "";
            var _context = new ProjetoFinalContext();
            DbSet<ProjetoFuncionario> funcionariosProjeto = _context.funcionariosProjeto;
            var ordenado = funcionariosProjeto.OrderBy(c => c.idProjeto);
            foreach (ProjetoFuncionario funcProj in ordenado)
            {
                int idProj = funcProj.idProjeto;
                int idFunc = funcProj.idFuncionario;
                if (idProj < 10)
                {
                    test += '0';
                }
                test += idProj;
                if (idFunc < 10)
                {
                    test += '0';
                }
                test += idFunc;
                linhasTabela.Add(test);
                test = "";
            }

            escreTxtSeq(fileName, linhasTabela);
        }

        private string sqlToTxt<T>(DbContext context, List<int> currentSetting, string fileName, int stop) where T : class
        {
            List<string> listaLinhas = new List<string>();
            int lastInt = 1;
            string test = "";
            int currentEntityIndex = 1;
            var _context = context.Set<T>();
            var primeira = _context.FirstOrDefault();
            if (primeira != null)
            {
                currentEntityIndex = Convert.ToInt32(primeira.GetType().GetProperties()[0].GetValue(primeira));
            }
            if (currentEntityIndex == 0)
            {
                return "";
            }
            int ogNumEnt = currentEntityIndex;
            if (_context.Count() != 0)
            {
                var lastEntity = _context.AsEnumerable().OrderByDescending(c => c.GetType().GetProperties().ElementAt(0).GetValue(c)).First();
                lastInt = Convert.ToInt32(lastEntity.GetType().GetProperties()[0].GetValue(lastEntity));
            }
            while (currentEntityIndex != lastInt + 1)
            {
                var entity = _context.Find(currentEntityIndex);
                if (entity != null && currentSetting != null)
                {
                    var props = entity.GetType().GetProperties();
                    for (int i = 0; i < stop; i++)
                    {
                        var attributeEntity = props[i];
                        var value = attributeEntity.GetValue(entity);
                        if (value != null)
                        {
                            string valueString = value.ToString() ?? "";
                            if (valueString != null)
                            {
                                int tamValue = valueString.Length;
                                int usoComparar = 0;
                                if (valueString.Length == 14 && i == 7)
                                {
                                    usoComparar = 14;
                                }
                                else
                                {
                                    usoComparar = currentSetting[i];
                                }
                                if (tamValue != usoComparar)
                                {
                                    int numEspacos = usoComparar - tamValue;
                                    valueString += new string(' ', numEspacos);
                                }
                                test += valueString;
                            }
                        }
                        else
                        {
                            if (currentSetting[i] < 4)
                            {
                                test += "00";
                            }
                            else
                            {
                                test += "null" + new string(' ', currentSetting[i] - 4);
                            }
                        }

                    }
                    listaLinhas.Add(test);
                    escreTxtSeq(fileName, listaLinhas);
                    test = "";
                }
                currentEntityIndex++;

            }
            return test;
        }

    }

}


