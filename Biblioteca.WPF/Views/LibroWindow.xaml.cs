using System;
using System.Windows;
using Biblioteca.Entidades;

namespace Biblioteca.WPF.Views
{
    public partial class LibroWindow : Window
    {
        public Libro Libro { get; private set; }

        public LibroWindow()
        {
            InitializeComponent();
            Libro = new Libro();
        }

        public void SetLibro(Libro libro)
        {
            Libro = libro ?? new Libro();
            txtTitulo.Text = Libro.Titulo;
            txtISBN.Text = Libro.ISBN;
            txtAutorId.Text = Libro.AutorId.ToString();
            txtEjemplares.Text = Libro.Ejemplares.ToString();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Libro.Titulo = txtTitulo.Text.Trim();
            Libro.ISBN = txtISBN.Text.Trim();
            if (int.TryParse(txtAutorId.Text.Trim(), out var a)) Libro.AutorId = a;
            if (int.TryParse(txtEjemplares.Text.Trim(), out var ej)) Libro.Ejemplares = ej;

            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
