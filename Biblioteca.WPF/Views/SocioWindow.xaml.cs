using System.Windows;
using Biblioteca.Entidades;

namespace Biblioteca.WPF.Views
{
    public partial class SocioWindow : Window
    {
        public Socio Socio { get; private set; }

        public SocioWindow()
        {
            InitializeComponent();
            Socio = new Socio();
        }

        public void SetSocio(Socio socio)
        {
            Socio = socio ?? new Socio();
            txtDNI.Text = Socio.DNI;
            txtNombre.Text = Socio.Nombre;
            txtEmail.Text = Socio.Email;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Socio.DNI = txtDNI.Text.Trim();
            Socio.Nombre = txtNombre.Text.Trim();
            Socio.Email = txtEmail.Text.Trim();

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
