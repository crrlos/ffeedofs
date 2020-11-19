using System.Collections.Generic;
using System.Text.Json;
using Repository;

namespace MVC.Servicios
{
    public interface ITipoCreditoService
    {
        List<TipoCreditoDTO> GetAll();
        void Register(TipoCreditoViewModel clienteViewModel, string name);
        TipoCreditoViewModel GetCliente(int id);
        void Edit(TipoCreditoViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
    }
    public class TipoCreditoService : ITipoCreditoService
    {
        private readonly ITipoCreditoRepository repository;

        public TipoCreditoService(ITipoCreditoRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id, string usuarioModificacion)
        {
           repository.Delete(id, usuarioModificacion);
        }

        public void Edit(TipoCreditoViewModel clienteViewModel, string name)
        {
            repository.Edit(clienteViewModel, name);
        }

        public List<TipoCreditoDTO> GetAll()
        {
            return repository.GetAll();
        }

        public TipoCreditoViewModel GetCliente(int id)
        {
            var clienteJson = JsonSerializer.Serialize(repository.getTipoCredito(id));
            return JsonSerializer.Deserialize<TipoCreditoViewModel>(clienteJson);
        }

        public void Register(TipoCreditoViewModel clienteViewModel, string usuarioModificacion)
        {
            repository.Register(clienteViewModel, usuarioModificacion);
        }
    }
}