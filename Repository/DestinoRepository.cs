using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Repository
{

    public interface IDestinoRepository
    {
        List<DestinoDTO> GetAll();
        void Register(DestinoViewModel clienteViewModel, string usuarioModificacion);
        DestinoDTO getDestino(int id);
        void Edit(DestinoViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
    }
    public class DestinoRepository : IDestinoRepository
    {
        private readonly IConfiguration configuration;
        public DestinoRepository(IConfiguration configuation)
        {
            this.configuration = configuation;
        }

        public void Delete(int id, string usuarioModificacion)
        {
           using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
               
                command.CommandText = @"update  destinos set 
                            
                            usuarioModificacion = @usuarioModificacion, 
                            fechaModificacion = @fechaModificacion
                            where id = @id;
                            
                            delete from destinos where id = @id

                            ";

                
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@id",id);

                command.ExecuteNonQuery();

                
            }
        }

        public void Edit(DestinoViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"update  destinos set 
                            codigo = @codigo, 
                            nombre = @nombre, 
                            
                            usuarioModificacion = @usuarioModificacion, 
                            fechaModificacion = @fechaModificacion
                            where id = @id";

                command.Parameters.AddWithValue("@codigo", clienteViewModel.Codigo);
                command.Parameters.AddWithValue("@nombre", clienteViewModel.Nombre);
              
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@id", clienteViewModel.Id);

                command.ExecuteNonQuery();

            }
        }

        public List<DestinoDTO> GetAll()
        {
            List<DestinoDTO> clientes = new List<DestinoDTO>();
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre  from destinos";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new DestinoDTO
                    {
                        Id = (int)reader["id"],
                        Codigo = (string)reader["codigo"],
                        Nombre = (string)reader["nombre"]
                    });
                }

            }
            return clientes;
        }

        public DestinoDTO getDestino(int id)
        {
            DestinoDTO cliente = null;
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre from destinos where id = @id";
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente = new DestinoDTO
                    {
                        Id = (int)reader["id"],
                        Codigo = (string)reader["codigo"],
                        Nombre = (string)reader["nombre"]
                    };
                }

            }
            return cliente;
        }

        public void Register(DestinoViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"insert into destinos 
                            values(@codigo, @nombre, @usuarioModificacion, @fechaModificacion)";

                command.Parameters.AddWithValue("@codigo", clienteViewModel.Codigo);
                command.Parameters.AddWithValue("@nombre", clienteViewModel.Nombre);
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);

                command.ExecuteNonQuery();

            }
        }
    }
}