using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Projeto.Infra.Data.Entities;
using Projeto.Infra.Data.Repositories;
using Projeto.Presentation.Mvc.Models;
using Projeto.Presentation.Mvc.Reports;

namespace Projeto.Presentation.Mvc.Controllers
{
    [Authorize]
    public class AgendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // JsonResult pois ele se comunica com o javascript então ele recebe e retorna Json
        //método utilizado para retornar os dados para o gráfico
        public JsonResult ObterDadosGrafico(
        [FromServices] CompromissoRepository compromissoRepository,
        [FromServices] UsuarioRepository usuarioRepository
        )
        {
            try
            {
                //obter o usuário que está autenticado
                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);
                //consultar o resumo de compromissos por categoria
                var compromissos = compromissoRepository.GetResumoCategoriaDto(usuario.IdUsuario);
                //modelar os dados da consulta no padrão do HIGHCHARTS
                var result = new List<HighChartsModel>();
                foreach (var item in compromissos)
                {
                    var model = new HighChartsModel();
                    model.name = item.Categoria.ToString();
                    model.data = new List<int>() { item.Quantidade };
                    result.Add(model);
                }
                return Json(result);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }


        public IActionResult CadastroCompromisso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCompromisso(CompromissoCadastroModel model,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            //verificar se todos os campos da model passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //buscar no banco de dados os dados do usuario autenticado atraves do email
                    var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                    var compromisso = new Compromisso();
                    compromisso.Titulo = model.Titulo;
                    compromisso.Descricao = model.Descricao;
                    compromisso.DataInicio = DateTime.Parse(model.DataInicio);
                    compromisso.HoraInicio = TimeSpan.Parse(model.HoraInicio);
                    compromisso.DataFim = DateTime.Parse(model.DataFim);
                    compromisso.HoraFim = TimeSpan.Parse(model.HoraFim);
                    compromisso.IdUsuario = usuario.IdUsuario;
                    compromisso.Categoria = model.Categoria;

                    //gravar o compromisso no banco de dados
                    compromissoRepository.Create(compromisso);

                    TempData["MensagemSucesso"] = $"Compromisso {compromisso.Titulo} cadastrado com sucesso.";
                    ModelState.Clear(); //limpar o conteudo do formulario
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult ConsultaCompromisso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConsultaCompromisso(string dataMin, string dataMax, string exibirTodos,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            //lista de compromissos
            var lista = new List<Compromisso>();

            try
            {
                //verificar se a opção 'Exibir todos' foi marcada
                if(!string.IsNullOrEmpty(exibirTodos) && exibirTodos.Equals("1"))
               {
                    //obter o usuario que está autenticado no projeto
                    var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                    //pesquisar os compromissos no banco de dados
                    lista = compromissoRepository.GetAll(usuario.IdUsuario);

                    TempData["ExibirTodos"] = exibirTodos;
                }
                //verificando se as datas foram preenchidas no formulário
                else if (!string.IsNullOrEmpty(dataMin) && !string.IsNullOrEmpty(dataMax))
                {
                    //verificando se a dataMin é menor que a dataMax
                    if (DateTime.Parse(dataMin) < DateTime.Parse(dataMax))
                    {
                        //obter o usuario que está autenticado no projeto
                        var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                        //pesquisar os compromissos no banco de dados
                        lista = compromissoRepository.GetByDatas
                            (DateTime.Parse(dataMin), DateTime.Parse(dataMax), usuario.IdUsuario);

                        TempData["DataMin"] = dataMin;
                        TempData["DataMax"] = dataMax;
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Por favor, o período de datas informado é inválido.";
                    }
                }
                else
                {
                    TempData["MensagemErro"] = "Por favor, informe o período de datas para a pesquisa";
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Erro: " + e.Message;
            }

            //enviando os dados para a página
            return View(lista);
        }

        public IActionResult ExclusaoCompromisso(int id,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            try
            {
                //buscar o compromisso no banco de dados
                var compromisso = compromissoRepository.GetById(id);
                //buscar o usuario que está autenticado
                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                //verificando se o compromisso pertence ao usuario autenticado
                if (compromisso.IdUsuario == usuario.IdUsuario)
                {
                    compromissoRepository.Delete(compromisso);
                    TempData["MensagemSucesso"] = "Compromisso excluído com sucesso.";
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //redirecionamento
            return RedirectToAction("ConsultaCompromisso");
        }

        public IActionResult EdicaoCompromisso(int id,
            [FromServices] UsuarioRepository usuarioRepository,
            [FromServices] CompromissoRepository compromissoRepository)
        {
            //classe model utilizada no formulário
            var model = new CompromissoEdicaoModel();

            try
            {
                //buscar o compromisso pelo id
                var compromisso = compromissoRepository.GetById(id);
                //buscar o usuario no banco de dados
                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                //verificando se o compromisso pertence ao usuario autenticado
                if (compromisso.IdUsuario == usuario.IdUsuario)
                {
                    //transferindo os dados do compromisso para a model
                    model.IdCompromisso = compromisso.IdCompromisso;
                    model.Titulo = compromisso.Titulo;
                    model.Descricao = compromisso.Descricao;
                    model.DataInicio = compromisso.DataInicio.ToString("dd/MM/yyyy");
                    model.HoraInicio = compromisso.HoraInicio.ToString(@"hh\:mm");
                    model.DataFim = compromisso.DataFim.ToString("dd/MM/yyyy");
                    model.HoraFim = compromisso.HoraFim.ToString(@"hh\:mm");
                    model.Categoria = compromisso.Categoria;
                }
                else
                {
                    return RedirectToAction("ConsultaCompromisso");
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //enviando a model para a página
            return View(model);
        }

        [HttpPost]
        public IActionResult EdicaoCompromisso(CompromissoEdicaoModel model,
           [FromServices] CompromissoRepository compromissoRepository)
        {
            //verificar se todos os campos da model passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var compromisso = new Compromisso();
                    compromisso.IdCompromisso = model.IdCompromisso;
                    compromisso.Titulo = model.Titulo;
                    compromisso.Descricao = model.Descricao;
                    compromisso.DataInicio = DateTime.Parse(model.DataInicio);
                    compromisso.HoraInicio = TimeSpan.Parse(model.HoraInicio);
                    compromisso.DataFim = DateTime.Parse(model.DataFim);
                    compromisso.HoraFim = TimeSpan.Parse(model.HoraFim);
                    compromisso.Categoria = model.Categoria;

                    //atualizar o compromisso no banco de dados
                    compromissoRepository.Update(compromisso);

                    TempData["MensagemSucesso"] = $"Compromisso {compromisso.Titulo} atualizado com sucesso.";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        //método executado pelo LINK da página
        public void GerarRelatorio(
            [FromServices] CompromissoRepository compromissoRepository,
            [FromServices] UsuarioRepository usuarioRepository
            )
        {
            try
            {
                //lista de compromissos
                var compromissos = compromissoRepository.GetAll();
                //usuário autenticado no sistema
                var usuario = usuarioRepository.GetByEmail(User.Identity.Name);

                //gerar o relatório PDF..
                var compromissoReport = new CompromissoReport();
                var pdf = compromissoReport.ObterRelatorioDeCompromissos(compromissos, usuario);

                //variável para definir o nome do arquivo de download
                var nomeArquivo = $"Relatorio {usuario.Nome} {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.pdf";

                //DOWNLOAD do documento..
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.Headers.Add("content-disposition", $"attachment; filename={nomeArquivo}");
                Response.Body.WriteAsync(pdf, 0, pdf.Length);
                Response.Body.Flush();
                Response.StatusCode = StatusCodes.Status200OK;
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }
        }

    }
}