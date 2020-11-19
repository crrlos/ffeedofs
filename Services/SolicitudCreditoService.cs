using System.Collections.Generic;
using System.Text.Json;
using Repository;
using System.Linq;
using System;

namespace MVC.Servicios
{
    public interface ISolicitudCreditoService
    {
        List<SolicitudCreditoDTO> GetAll();
        int Register(SolicitudCreditoViewModel clienteViewModel, string name);
        SolicitudCreditoViewModel GetCliente(int id);
        void Edit(SolicitudCreditoViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
        SolicitudCreditoViewModel getSolicitudViewModel();
        void GenerarTablaAmortizacion(SolicitudCreditoViewModel clienteViewModel, int id);
        List<Registro> getTabla(int id);
    }
    public class SolicitudCreditoService : ISolicitudCreditoService
    {
        private readonly ISolicitudCreditoRepository repository;
        private readonly IClienteService clienteService;
        private readonly IDestinoService destinoService;
        private readonly ITipoCreditoService tipoCreditoService;

        public SolicitudCreditoService(ISolicitudCreditoRepository repository,
        IClienteService clienteService, IDestinoService destinoService, ITipoCreditoService tipoCreditoService)
        {
            this.repository = repository;
            this.clienteService = clienteService;
            this.destinoService = destinoService;
            this.tipoCreditoService = tipoCreditoService;
        }

        public void Delete(int id, string usuarioModificacion)
        {
            repository.Delete(id, usuarioModificacion);
        }

        public void Edit(SolicitudCreditoViewModel clienteViewModel, string name)
        {
            repository.Edit(clienteViewModel, name);
        }

        public void GenerarTablaAmortizacion(SolicitudCreditoViewModel clienteViewModel, int id)
        {
            double a = clienteViewModel.MontoSolicitado;
            int n = clienteViewModel.Plazo;
            double i = clienteViewModel.Tasa / 100;

            double r = (a * i) / (1 - Math.Pow(1 + i, -n));

            List<Registro> registros = new List<Registro>();

            registros.Add(new Registro(){
                       Periodo = 0, 
                        Cuota = 0,
                        Saldo = a,
                        Interes = 0,
                        Amortizacion =0
            });
            for (int j = 1; j <= n; j++)
            {
                
                registros.Add(new Registro{
                        Periodo = j, 
                        Cuota = r,
                        Saldo = a - (r - (a * i )),
                        Interes = a * i ,
                        Amortizacion = r - (a * i )
                });
                a = a - (r - (a * i ));
            }

            repository.GuardarTabla(registros, id);
        }

        

        public List<SolicitudCreditoDTO> GetAll()
        {
            return repository.GetAll();
        }

        public SolicitudCreditoViewModel GetCliente(int id)
        {
            var clienteJson = JsonSerializer.Serialize(repository.getSolicitudCredito(id));
            return JsonSerializer.Deserialize<SolicitudCreditoViewModel>(clienteJson);
        }

        public SolicitudCreditoViewModel getSolicitudViewModel()
        {
            var clientes = clienteService.GetAll();
            var destinos = destinoService.GetAll();
            var tiposCreditos = tipoCreditoService.GetAll();

            return new SolicitudCreditoViewModel
            {
                Clientes = clientes,
                Destinos = destinos,
                TiposCreditos = tiposCreditos
            };
        }

        public List<Registro> getTabla(int id)
        {
           return repository.getTabla(id);
        }

        public int Register(SolicitudCreditoViewModel clienteViewModel, string usuarioModificacion)
        {
            return repository.Register(clienteViewModel, usuarioModificacion);
        }
    }
    public class Registro
        {
            public int Periodo { get; set; }
            public double Cuota { get; set; }
            public double Interes { get; set; }
            public double Amortizacion { get; set; }
            public double Saldo { get; set; }

        }
}