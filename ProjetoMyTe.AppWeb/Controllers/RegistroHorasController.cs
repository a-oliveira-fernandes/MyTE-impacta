﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoMyTe.AppWeb.Models.Entities;
using ProjetoMyTe.AppWeb.Services;


namespace ProjetoMyTe.AppWeb.Controllers
{

    public class RegistroHorasController : Controller
    {
        private readonly RegistroHorasService registroHorasService;
        private readonly ColaboradoresService colaboradoresService;
        private readonly WbssService wbssService;

        public RegistroHorasController(RegistroHorasService registroHorasService, ColaboradoresService colaboradoresService, WbssService wbssService)
        {
            this.registroHorasService = registroHorasService;
            this.colaboradoresService = colaboradoresService;
            this.wbssService = wbssService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListarRegistros()
        {
            try
            {
                var lista = registroHorasService.Listar();
                return View(lista);
            }
            catch (Exception ex)
            {

                return View("_Erro", ex);
            }
        }

        [HttpGet]
        public IActionResult IncluirRegistro()
        {
            ViewBag.ListaDeWbss = new SelectList(wbssService.Listar(), "Id", "Descricao");
            ViewBag.ListaDeColaboradores = new SelectList(colaboradoresService.Listar(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult IncluirRegistro(RegistroHoras registroHoras)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                registroHoras.DataRegistro = DateTime.Now;
                registroHorasService.Incluir(registroHoras);
                return RedirectToAction("ListarRegistros");
            }
            catch (Exception e)
            {

                return View("_Erro", e);
            }
        }

        [HttpGet]
        public IActionResult AlterarRegistro(int id)
        {
            try
            {
                ViewBag.ListaDeWbss = new SelectList(wbssService.Listar(), "Id", "Descricao");
                if (id <= 0)
                {
                    throw new
                        ArgumentException($"O valor informado na URL ({id}) é inválido");
                }

                RegistroHoras? registroHoras = registroHorasService.Buscar(id);
                if (registroHoras == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id: {id}");
                }
                return View(registroHoras);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpPost]
        public IActionResult AlterarRegistro(RegistroHoras registroHoras)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                registroHoras.DataRegistro = DateTime.Now;
                registroHorasService.Alterar(registroHoras);
                return RedirectToAction("ListarRegistros"); // Requisição get
            }
            catch (Exception e)
            {

                return View("_Erro", e);
            }
        }

        [HttpGet]
        public IActionResult RemoverRegistro(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new
                        ArgumentException($"O valor informado na URL ({id}) é inválido");
                }

                RegistroHoras? registroHoras = registroHorasService.Buscar(id);
                if (registroHoras == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id: {id}");
                }
                return View(registroHoras);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpPost]
        public IActionResult RemoverRegistro(RegistroHoras registroHoras)
        {
            try
            {
                registroHorasService.Remover(registroHoras);
                return RedirectToAction("ListarRegistros");
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }
    }
}