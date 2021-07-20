using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Repository.Entities;
using Project.Repository.Repositories;
using Projetc.Presentation.Mvc.Models;
using Projeto.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Projetc.Presentation.Mvc.Controllers
{
    public class DependenteController : Controller
    {
        public IActionResult Cadastro([FromServices] ClientRepository clienteRepository)
        {
            var result = GetDependenteCadastroModel(clienteRepository);
            return View(result);
        }


        [HttpPost] //Método é executado pelo SUBMIT do formulário
        public IActionResult Cadastro(
            DependenteCadastroModel model,
            [FromServices] DependenteRepository dependenteRepository,
            [FromServices] ClientRepository clienteRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dependente = new Dependente();
                    dependente.Nome = model.Nome;
                    dependente.DataNascimento = DateTime.Parse(model.DataNascimento, CultureInfo.InvariantCulture);
                    dependente.IdCliente = int.Parse(model.IdCliente);

                    dependenteRepository.Create(dependente);

                    TempData["MensagemSucesso"] = $"Dependente {dependente.Nome} cadastrado com sucesso.";
                    ModelState.Clear(); //limpart os campos do formúlario
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Error: " + e.Message;
                }
            }
            var result = GetDependenteCadastroModel(clienteRepository);

            return View(result);
        }

        public IActionResult Consulta([FromServices] DependenteRepository dependenteRepository)
        {
            var dependentes = new List<Dependente>();
            try
            {
                dependentes = dependenteRepository.GetAll();
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Error :" + e.Message;
            }
            return View(dependentes);
        }
        [HttpPost]
        public IActionResult Consulta(string nome, [FromServices] DependenteRepository dependenteRepository)
        {
            var dependentes = new List<Dependente>();
            try
            {
                dependentes = dependenteRepository.GetByNome(nome);
                TempData["Nome"] = nome;
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Error : " + e.Message;
            }
            return View(dependentes);
        }
        public IActionResult Exclusao(int id, [FromServices] DependenteRepository dependenteRepository)
        {
            try
            {
                //vai fazer uma busca no banco de dados atravez do ID
                var dependenete = dependenteRepository.GetById(id);
                //verificando se o dependente foi encontrado
                if (dependenete != null)
                {
                    //excluindo o dependente
                    dependenteRepository.Delete(dependenete);
                    TempData["MensagemSucesso"] = "Dependente excluído com sucesso.";
                }
                else
                {
                    throw new Exception("Dependente não encontrado.");
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Error : " + e.Message;
            }
            return RedirectToAction("Consulta"); //redirecionamento para a pagina
        }
        public IActionResult Edicao(
            int id,
            [FromServices] DependenteRepository dependenteRepository,
            [FromServices] ClientRepository clientRepository)
        {
            var model = GetDependenteEdicaoModel(clientRepository);
            try
            {
                //buscar o dependente no banco de dados pelo id
                var dependente = dependenteRepository.GetById(id);

                //transferir os dados do dependente para o model
                model.IdDependente = dependente.IdDependente;
                model.Nome = dependente.Nome;
                model.DataNascimento = dependente.DataNascimento.ToString("dd/MM/yyyy");
                model.IdCliente = dependente.IdCliente.ToString();
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Error :" + e.Message;
            }

            return View(model); //enviando a model para a pagina para mostrar a lista de cliente e campos preenchidos
        }
        [HttpPost]
        public IActionResult Edicao(
            DependenteEdicaoModel model,
            [FromServices] ClientRepository clienteRepository,
            [FromServices] DependenteRepository dependenteRepository)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var dependente = new Dependente();
                    dependente.IdDependente = model.IdDependente;
                    dependente.Nome = model.Nome;
                    dependente.DataNascimento = DateTime.Parse(model.DataNascimento);
                    dependente.IdCliente = int.Parse(model.IdCliente);

                    //atualizando o banco de dados
                    dependenteRepository.Update(dependente);
                    TempData["MensagemSucesso"] = $"Dependente {dependente.Nome} atualizado com sucesso.";

                }catch(Exception e)
                {
                    TempData["MensagemErro"] = "Error: " + e.Message;
                }
            }
                //var result = GetDependenteEdicaoModel(clienteRepository);
                //return View(result);
                return RedirectToAction("Consulta");
        }
        private List<SelectListItem> GetClientes(ClientRepository clienteRepository)
        {
            //carregar a lista com os clientes (campo de seleção)
            var listagemDeClientes = new List<SelectListItem>();
            //percorrer todos os clientes obtidos no banco de dados
            foreach (var item in clienteRepository.GetAll())
            {
                //criando 1 item do campo de seleção
                var opcao = new SelectListItem();
                opcao.Value = item.IdCliente.ToString();
                opcao.Text = item.Nome;
                listagemDeClientes.Add(opcao); //adicionando
            }
            return listagemDeClientes;
        }
        private DependenteEdicaoModel GetDependenteEdicaoModel(ClientRepository clienteRepository)
        {
            var model = new DependenteEdicaoModel();
            try
            {
                model.ListamgeDeClientes = GetClientes(clienteRepository);
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Error :" + e.Message;
            }
            return model;
        }
        private DependenteCadastroModel GetDependenteCadastroModel(ClientRepository clienteRepository)
        {
            var model = new DependenteCadastroModel();
            try
            {
                model.ListagemDeClientes = GetClientes(clienteRepository);
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Erro: " + e.Message;
            }

            return model;
        }
    }
}
