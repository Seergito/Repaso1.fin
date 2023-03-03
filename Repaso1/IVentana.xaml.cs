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
using System.Windows.Shapes;

namespace Repaso1
{
    /// <summary>
    /// Lógica de interacción para IVentana.xaml
    /// </summary>
    public partial class IVentana : Window
    {
        Empleado e;
        int index;
        
        BDMySql db;

        DataTable tabla_dep;
        DataTable tabla_emp;

        string sql = "SELECT * FROM empleados";
        string sql2 = "SELECT * FROM departamentos";
        bool editando;

        public IVentana(Empleado e,int index)
        {
            InitializeComponent();
            this.e = e;
            this.index = index;

            editando = true;

            db = BDMySql.getInstance();
            db.AbrirConexion();

            tabla_emp = db.LanzaSelect(sql, false);
            tabla_dep = db.LanzaSelect(sql2, false);

            combo.SelectedValuePath = "NUM_DEPARTAMENTO";
            combo.DisplayMemberPath = "NOMBRE";
            combo.ItemsSource = tabla_dep.DefaultView;

            tb_numero.Text = e.num.ToString();
            tb_nombre.Text = e.nombre;
            tb_puesto.Text = e.puesto;
            tb_jefe.Text = e.jefe.ToString();
            fecha.SelectedDate = e.fecha;
            tb_salario.Text = e.salario.ToString();
            tb_comision.Text = e.comision.ToString();
            cb.IsChecked = e.vip;
            combo.SelectedValue = e.ndepart;




        }
        public IVentana()
        {
            InitializeComponent();

            editando = false;

            db =BDMySql.getInstance();
            db.AbrirConexion();

            tabla_emp = db.LanzaSelect(sql, false);
            tabla_dep = db.LanzaSelect(sql2, false);

            combo.SelectedValuePath = "NUM_DEPARTAMENTO";
            combo.DisplayMemberPath = "NOMBRE";
            combo.ItemsSource = tabla_dep.DefaultView;


        }

        private void bt_aceptar_Click(object sender, RoutedEventArgs e)
        {
            int numero;
     
            
             numero =Int32.Parse(tb_numero.Text);
       
            string nombre=tb_nombre.Text;
            string puesto=tb_puesto.Text;
            int nejefe=Int32.Parse(tb_jefe.Text);
            DateTime? f = fecha.SelectedDate;   //IMP PROPERTY SELECTED_DATE
            decimal salario = Decimal.Parse(tb_salario.Text);
            decimal comision = Decimal.Parse(tb_comision.Text); //IMP PROPERTY IS CHECKED
            bool? vip = Convert.ToBoolean(cb.IsChecked);
            int ndep = Int32.Parse(combo.SelectedValue.ToString());

            Empleado emp=new Empleado(numero,nombre,puesto,nejefe,f,salario,comision,vip,ndep);

            if (editando == false)
            {
                // validar datos
                DataRow fila = tabla_emp.NewRow();
                fila["NUM_EMPLEADO"] = numero;
                fila["NOMBRE_EMPLEADO"] = nombre;
                fila["PUESTO"] = puesto;
                fila["NUM_JEFE"] = nejefe;
                fila["FECHA_ALTA"] = f;
                fila["SALARIO"] = salario;
                fila["COMISION"] = comision;
                fila["VIP"] = vip;
                fila["NUM_DEPARTAMENTO"] = ndep;
                tabla_emp.Rows.Add(fila);
                db.ActualizaDatosTabla(sql, tabla_emp);
                MessageBox.Show("Empleado insertado con exito");
                Close();
            }
            else
            {
                // validar datos
                int num_fila = -1;
                for (int i=0;i<tabla_emp.Rows.Count; i++ )
                {
                    if (Convert.ToInt32(tabla_emp.Rows[i]["NUM_EMPLEADO"]) == numero)
                    {
                        num_fila = i;
                        break;
                    }
                }
                DataRow fila = tabla_emp.Rows[num_fila];
                fila["NUM_EMPLEADO"] = numero;
                fila["NOMBRE_EMPLEADO"] = nombre;
                fila["PUESTO"] = puesto;
                fila["NUM_JEFE"] = nejefe;
                fila["FECHA_ALTA"] = f;
                fila["SALARIO"] = salario;
                fila["COMISION"] = comision;
                fila["VIP"] = vip;
                fila["NUM_DEPARTAMENTO"] = ndep;
                MessageBox.Show("Empleado actualizado con exito");
                db.ActualizaDatosTabla(sql, tabla_emp);
                Close();
                
            }
            

        }

        private void bt_cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
