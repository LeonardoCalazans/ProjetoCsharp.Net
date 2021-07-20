using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Projeto.Infra.Data.Entities;
using Projeto.Infra.Data.Repositories;
using Projeto.Presentation.Mvc.Models;

namespace Projeto.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        //método que abre a página inicialmente
        public IActionResult Login()
        { 
            return View();
        }

        //método que abre a página inicialmente
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost] //método executado pelo SUBMIT do formulário
        public IActionResult Login(AccountLoginModel model, 
            [FromServices] UsuarioRepository usuarioRepository)
        {
            //verificar se todos os campos foram validados com sucesso
            if(ModelState.IsValid)
            {
                try
                {
                    //buscar o usuario no banco de dados pelo email e senha
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, model.Senha);

                    //verificando se o usuario foi encontrado
                    if(usuario != null)
                    {
                        //criando a permissão de acesso do usuário
                        var identity = new ClaimsIdentity(
                            new[] { new Claim(ClaimTypes.Name, usuario.Email) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        //gravar a permissão em um cookie encriptado
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity));

                        //redirecionamento
                        return RedirectToAction("Index", "Agenda");
                    }
                    else
                    {
                        //lançar uma exceção
                        throw new Exception("Usuário inválido. Acesso Negado.");
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = "Erro: " + e.Message;
                }
            }

            return View();
        }

        [HttpPost] //método executado pelo SUBMIT do formulário
        public IActionResult Register(AccountRegisterModel model, 
            [FromServices] UsuarioRepository usuarioRepository)
        {
            //verificar se todos os campos foram validados com sucesso
            if (ModelState.IsValid)
            {
                try
                {
                    //verificar se já existe um usuário com o email informado cadastrado
                    if(usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        //lançar uma exceção
                        throw new Exception("O email informado já encontra-se cadastrado.");
                    }
                    
                    var usuario = new Usuario();
                    usuario.Nome = model.Nome;
                    usuario.Email = model.Email;
                    usuario.Senha = model.Senha;
                    usuario.DataCriacao = DateTime.Now;

                    usuarioRepository.Create(usuario);

                    TempData["MensagemSucesso"] = $"Usuário {usuario.Nome}, cadastrado com sucesso.";
                    ModelState.Clear();
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = "Erro: " + e.Message;
                }
            }

            return View();
        }

        public IActionResult Logout()
        {
            //destruir o cookie contendo a permissão do usuário
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar para a página de login
            return RedirectToAction("Login");
        }
    }
}