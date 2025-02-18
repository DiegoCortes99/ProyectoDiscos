using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Aplicacion_DiscosDB
{
    public partial class frmAgregar : Form
    {

        private Discos discos = null;

        public frmAgregar()
        {
            InitializeComponent();
        }

        public frmAgregar(Discos discos)
        {
            InitializeComponent();
            this.discos = discos;
            Text = "Modificar";
        }



        private void frmAgregar_Load(object sender, EventArgs e)
        {
            EstiloNegocio estiloNegocio = new EstiloNegocio();
            TipoNegocio tipoNegocio = new TipoNegocio();
            try
            {
                cboEstilo.DataSource =  estiloNegocio.listarEstilos();
                cboEstilo.ValueMember = "Id";
                cboEstilo.DisplayMember = "Descripcion";
                
                cboTipo.DataSource = tipoNegocio.listar();
                cboTipo.ValueMember = "Id";
                cboTipo.DisplayMember = "Descripcion";

                if (discos != null)
                {
                    txtTitulo.Text = discos.Titulo;
                    txtCantidadCanciones.Text = discos.CantidadCanciones.ToString();
                    txtImg.Text = discos.UrlImagen;
                    cargarImagen(discos.UrlImagen);
                    cboEstilo.SelectedValue = discos.Estilo.Id;
                    cboTipo.SelectedValue = discos.Tipo.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();

            try
            {
                if (discos == null)
                {
                    discos = new Discos();
                }

                discos.Titulo = txtTitulo.Text;
                discos.FechaLanzamiento = dtpFecha.Value;
                discos.CantidadCanciones = int.Parse(txtCantidadCanciones.Text);
                discos.UrlImagen = txtImg.Text;
                cargarImagen(txtImg.Text);
                discos.Estilo = (Estilos)cboEstilo.SelectedItem;
                discos.Tipo = (TipoEdicion)cboTipo.SelectedItem;


                if (discos.Id != 0)
                {
                    negocio.modificar(discos);
                    MessageBox.Show("Modificado Exitosamente");
                }
                else
                {
                    negocio.agregar(discos);
                    MessageBox.Show("Agregado Exitosamente");
                }

                Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                ptbImg.Load(imagen);
            }
            catch (Exception)
            {
                ptbImg.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        private void txtImg_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImg.Text);
        }
    }
}
