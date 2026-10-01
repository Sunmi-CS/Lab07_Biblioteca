using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Biblioteca.Negocio;
using Biblioteca.Negocio.Interfaces;
using Biblioteca.Datos;

namespace Biblioteca.WPF.Services
{
    public class NegocioService
    {
        private readonly LibroNegocio _libroNegocio;
        private readonly SocioNegocio _socioNegocio = new SocioNegocio();
        private readonly PrestamoNegocio _prestamoNegocio = new PrestamoNegocio();

        public NegocioService()
        {
            // Inyección manual mínima: crear repositorio de datos para libros y pasarlo al negocio
            var librosRepo = new Biblioteca.Datos.LibrosRepositorio();
            _libroNegocio = new LibroNegocio(new Biblioteca.Negocio.Adapters.LibroRepositorioAdapter(librosRepo));
        }

        public Task<List<Libro>> ListarLibrosAsync() => _libroNegocio.ListarAsync();
        public Task<List<Libro>> BuscarLibrosAsync(string texto) => _libroNegocio.BuscarAsync(texto);
        public Task<List<Libro>> ListarLibrosDisponiblesAsync() => _libroNegocio.ListarDisponiblesAsync();

        public Task<int> InsertarLibroAsync(Libro libro) => _libroNegocio.InsertarAsync(libro);
        public Task ActualizarLibroAsync(Libro libro) => _libroNegocio.ActualizarAsync(libro);
        public Task EliminarLibroAsync(int libroId) => _libroNegocio.EliminarAsync(libroId);

        public Task<int> InsertarSocioAsync(Socio socio) => _socioNegocio.InsertarAsync(socio);
        public Task ActualizarSocioAsync(Socio socio) => _socioNegocio.ActualizarAsync(socio);
        public Task EliminarSocioAsync(int socioId) => _socioNegocio.EliminarAsync(socioId);

        public Task<List<Socio>> ListarSociosAsync() => _socioNegocio.ListarAsync();

        public Task<List<Prestamo>> ListarPrestamosAsync() => _prestamoNegocio.ListarAsync();

        public Task<List<DetallePrestamo>> ObtenerDetalleAsync(int prestamoId) => _prestamoNegocio.ObtenerDetalleAsync(prestamoId);

        public Task<int> RegistrarPrestamoAsync(int socioId, System.DateTime fechaPrestamo, System.DateTime fechaLimite, IEnumerable<int> libroIds)
            => _prestamoNegocio.RegistrarPrestamoAsync(socioId, fechaPrestamo, fechaLimite, libroIds);

        public Task<PrestamosRepositorio.DevolucionResult> DevolverLibroAsync(int prestamoId, int libroId, System.DateTime fechaDevolucion)
            => _prestamoNegocio.DevolverLibroAsync(prestamoId, libroId, fechaDevolucion);
    }
}
