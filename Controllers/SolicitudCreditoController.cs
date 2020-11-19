using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Servicios;

namespace MVC.Controllers
{
    [Authorize]
    [ResponseCache(CacheProfileName = "0")]
    public class SolicitudCreditoController : Controller
    {

        private readonly ISolicitudCreditoService service;
        public SolicitudCreditoController(ISolicitudCreditoService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            var solicitudes = service.GetAll();
            return View(solicitudes);
        }

        public ActionResult Register()
        {
            var solicitudViewModel = service.getSolicitudViewModel();
            return View(solicitudViewModel);
        }

        [HttpPost]
        public ActionResult Register(SolicitudCreditoViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(service.getSolicitudViewModel());
            }
            
            int r = service.Register(clienteViewModel, this.User.Identity.Name);
            service.GenerarTablaAmortizacion(clienteViewModel,r);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Tabla(int id){
            var registros = service.getTabla(id);
            return View(registros);
        }

        public ActionResult Edit(int id)
        {
            var cliente = service.GetCliente(id);
            return View(cliente);
        }

        [HttpPost]
        public ActionResult Edit(ClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(clienteViewModel);
            }
            //service.Edit(clienteViewModel, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id){
           
            return View(new ClienteViewModel{Id = id});
        }
        
       [HttpPost]
        public ActionResult Delete(ClienteViewModel model){
            service.Delete(model.Id, this.User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }


    }
}