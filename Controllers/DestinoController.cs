using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Servicios;

namespace MVC.Controllers
{
    [Authorize]
     [ResponseCache(CacheProfileName = "0")]
    public class DestinoController : Controller
    {

        private readonly IDestinoService service;
        public DestinoController(IDestinoService service)
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
        public ActionResult Register(DestinoViewModel clienteViewModel)
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
        public ActionResult Edit(DestinoViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(clienteViewModel);
            }
            service.Edit(clienteViewModel, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id){
           
            return View(new DestinoViewModel{Id = id});
        }
        
       [HttpPost]
        public ActionResult Delete(DestinoViewModel model){
            service.Delete(model.Id, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }


    }
}