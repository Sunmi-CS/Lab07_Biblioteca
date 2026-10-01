using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Biblioteca.Entidades;
using Biblioteca.WPF.Services;

namespace Biblioteca.WPF
{
    public partial class MainWindow : Window
    {
        private readonly NegocioService _service = new NegocioService();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await CargarLibrosAsync();
                await CargarSociosAsync();
                await CargarDatosPrestamoAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarLibrosAsync()
        {
            var lista = await _service.ListarLibrosAsync();
            dgLibros.ItemsSource = lista;
        }

        private async Task BuscarLibrosAsync(string texto)
        {
            var lista = await _service.BuscarLibrosAsync(texto);
            dgLibros.ItemsSource = lista;
        }

        private async Task CargarSociosAsync()
        {
            var lista = await _service.ListarSociosAsync();
            dgSocios.ItemsSource = lista;
            dgSociosPrestamo.ItemsSource = lista;
        }

        private async Task CargarDatosPrestamoAsync()
        {
            var disponibles = await _service.ListarLibrosDisponiblesAsync();
            lbLibrosDisponibles.ItemsSource = disponibles;

            var prestamos = await _service.ListarPrestamosAsync();
            dgPrestamos.ItemsSource = prestamos;
        }

        private async void BtnRefrescarLibros_Click(object sender, RoutedEventArgs e)
        {
            await CargarLibrosAsync();
        }

        private async void BtnNuevoLibro_Click(object sender, RoutedEventArgs e)
        {
            var win = new Views.LibroWindow();
            if (win.ShowDialog() == true)
            {
                try
                {
                    var id = await _service.InsertarLibroAsync(win.Libro);
                    MessageBox.Show($"Libro insertado. Id: {id}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarLibrosAsync();
                }
                catch (ReglaNegocioException ex)
                {
                    MessageBox.Show(ex.Message, "Regla de negocio", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnEditarLibro_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgLibros.SelectedItem is Libro l))
            {
                MessageBox.Show("Seleccione un libro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var win = new Views.LibroWindow();
            win.SetLibro(new Libro { LibroId = l.LibroId, Titulo = l.Titulo, ISBN = l.ISBN, AutorId = l.AutorId, Ejemplares = l.Ejemplares });
            if (win.ShowDialog() == true)
            {
                try
                {
                    await _service.ActualizarLibroAsync(win.Libro);
                    MessageBox.Show("Libro actualizado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarLibrosAsync();
                }
                catch (ReglaNegocioException ex)
                {
                    MessageBox.Show(ex.Message, "Regla de negocio", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnEliminarLibro_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgLibros.SelectedItem is Libro l))
            {
                MessageBox.Show("Seleccione un libro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("¿Confirma eliminar el libro?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _service.EliminarLibroAsync(l.LibroId);
                    MessageBox.Show("Libro eliminado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarLibrosAsync();
                }
                catch (ReglaNegocioException ex)
                {
                    MessageBox.Show(ex.Message, "Regla de negocio", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnBuscarLibros_Click(object sender, RoutedEventArgs e)
        {
            var texto = txtBuscarLibros.Text.Trim();
            var lista = await _service.BuscarLibrosAsync(texto);
            dgLibros.ItemsSource = lista;
        }

        private async void BtnRefrescarSocios_Click(object sender, RoutedEventArgs e)
        {
            await CargarSociosAsync();
        }

        private async void BtnNuevoSocio_Click(object sender, RoutedEventArgs e)
        {
            var win = new Views.SocioWindow();
            if (win.ShowDialog() == true)
            {
                try
                {
                    var id = await _service.InsertarSocioAsync(win.Socio);
                    MessageBox.Show($"Socio insertado. Id: {id}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarSociosAsync();
                }
                catch (ReglaNegocioException ex)
                {
                    MessageBox.Show(ex.Message, "Regla de negocio", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnEditarSocio_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgSocios.SelectedItem is Socio s))
            {
                MessageBox.Show("Seleccione un socio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var win = new Views.SocioWindow();
            win.SetSocio(new Socio { SocioId = s.SocioId, DNI = s.DNI, Nombre = s.Nombre, Email = s.Email });
            if (win.ShowDialog() == true)
            {
                try
                {
                    await _service.ActualizarSocioAsync(win.Socio);
                    MessageBox.Show("Socio actualizado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarSociosAsync();
                }
                catch (ReglaNegocioException ex)
                {
                    MessageBox.Show(ex.Message, "Regla de negocio", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnEliminarSocio_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgSocios.SelectedItem is Socio s))
            {
                MessageBox.Show("Seleccione un socio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("¿Confirma eliminar el socio?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _service.EliminarSocioAsync(s.SocioId);
                    MessageBox.Show("Socio eliminado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CargarSociosAsync();
                }
                catch (ReglaNegocioException ex)
                {
                    MessageBox.Show(ex.Message, "Regla de negocio", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnBuscarSocios_Click(object sender, RoutedEventArgs e)
        {
            var texto = txtBuscarSocios.Text.Trim();
            var lista = await _service.ListarSociosAsync();
            if (!string.IsNullOrEmpty(texto))
            {
                lista = lista.FindAll(s => s.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) || s.DNI.Contains(texto));
            }

            dgSocios.ItemsSource = lista;
        }

        private async void BtnRegistrarPrestamo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var socio = dgSociosPrestamo.SelectedItem as Socio;
                if (socio == null)
                {
                    MessageBox.Show("Seleccione un socio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var libros = lbLibrosDisponibles.SelectedItems.Cast<Libro>().ToList();
                if (libros.Count == 0)
                {
                    MessageBox.Show("Seleccione al menos un libro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var ids = libros.Select(l => l.LibroId);
                var fechaPrestamo = DateTime.Now.Date;
                var fechaLimite = fechaPrestamo.AddDays(15);

                var prestamoId = await _service.RegistrarPrestamoAsync(socio.SocioId, fechaPrestamo, fechaLimite, ids);
                MessageBox.Show($"Préstamo registrado. Id: {prestamoId}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                await CargarDatosPrestamoAsync();
                await CargarLibrosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DgPrestamos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPrestamos.SelectedItem is Prestamo p)
            {
                var detalle = await _service.ObtenerDetalleAsync(p.PrestamoId);
                dgDetalle.ItemsSource = detalle;
            }
            else
            {
                dgDetalle.ItemsSource = null;
            }
        }

        private async void BtnDevolver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(dgPrestamos.SelectedItem is Prestamo p))
                {
                    MessageBox.Show("Seleccione un préstamo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!(dgDetalle.SelectedItem is DetallePrestamo d))
                {
                    MessageBox.Show("Seleccione un libro del detalle.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var fecha = DateTime.Now.Date;
                var res = await _service.DevolverLibroAsync(p.PrestamoId, d.LibroId, fecha);
                MessageBox.Show($"Devolución registrada. Multa: S/ {res.Multa:0.00}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                await CargarDatosPrestamoAsync();
                await CargarLibrosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
