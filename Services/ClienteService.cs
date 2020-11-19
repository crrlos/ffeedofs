using System.Collections.Generic;
using System.Text.Json;
using Repository;

namespace MVC.Servicios
{
    public interface IClienteService
    {
        List<ClienteDTO> GetAll();
        void Register(ClienteViewModel clienteViewModel, string name);
        ClienteViewModel GetCliente(int id);
        void Edit(ClienteViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
    }
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository repository;

        public ClienteService(IClienteRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id, string usuarioModificacion)
        {
           repository.Delete(id, usuarioModificacion);
        }

        public void Edit(ClienteViewModel clienteViewModel, string name)
        {
            repository.Edit(clienteViewModel, name);
        }

        public List<ClienteDTO> GetAll()
        {
            return repository.GetAll();
        }

        public ClienteViewModel GetCliente(int id)
        {
            var clienteJson = JsonSerializer.Serialize(repository.getCliente(id));
            return JsonSerializer.Deserialize<ClienteViewModel>(clienteJson);
        }

        public void Register(ClienteViewModel clienteViewModel, string usuarioModificacion)
        {
            repository.Register(clienteViewModel, usuarioModificacion);
        }
    }
}