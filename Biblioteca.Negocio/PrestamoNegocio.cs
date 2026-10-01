using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca.Datos;
using Biblioteca.Entidades;

namespace Biblioteca.Negocio
{
    public class PrestamoNegocio
    {
        private readonly PrestamosRepositorio _repositorio = new PrestamosRepositorio();
        private readonly SociosRepositorio _sociosRepo = new SociosRepositorio();

        public async Task<List<Prestamo>> ListarAsync()
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

        public async Task<List<DetallePrestamo>> ObtenerDetalleAsync(int prestamoId)
        {
            try
            {
                return await _repositorio.ObtenerDetalleAsync(prestamoId);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task<int> RegistrarPrestamoAsync(int socioId, DateTime fechaPrestamo, DateTime fechaLimite, IEnumerable<int> libroIds)
        {
            if (socioId <= 0)
                throw new ReglaNegocioException("SocioId inválido.");

            if (fechaLimite < fechaPrestamo)
                throw new ReglaNegocioException("La fecha límite no puede ser anterior a la fecha del préstamo.");

            // Validar que el socio existe y está activo
            var socios = await _sociosRepo.ListarAsync();
            var existe = socios.Exists(s => s.SocioId == socioId && s.Activo);
            if (!existe)
                throw new ReglaNegocioException("El socio no existe o está inactivo.");

            try
            {
                return await _repositorio.RegistrarPrestamoAsync(socioId, fechaPrestamo, fechaLimite, libroIds);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }

        public async Task<PrestamosRepositorio.DevolucionResult> DevolverLibroAsync(int prestamoId, int libroId, DateTime fechaDevolucion)
        {
            if (prestamoId <= 0 || libroId <= 0)
                throw new ReglaNegocioException("PrestamoId o LibroId inválido.");

            try
            {
                return await _repositorio.DevolverLibroAsync(prestamoId, libroId, fechaDevolucion);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }
        }
    }
}
