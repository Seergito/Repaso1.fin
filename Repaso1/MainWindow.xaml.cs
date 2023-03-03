using EjemploConexiónBDAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Repaso1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        BDMySql db;
        DataTable tabla;
        String sql = "SELECT * FROM empleados order by num_empleado";
       

        

        public MainWindow()
        {
            InitializeComponent();
            db = BDMySql.getInstance();
            db.AbrirConexion();

            tabla = db.LanzaSelect(sql, false);
            
            dg.ItemsSource = tabla.DefaultView;
            dg.AutoGenerateColumns = true;
            dg.CanUserAddRows = false;
            

       
        }
        


        private void btn_agregar_Click(object sender, RoutedEventArgs e)
        {
            IVentana iventana=new IVentana();
            iventana.ShowDialog();
            tabla = db.LanzaSelect(sql, false);
            dg.ItemsSource = tabla.DefaultView;
        }

        private void btn_editar_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedIndex >= 0)
            {
                Empleado emp = get_emp();
                IVentana iventana = new IVentana(emp,dg.SelectedIndex);
                iventana.ShowDialog();
                tabla = db.LanzaSelect(sql, false);
                dg.ItemsSource = tabla.DefaultView;

            }
            else
            {
                MessageBox.Show("Selecciona al menos un empleado");
            }

        }

 
        private void btn_eliminar_Click(object sender, RoutedEventArgs e)
        {

            if (dg.SelectedIndex >= 0)
            {
                // MODO DIRECTO
                /*
                tabla.Rows[dg.SelectedIndex].Delete();
                db.ActualizaDatosTabla(sql, tabla);
                */

                // MODO AISLADO
                DataRowView fila = (DataRowView)dg.SelectedItem;
                int num_emp = Int32.Parse(fila[0].ToString());

                //EJECUTAR CONSULTA DE ACCION+
                string delete = "DELETE FROM empleados WHERE NUM_EMPLEADO =" + num_emp;
                db.EjecutaConsultaAccion(delete);

                //REVISUALIZAR TABLA
                tabla = db.LanzaSelect(sql, false);
                dg.ItemsSource = tabla.DefaultView;

            }
            else
            {
                MessageBox.Show("Debe seleccionar un empleado");
            }
        }

        public Empleado get_emp()
        {

            //INDICE, FILA MARCADA EN DG
            if (dg.SelectedIndex >= 0)
            {
                //EXTRAER TODA LA INFORMACION PARA OBTENER EMP
                DataRowView fila = (DataRowView)dg.SelectedItem;
                int num_emp = Int32.Parse(fila[0].ToString());
                string nombre = fila[1].ToString();
                string puesto = fila[2].ToString();
                int jefe;
                //CONTROL ERRORES
                try
                {
                    jefe = Int32.Parse(fila[3].ToString());
                }
                catch (Exception)
                {
                    jefe = 0;
                }
                
                DateTime? fecha = Convert.ToDateTime(fila[4].ToString());
                decimal salario = decimal.Parse(fila[5].ToString());
                decimal comision;
                try
                {
                    comision = decimal.Parse(fila[6].ToString());
                }
                catch (Exception)
                {
                    comision = 0;
                }
                bool? vip = Convert.ToBoolean(fila[7].ToString());
                int ndepart = Int32.Parse(fila[8].ToString());

                Empleado emp = new Empleado(num_emp, nombre, puesto, jefe, fecha, salario, comision, vip, ndepart);
                return emp;
            }
            return null;
        }


        private void menu_salir_Click(object sender, RoutedEventArgs e)
        {
            db.CerrarConexion();
            Close();
        }
    }
}
