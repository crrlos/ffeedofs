using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MVC.Servicios;

namespace Repository
{

    public interface ISolicitudCreditoRepository
    {
        List<SolicitudCreditoDTO> GetAll();
        int Register(SolicitudCreditoViewModel clienteViewModel, string usuarioModificacion);
        SolicitudCreditoDTO getSolicitudCredito(int id);
        void Edit(SolicitudCreditoViewModel clienteViewModel, string name);
        void Delete(int id, string usuarioModificacion);
        void GuardarTabla(List<Registro> registros, int id);
        List<Registro> getTabla(int id);
    }
    public class SolicitudCreditoRepository : ISolicitudCreditoRepository
    {
        private readonly IConfiguration configuration;
        public SolicitudCreditoRepository(IConfiguration configuation)
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
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();


            }
        }

        public void Edit(SolicitudCreditoViewModel clienteViewModel, string usuarioModificacion)
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


                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);


                command.ExecuteNonQuery();

            }
        }

        public List<SolicitudCreditoDTO> GetAll()
        {
            List<SolicitudCreditoDTO> clientes = new List<SolicitudCreditoDTO>();
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"Select s.id, c.nombre  from solicitudCredito s
                join clientes c on c.id = s.clienteid";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new SolicitudCreditoDTO
                    {
                        Id = (int)reader["id"],
                        Cliente = (string)reader["nombre"]
                    });
                }

            }
            return clientes;
        }

        public SolicitudCreditoDTO getSolicitudCredito(int id)
        {
            SolicitudCreditoDTO cliente = null;
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select id, codigo, nombre from tiposcreditos where id = @id";
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente = new SolicitudCreditoDTO
                    {
                        Id = (int)reader["id"],
                        // Codigo = (string)reader["codigo"],
                        // Nombre = (string)reader["nombre"]
                    };
                }

            }
            return cliente;
        }

        public List<Registro> getTabla(int id)
        {

            List<Registro> registros = new List<Registro>();
             using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                
                command.CommandText = "select * from amortizacion where solicitudcreditoid = @id";
                command.Parameters.AddWithValue("@id",id);
                var reader = command.ExecuteReader();

                while(reader.Read()){
                        registros.Add(new Registro{
                                Periodo = (int)reader["periodo"],
                                Cuota = (double)(decimal)reader["cuota"],
                                Amortizacion = (double)(decimal) reader["amortizacion"],
                                Interes = (double)(decimal) reader["interes"],
                                Saldo = (double)(decimal) reader["saldo"]
                        });
                }


            }
            return registros;

        }

        public void GuardarTabla(List<Registro> registros, int id)
        {



            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                
                command.CommandText = "select top 1 id from solicitudcredito order by id desc";
                int d = (int) command.ExecuteScalar();

                registros.ForEach(r =>
                {
                    command = con.CreateCommand();
                    command.CommandText = @"insert into amortizacion 
                            values(@solicitudCreditoid, @periodo, @cuota, @interes, @amortizacion, @saldo)";
                    command.Parameters.AddWithValue("@solicitudcreditoid", d);
                    command.Parameters.AddWithValue("@periodo", r.Periodo);
                    command.Parameters.AddWithValue("@cuota", r.Cuota);
                    command.Parameters.AddWithValue("@interes", r.Interes);
                    command.Parameters.AddWithValue("@amortizacion", r.Amortizacion);
                    command.Parameters.AddWithValue("@saldo", r.Saldo);

                    command.ExecuteNonQuery();

                });








            }

        }

        public int Register(SolicitudCreditoViewModel clienteViewModel, string usuarioModificacion)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("CLIN")))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = @"insert into solicitudcredito 
                            values(@clienteId, @ingresos, @egresos, @montoSolicitado, @plazo, @tasa, @destinoId, 
                            @tipoCreditoId,@usuarioModificacion, @fechaModificacion)";

                command.Parameters.AddWithValue("@clienteId", clienteViewModel.ClienteId);
                command.Parameters.AddWithValue("@ingresos", clienteViewModel.Ingresos);
                command.Parameters.AddWithValue("@egresos", clienteViewModel.Egresos);
                command.Parameters.AddWithValue("@montoSolicitado", clienteViewModel.MontoSolicitado);
                command.Parameters.AddWithValue("@plazo", clienteViewModel.Plazo);
                command.Parameters.AddWithValue("@tasa", clienteViewModel.Tasa);
                command.Parameters.AddWithValue("@destinoId", clienteViewModel.DestinoId);
                command.Parameters.AddWithValue("@tipoCreditoId", clienteViewModel.TipoCreditoId);

                command.Parameters.AddWithValue("@usuarioModificacion", usuarioModificacion);
                command.Parameters.AddWithValue("@fechaModificacion", DateTime.Now);

                return Convert.ToInt32(command.ExecuteScalar());

            }
        }
    }
}