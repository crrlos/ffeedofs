using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Repository
{

    public interface IClienteRepository
    {
        List<ClienteDTO> GetAll();
        void Register(ClienteViewModel clienteViewModel, string usuarioModificacion);
        ClienteDTO getCliente(int id);
        void Edit(ClienteViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
    }
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConfiguration configuration;
        public ClienteRepository(IConfiguration configuation)
        {
            this.configuration = configuation;
        }

        public void Delete(int id, string usuarioModificacion)
        {
           using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
               
                command.CommandText = @"update  clientes set 
                            
                            usuarioModificacion = @usuarioModificacion, 
                            fechaModificacion = @fechaModificacion
                            where id = @id;
                            
                            delete from clientes where id = @id

                            ";

                
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@id",id);

                command.ExecuteNonQuery();

                
            }
        }

        public void Edit(ClienteViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"update  clientes set 
                            codigo = @codigo, 
                            nombre = @nombre, 
                            apellidos = @apellidos, 
                            usuarioModificacion = @usuarioModificacion, 
                            fechaModificacion = @fechaModificacion
                            where id = @id";

                command.Parameters.AddWithValue("@codigo", clienteViewModel.Codigo);
                command.Parameters.AddWithValue("@nombre", clienteViewModel.Nombre);
                command.Parameters.AddWithValue("@apellidos", clienteViewModel.Apellidos);
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@id", clienteViewModel.Id);

                command.ExecuteNonQuery();

            }
        }

        public List<ClienteDTO> GetAll()
        {
            List<ClienteDTO> clientes = new List<ClienteDTO>();
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre, apellidos from Clientes";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new ClienteDTO
                    {
                        Id = (int)reader["id"],
                        Codigo = (string)reader["codigo"],
                        Nombre = (string)reader["nombre"],
                        Apellidos = (string)reader["apellidos"]
                    });
                }

            }
            return clientes;
        }

        public ClienteDTO getCliente(int id)
        {
            ClienteDTO cliente = null;
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre, apellidos from Clientes where id = @id";
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente = new ClienteDTO
                    {
                        Id = (int)reader["id"],
                        Codigo = (string)reader["codigo"],
                        Nombre = (string)reader["nombre"],
                        Apellidos = (string)reader["apellidos"]
                    };
                }

            }
            return cliente;
        }

        public void Register(ClienteViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"insert into clientes 
                            values(@codigo, @nombre, @apellidos, @usuarioModificacion, @fechaModificacion)";

                command.Parameters.AddWithValue("@codigo", clienteViewModel.Codigo);
                command.Parameters.AddWithValue("@nombre", clienteViewModel.Nombre);
                command.Parameters.AddWithValue("@apellidos", clienteViewModel.Apellidos);
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);

                command.ExecuteNonQuery();

            }
        }
    }
}