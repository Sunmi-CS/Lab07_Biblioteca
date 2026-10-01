using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca.Entidades;
using Biblioteca.Negocio.Interfaces;

namespace Biblioteca.Negocio.Adapters
{
    public class LibroRepositorioAdapter : ILibroRepositorio
    {
        private readonly Biblioteca.Datos.LibrosRepositorio _repo;

        public LibroRepositorioAdapter(Biblioteca.Datos.LibrosRepositorio repo)
        {
            _repo = repo;
        }

        public Task<int> InsertarAsync(Libro libro) => _repo.InsertarAsync(libro);

        public Task ActualizarAsync(Libro libro) => _repo.ActualizarAsync(libro);

        public Task EliminarAsync(int libroId) => _repo.EliminarAsync(libroId);

        public Task<List<Libro>> ListarAsync() => _repo.ListarAsync();

        public Task<List<Libro>> BuscarAsync(string texto) => _repo.BuscarAsync(texto);

        public Task<List<Libro>> ListarDisponiblesAsync() => _repo.ListarDisponiblesAsync();
    }
}
