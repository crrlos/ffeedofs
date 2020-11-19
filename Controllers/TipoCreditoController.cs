using Microsoft.AspNetCore.Mvc;
using MVC.Servicios;

namespace MVC.Controllers
{
    public class TipoCreditoController : Controller
    {

        private readonly ITipoCreditoService service;
        public TipoCreditoController(ITipoCreditoService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            var clientes = service.GetAll();
            return View(clientes);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TipoCreditoViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(clienteViewModel);
            }
            service.Register(clienteViewModel, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var cliente = service.GetCliente(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult Edit(TipoCreditoViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(clienteViewModel);
            }
            service.Edit(clienteViewModel, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id){
           
            return View(new TipoCreditoViewModel{Id = id});
        }
        
       [HttpPost]
        public ActionResult Delete(TipoCreditoViewModel model){
            service.Delete(model.Id, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }


    }
}