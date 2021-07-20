using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projetc.Presentation.Mvc.Models;
using Project.Repository.Repositories;
using Project.Repository.Entities;

namespace Projetc.Presentation.Mvc.Controllers
{
    public class ClienteController : Controller
    {
        //abrir a pagina
        public IActionResult Cadastro()
        {
            return View();
        }
        [HttpPost] //método recebe o SUMIT do formúlario (envio de dados)
        public IActionResult Cadastro(ClienteCadastroModel model,
            [FromServices] ClientRepository clienteRepository)
        {
            //verificando se todos os comapos da model passaram nas validações
            if (ModelState.IsValid) //decoreba, sempre vai ter esse campo para a validação
            {
                try
                {
                    //verificar se o email do cliente já encontra-se cadastrado
                    if (clienteRepository.GetByEmail(model.Email) != null)
                    {
                        throw new Exception("O email informado já encontra - se cadastrado.Tente outro.");
                    }
                    //verificar se o cpf do cliente já encontra-se cadastrado
                    else if (clienteRepository.GetByCpf(model.Cpf) != null)
                    {
                        throw new Exception("O CPF informado já encontra - se cadastrado.Tente outro.");
                    }

                    //criando um objeto da classe de entidade
                    var cliente = new Cliente();
                    cliente.Nome = model.Nome;
                    cliente.Email = model.Email;
                    cliente.Cpf = model.Cpf;

                    clienteRepository.Create(cliente); //gravando no banco de dados

                    //exibir mensagem de sucesso
                    TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso.";

                    ModelState.Clear(); //limpar os campos do formulário
                }
                catch (Exception e)
                {
                    //exibir mensagem de erro
                    TempData["MensagemErro"] = "Error: " + e.Message;
                }
            }
            return View();
        }
        //abrir a pagina metodo de consulta
        public IActionResult Consulta(
            [FromServices] ClientRepository clientRepository)
        {
            //declarando uma lista de clientes
            var clientes = new List<Cliente>();
            try
            {
                //consultar os clientes da base de dados
                clientes = clientRepository.GetAll();
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Erro: " + e.Message;
            }
            //enviando os clientes para a página...
            return View(clientes);
        }
        [HttpPost]
        public IActionResult Consulta(string nome, [FromServices] ClientRepository clientRepository)
        {
            var clientes = new List<Cliente>();
            try
            {
                clientes = clientRepository.GetByNome(nome);
                TempData["Nome"] = nome;
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Error: " + e.Message;
            }
            return View(clientes);
        }
        //metodo de ação
        public IActionResult Exclusao(int id, [FromServices] ClientRepository clienteRepository)
        {
            try
            {
                //buscando os dados do cliente pelo id
                var cliente = clienteRepository.GetById(id);
                //verificar se o cliente foi obitido no bancod de dados
                if (cliente != null)
                {
                    //verificar se o cliente possui dependentes
                    var quantidadeDependentes = clienteRepository.CountDependentes(cliente.IdCliente);
                    if(quantidadeDependentes == 0)
                    {
                    //excluindo o cliente
                    clienteRepository.Delete(cliente);
                    TempData["MensagemSucesso"] = "Cliente excluído com sucesso.";
                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Não é possivel exlcuir o cliente {cliente.Nome} pois ele possui {quantidadeDependentes} dependente(s).";
                    }
                }
                else
                {
                    throw new Exception("Cliente não encontrado.");
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Erro: " + e.Message;
            }
            //redirecionar de volta para a página de consulta
            return RedirectToAction("Consulta");
        }

        public IActionResult Edicao(int id, [FromServices] ClientRepository clienteRepository)
        {
            //criando um objeto da classe model
            var model = new ClienteEdicaoModel();

            try
            {
                //buscando o cliente no banco de dados pelo id
                var cliente = clienteRepository.GetById(id);
                //transferir os dados do cliente para a model
                model.IdCliente = cliente.IdCliente;
                model.Nome = cliente.Nome;
                model.Email = cliente.Email;
                model.Cpf = cliente.Cpf;

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Erro: " + e.Message;
            }
            return View(model); //abrir uma pagina
        }

        [HttpPost] //método recebe o SUBMIT do formulário
        public IActionResult Edicao(ClienteEdicaoModel model, [FromServices] ClientRepository clienteRepository)
        {
            //verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = new Cliente();
                    cliente.IdCliente = model.IdCliente;
                    cliente.Nome = model.Nome;
                    cliente.Email = model.Email;
                    cliente.Cpf = model.Cpf;

                    //atualizando no banco de dados
                    clienteRepository.Update(cliente);
                    TempData["MensagemSucesso"] = $"Cliente {cliente.Nome} atualizado com sucesso.";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Erro: " + e.Message;
                }
            }
            return RedirectToAction("Consulta");
        }

    }

}
