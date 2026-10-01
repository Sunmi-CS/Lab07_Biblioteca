using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Datos
{
    public class PrestamosRepositorio
    {
        private string CadenaConexion =>
            ConfigurationManager.ConnectionStrings["CadenaBiblioteca"]?.ConnectionString
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'CadenaBiblioteca'.");

        public async Task<int> RegistrarPrestamoAsync(int socioId, DateTime fechaPrestamo, DateTime fechaLimite, IEnumerable<int> libroIds)
        {
            var lista = string.Join(',', libroIds);

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Prestamos_Registrar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SocioId", socioId);
            cmd.Parameters.AddWithValue("@FechaPrestamo", fechaPrestamo.Date);
            cmd.Parameters.AddWithValue("@FechaLimite", fechaLimite.Date);
            cmd.Parameters.AddWithValue("@LibroIds", lista);

            await cn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<Prestamo>> ListarAsync()
        {
            var lista = new List<Prestamo>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Prestamos_Listar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var p = new Prestamo
                {
                    PrestamoId = reader.GetInt32(reader.GetOrdinal("PrestamoId")),
                    SocioId = reader.GetInt32(reader.GetOrdinal("SocioId")),
                    NombreSocio = reader.GetString(reader.GetOrdinal("NombreSocio")),
                    FechaPrestamo = reader.GetDateTime(reader.GetOrdinal("FechaPrestamo")),
                    FechaLimite = reader.GetDateTime(reader.GetOrdinal("FechaLimite")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado"))
                };

                lista.Add(p);
            }

            return lista;
        }

        public async Task<List<DetallePrestamo>> ObtenerDetalleAsync(int prestamoId)
        {
            var lista = new List<DetallePrestamo>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Prestamos_Detalle", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PrestamoId", prestamoId);

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var d = new DetallePrestamo
                {
                    PrestamoId = reader.GetInt32(reader.GetOrdinal("PrestamoId")),
                    LibroId = reader.GetInt32(reader.GetOrdinal("LibroId")),
                    TituloLibro = reader.GetString(reader.GetOrdinal("TituloLibro")),
                    FechaDevolucion = reader.IsDBNull(reader.GetOrdinal("FechaDevolucion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDevolucion"))
                };

                lista.Add(d);
            }

            return lista;
        }

        public async Task<DevolucionResult> DevolverLibroAsync(int prestamoId, int libroId, DateTime fechaDevolucion)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Prestamos_DevolverLibro", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PrestamoId", prestamoId);
            cmd.Parameters.AddWithValue("@LibroId", libroId);
            cmd.Parameters.AddWithValue("@FechaDevolucion", fechaDevolucion.Date);

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var res = new DevolucionResult
                {
                    PrestamoId = reader.GetInt32(reader.GetOrdinal("PrestamoId")),
                    LibroId = reader.GetInt32(reader.GetOrdinal("LibroId")),
                    FechaDevolucion = reader.GetDateTime(reader.GetOrdinal("FechaDevolucion")),
                    DiasRetraso = reader.GetInt32(reader.GetOrdinal("DiasRetraso")),
                    Multa = reader.GetDecimal(reader.GetOrdinal("Multa"))
                };

                return res;
            }

            throw new InvalidOperationException("No se devolvió información desde el procedimiento.");
        }

        public class DevolucionResult
        {
            public int PrestamoId { get; set; }
            public int LibroId { get; set; }
            public DateTime FechaDevolucion { get; set; }
            public int DiasRetraso { get; set; }
            public decimal Multa { get; set; }
        }
    }
}
