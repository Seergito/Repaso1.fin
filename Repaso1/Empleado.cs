using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Repaso1
{
   public  class Empleado
    {

        public int num { get; set; }
        public string nombre { get; set; }

        public string puesto { get; set; }  
        public int jefe { get; set; }
        public DateTime? fecha { get; set; }    

        public decimal salario { get; set; }    

        public decimal comision { get; set; }

        public bool? vip { get; set; }

        public int ndepart { get; set; }

        public Empleado(int num,string nombre,string puesto,int jefe,DateTime? fecha,decimal salario,decimal comision,bool? vip,int ndepart)
        {
            this.num = num;
            this.nombre = nombre;   
            this.puesto = puesto;
            this.jefe = jefe;
            this.fecha = fecha;
            this.salario = salario;
            this.comision = comision;
            this.vip = vip;
            this.ndepart = ndepart;
        }

        public static implicit operator Empleado(DataRow v)
        {
            throw new NotImplementedException();
        }
    }
}
