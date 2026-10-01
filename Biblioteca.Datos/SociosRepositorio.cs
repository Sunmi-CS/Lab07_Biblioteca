using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Datos
{
    public class SociosRepositorio
    {
        private string CadenaConexion =>
            ConfigurationManager.ConnectionStrings["CadenaBiblioteca"]?.ConnectionString
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'CadenaBiblioteca'.");

        public async Task<List<Socio>> ListarAsync()
        {
            var lista = new List<Socio>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Socios_Listar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var s = new Socio
                {
                    SocioId = reader.GetInt32(reader.GetOrdinal("SocioId")),
                    DNI = reader.GetString(reader.GetOrdinal("DNI")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                };

                lista.Add(s);
            }

            return lista;
        }

        public async Task<List<Socio>> BuscarAsync(string texto)
        {
            var lista = new List<Socio>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Socios_Buscar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Texto", texto ?? string.Empty);

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var s = new Socio
                {
                    SocioId = reader.GetInt32(reader.GetOrdinal("SocioId")),
                    DNI = reader.GetString(reader.GetOrdinal("DNI")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                };

                lista.Add(s);
            }

            return lista;
        }

        public async Task<int> InsertarAsync(Socio socio)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Socios_Insertar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DNI", socio.DNI);
            cmd.Parameters.AddWithValue("@Nombre", socio.Nombre);
            cmd.Parameters.AddWithValue("@Email", socio.Email);

            await cn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task ActualizarAsync(Socio socio)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Socios_Actualizar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SocioId", socio.SocioId);
            cmd.Parameters.AddWithValue("@DNI", socio.DNI);
            cmd.Parameters.AddWithValue("@Nombre", socio.Nombre);
            cmd.Parameters.AddWithValue("@Email", socio.Email);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int socioId)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Socios_Eliminar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SocioId", socioId);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
