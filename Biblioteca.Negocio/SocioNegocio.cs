using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Biblioteca.Datos;

namespace Biblioteca.Negocio
{
    public class SocioNegocio
    {
        private readonly SociosRepositorio _repositorio = new SociosRepositorio();

        public async Task<List<Socio>> ListarAsync()
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

        public async Task<List<Socio>> BuscarAsync(string texto)
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

        public async Task<int> InsertarAsync(Socio socio)
        {
            if (string.IsNullOrWhiteSpace(socio.DNI))
                throw new ReglaNegocioException("El DNI es obligatorio.");

            if (string.IsNullOrWhiteSpace(socio.Nombre))
                throw new ReglaNegocioException("El nombre es obligatorio.");

            try
            {
                return await _repositorio.InsertarAsync(socio);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task ActualizarAsync(Socio socio)
        {
            if (socio.SocioId <= 0)
                throw new ReglaNegocioException("SocioId inválido.");

            try
            {
                await _repositorio.ActualizarAsync(socio);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task EliminarAsync(int socioId)
        {
            if (socioId <= 0)
                throw new ReglaNegocioException("SocioId inválido.");

            try
            {
                await _repositorio.EliminarAsync(socioId);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }
    }
}
