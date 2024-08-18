using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace app
{
    public partial class frmArticulo : Form
    {
        private List<Articulo> listaArticulos;
        
        public frmArticulo()
        {
            InitializeComponent();
        }

        private void frmArticulo_Load(object sender, EventArgs e)
        {
            cargar();
            listarCriteriosDeOrdenamiento();
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.SelectedRows.Count > 2)
            {
                dgvArticulos.SelectedRows[dgvArticulos.SelectedRows.Count - 1].Selected = false;
            }
            
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulos = negocio.listarArticulo();
                dgvArticulos.DataSource = listaArticulos;
                ocultarColumnas();
                cboOrdenar.SelectedItem = null;
                pbArticulo.Load(listaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbArticulo.Load("https://t4.ftcdn.net/jpg/05/17/53/57/360_F_517535712_q7f9QC9X6TQxWi6xYZZbMmw5cnLMr279.jpg");
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void agregar()
        {
            frmGestionArticulo agregar = new frmGestionArticulo();

            agregar.ShowDialog();
            cargar();
        }

        private void modificar()
        {
            if (dgvArticulos.CurrentRow != null && dgvArticulos.CurrentRow.DataBoundItem != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmGestionArticulo modificar = new frmGestionArticulo(seleccionado);

                modificar.ShowDialog();
                cargar();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artículo antes de modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comparar()
        {
            if (dgvArticulos.SelectedRows.Count != 2)
            {
                MessageBox.Show("Por favor, selecciona exactamente dos artículos para comparar.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Articulo seleccionado1;
            Articulo seleccionado2;

            seleccionado1 = (Articulo)dgvArticulos.SelectedRows[0].DataBoundItem;
            seleccionado2 = (Articulo)dgvArticulos.SelectedRows[1].DataBoundItem;

            frmCompararArticulo comparar = new frmCompararArticulo(seleccionado2, seleccionado1);

            comparar.ShowDialog();
            cargar();
        }

        private void eliminar(bool logico = false)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
                if (dgvArticulos.CurrentRow == null)
                {
                    MessageBox.Show("No hay ningún artículo seleccionado para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                DialogResult resp = MessageBox.Show("¿Desea eliminar el artículo " + seleccionado.Nombre + " de la lista?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resp == DialogResult.Yes)
                {
                    try
                    {
                        negocio.eliminar(seleccionado.Id);

                        MessageBox.Show("El artículo " + seleccionado.Nombre + " se ha eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cargar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al intentar eliminar el artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listarCriteriosDeOrdenamiento()
        {
            foreach (DataGridViewColumn col in dgvArticulos.Columns) 
            {
                if (col.Name != "Id" && col.Name != "ImagenUrl")
                {
                    cboOrdenar.Items.Add(col.Name);
                }
            }
        }

        private List<Articulo> ordenarPorColumna(List<Articulo> lista, string columna)
        {
            var propiedad = lista[0].GetType().GetProperty(columna);

            if (propiedad == null)
                throw new ArgumentException("La columna seleccionada no es válida.", nameof(columna));

            if (propiedad.PropertyType == typeof(Marca))
            {
                return lista
                    .OrderBy(a => ((Marca)propiedad.GetValue(a)).Descripcion)
                    .ToList();
            }
            else if (propiedad.PropertyType == typeof(Categoria))
            {
                return lista
                    .OrderBy(a => ((Categoria)propiedad.GetValue(a)).Descripcion)
                    .ToList();
            }
            else
            {
                return lista
                    .OrderBy(a => propiedad.GetValue(a))
                    .ToList();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            agregar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            modificar();
        }

        private void btnComparar_Click(object sender, EventArgs e)
        {
            comparar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboOrdenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOrdenar.SelectedItem != null)
            {
                string columnaSeleccionada = cboOrdenar.SelectedItem.ToString();

                if (listaArticulos.Count > 0)
                {
                    var listaOrdenada = ordenarPorColumna(listaArticulos, columnaSeleccionada);

                    dgvArticulos.DataSource = null;
                    dgvArticulos.DataSource = listaOrdenada;

                    ocultarColumnas();
                }
            }
        }
    }
}
