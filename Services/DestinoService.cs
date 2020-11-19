using System.Collections.Generic;
using System.Text.Json;
using Repository;

namespace MVC.Servicios
{
    public interface IDestinoService
    {
        List<DestinoDTO> GetAll();
        void Register(DestinoViewModel clienteViewModel, string name);
        DestinoViewModel GetCliente(int id);
        void Edit(DestinoViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
    }
    public class DestinoService : IDestinoService
    {
        private readonly IDestinoRepository repository;

        public DestinoService(IDestinoRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id, string usuarioModificacion)
        {
           repository.Delete(id, usuarioModificacion);
        }

        public void Edit(DestinoViewModel clienteViewModel, string name)
        {
            repository.Edit(clienteViewModel, name);
        }

        public List<DestinoDTO> GetAll()
        {
            return repository.GetAll();
        }

        public DestinoViewModel GetCliente(int id)
        {
            var clienteJson = JsonSerializer.Serialize(repository.getDestino(id));
            return JsonSerializer.Deserialize<DestinoViewModel>(clienteJson);
        }

        public void Register(DestinoViewModel clienteViewModel, string usuarioModificacion)
        {
            repository.Register(clienteViewModel, usuarioModificacion);
        }
    }
}