using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app
{
    public partial class frmCompararArticulo : Form
    {
        private Articulo articulo1 = null;
        private Articulo articulo2 = null;

        public frmCompararArticulo()
        {
            InitializeComponent();
        }

        public frmCompararArticulo(Articulo articulo1, Articulo articulo2)
        {
            InitializeComponent();

            this.articulo1 = articulo1;
            this.articulo2 = articulo2;

            Text = articulo1.Nombre + " / " + articulo2.Nombre; 
        }

        private void frmCompararArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cboMarcaArt1.DataSource = marcaNegocio.listar();
                cboMarcaArt1.ValueMember = "Id";
                cboMarcaArt1.DisplayMember = "Descripcion";
                cboCategoriaArt1.DataSource = categoriaNegocio.listar();
                cboCategoriaArt1.ValueMember = "Id";
                cboCategoriaArt1.DisplayMember = "Descripcion";

                cboMarcaArt2.DataSource = marcaNegocio.listar();
                cboMarcaArt2.ValueMember = "Id";
                cboMarcaArt2.DisplayMember = "Descripcion";
                cboCategoriaArt2.DataSource = categoriaNegocio.listar();
                cboCategoriaArt2.ValueMember = "Id";
                cboCategoriaArt2.DisplayMember = "Descripcion";

                if (articulo1 != null && articulo2 != null)
                {
                    txtCodigoArt1.Text = articulo1.Codigo;
                    txtNombreArt1.Text = articulo1.Nombre;
                    txtDescripcionArt1.Text = articulo1.Descripcion;
                    txtPrecioArt1.Text = articulo1.Precio.ToString();
                    cboMarcaArt1.SelectedValue = articulo1.Marca.Id;
                    cboCategoriaArt1.SelectedValue = articulo1.Categoria.Id;
                    txtImagenUrlArt1.Text = articulo1.ImagenUrl;

                    txtCodigoArt2.Text = articulo2.Codigo;
                    txtNombreArt2.Text = articulo2.Nombre;
                    txtDescripcionArt2.Text = articulo2.Descripcion;
                    txtPrecioArt2.Text = articulo2.Precio.ToString();
                    cboMarcaArt2.SelectedValue = articulo2.Marca.Id;
                    cboCategoriaArt2.SelectedValue = articulo2.Categoria.Id;
                    txtImagenUrlArt2.Text = articulo2.ImagenUrl;

                    cargarImagenes(articulo1.ImagenUrl, articulo2.ImagenUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagenes(string imagen1, string imagen2)
        {
            try
            {
                pbArticulo1.Load(imagen1);
                pbArticulo2.Load(imagen2);
            }
            catch (Exception)
            {
                pbArticulo1.Load("https://t4.ftcdn.net/jpg/05/17/53/57/360_F_517535712_q7f9QC9X6TQxWi6xYZZbMmw5cnLMr279.jpg");
                pbArticulo2.Load("https://t4.ftcdn.net/jpg/05/17/53/57/360_F_517535712_q7f9QC9X6TQxWi6xYZZbMmw5cnLMr279.jpg");
            }
        }
    }
}
