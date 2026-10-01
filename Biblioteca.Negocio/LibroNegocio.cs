using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Biblioteca.Negocio.Interfaces;

namespace Biblioteca.Negocio
{
    public class LibroNegocio
    {
        private readonly ILibroRepositorio _repositorio;

        public LibroNegocio(ILibroRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<Libro>> ListarAsync()
        {
            try
            {
                return await _repositorio.ListarAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task<List<Libro>> BuscarAsync(string texto)
        {
            try
            {
                return await _repositorio.BuscarAsync(texto);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task<int> InsertarAsync(Libro libro)
        {
            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new ReglaNegocioException("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(libro.ISBN))
                throw new ReglaNegocioException("El ISBN es obligatorio.");

            if (libro.Ejemplares < 0)
                throw new ReglaNegocioException("La cantidad de ejemplares no puede ser negativa.");

            try
            {
                return await _repositorio.InsertarAsync(libro);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task ActualizarAsync(Libro libro)
        {
            if (libro.LibroId <= 0)
                throw new ReglaNegocioException("LibroId inválido.");

            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new ReglaNegocioException("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(libro.ISBN))
                throw new ReglaNegocioException("El ISBN es obligatorio.");

            if (libro.Ejemplares < 0)
                throw new ReglaNegocioException("La cantidad de ejemplares no puede ser negativa.");

            try
            {
                await _repositorio.ActualizarAsync(libro);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task EliminarAsync(int libroId)
        {
            if (libroId <= 0)
                throw new ReglaNegocioException("LibroId inválido.");

            try
            {
                await _repositorio.EliminarAsync(libroId);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task<List<Libro>> ListarDisponiblesAsync()
        {
            try
            {
                return await _repositorio.ListarDisponiblesAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }
    }
}
