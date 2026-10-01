using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Datos
{
    public class LibrosRepositorio
    {
        private string CadenaConexion =>
            ConfigurationManager.ConnectionStrings["CadenaBiblioteca"]?.ConnectionString
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'CadenaBiblioteca'.");

        public async Task<List<Libro>> ListarAsync()
        {
            var lista = new List<Libro>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Libros_Listar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var l = new Libro
                {
                    LibroId = reader.GetInt32(reader.GetOrdinal("LibroId")),
                    Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                    ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                    AutorId = reader.GetInt32(reader.GetOrdinal("AutorId")),
                    NombreAutor = reader.GetString(reader.GetOrdinal("NombreAutor")),
                    Ejemplares = reader.GetInt32(reader.GetOrdinal("Ejemplares")),
                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                };

                lista.Add(l);
            }

            return lista;
        }

        public async Task<List<Libro>> BuscarAsync(string texto)
        {
            var lista = new List<Libro>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Libros_Buscar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Texto", texto ?? string.Empty);

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var l = new Libro
                {
                    LibroId = reader.GetInt32(reader.GetOrdinal("LibroId")),
                    Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                    ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                    AutorId = reader.GetInt32(reader.GetOrdinal("AutorId")),
                    NombreAutor = reader.GetString(reader.GetOrdinal("NombreAutor")),
                    Ejemplares = reader.GetInt32(reader.GetOrdinal("Ejemplares")),
                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                };

                lista.Add(l);
            }

            return lista;
        }

        public async Task<int> InsertarAsync(Libro libro)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Libros_Insertar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Titulo", libro.Titulo);
            cmd.Parameters.AddWithValue("@ISBN", libro.ISBN);
            cmd.Parameters.AddWithValue("@AutorId", libro.AutorId);
            cmd.Parameters.AddWithValue("@Ejemplares", libro.Ejemplares);

            await cn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task ActualizarAsync(Libro libro)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Libros_Actualizar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@LibroId", libro.LibroId);
            cmd.Parameters.AddWithValue("@Titulo", libro.Titulo);
            cmd.Parameters.AddWithValue("@ISBN", libro.ISBN);
            cmd.Parameters.AddWithValue("@AutorId", libro.AutorId);
            cmd.Parameters.AddWithValue("@Ejemplares", libro.Ejemplares);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int libroId)
        {
            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Libros_Eliminar", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@LibroId", libroId);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Libro>> ListarDisponiblesAsync()
        {
            var lista = new List<Libro>();

            await using var cn = new SqlConnection(CadenaConexion);
            await using var cmd = new SqlCommand("sp_Libros_Disponibles", cn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            await cn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var l = new Libro
                {
                    LibroId = reader.GetInt32(reader.GetOrdinal("LibroId")),
                    Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                    ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                    AutorId = reader.GetInt32(reader.GetOrdinal("AutorId")),
                    NombreAutor = reader.GetString(reader.GetOrdinal("NombreAutor")),
                    Ejemplares = reader.GetInt32(reader.GetOrdinal("Ejemplares")),
                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                };

                lista.Add(l);
            }

            return lista;
        }
    }
}
