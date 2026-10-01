using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca.Entidades;

namespace Biblioteca.Negocio.Interfaces
{
    public interface ILibroRepositorio
    {
        Task<List<Libro>> ListarAsync();
        Task<List<Libro>> BuscarAsync(string texto);
        Task<int> InsertarAsync(Libro libro);
        Task ActualizarAsync(Libro libro);
        Task EliminarAsync(int libroId);
        Task<List<Libro>> ListarDisponiblesAsync();
    }
}
