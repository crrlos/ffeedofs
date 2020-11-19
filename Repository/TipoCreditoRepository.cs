using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Repository
{

    public interface ITipoCreditoRepository
    {
        List<TipoCreditoDTO> GetAll();
        void Register(TipoCreditoViewModel clienteViewModel, string usuarioModificacion);
        TipoCreditoDTO getTipoCredito(int id);
        void Edit(TipoCreditoViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
    }
    public class TipoCreditoRepository : ITipoCreditoRepository
    {
        private readonly IConfiguration configuration;
        public TipoCreditoRepository(IConfiguration configuation)
        {
            this.configuration = configuation;
        }

        public void Delete(int id, string usuarioModificacion)
        {
           using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
               
                command.CommandText = @"update  tiposcreditos set 
                            
                            usuarioModificacion = @usuarioModificacion, 
                            fechaModificacion = @fechaModificacion
                            where id = @id;
                            
                            delete from tiposcreditos where id = @id

                            ";

                
                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@id",id);

                command.ExecuteNonQuery();

                
            }
        }

        public void Edit(TipoCreditoViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"update  tiposcreditos set 
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

        public List<TipoCreditoDTO> GetAll()
        {
            List<TipoCreditoDTO> clientes = new List<TipoCreditoDTO>();
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre  from tiposcreditos";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new TipoCreditoDTO
                    {
                        Id = (int)reader["id"],
                        Codigo = (string)reader["codigo"],
                        Nombre = (string)reader["nombre"]
                    });
                }

            }
            return clientes;
        }

        public TipoCreditoDTO getTipoCredito(int id)
        {
            TipoCreditoDTO cliente = null;
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre from tiposcreditos where id = @id";
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente = new TipoCreditoDTO
                    {
                        Id = (int)reader["id"],
                        Codigo = (string)reader["codigo"],
                        Nombre = (string)reader["nombre"]
                    };
                }

            }
            return cliente;
        }

        public void Register(TipoCreditoViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"insert into tiposcreditos 
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