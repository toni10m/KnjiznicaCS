using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//uporabljeno za pridobivanje polj v objektov v datagridview
namespace Knjiznica_AB
{
    class MyCustomTypeDescriptor : CustomTypeDescriptor
    {
        public MyCustomTypeDescriptor(ICustomTypeDescriptor parent)


            : base(parent)


        {


        }





        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)


        {


            PropertyDescriptorCollection cols = base.GetProperties(attributes);





            PropertyDescriptor gradivoPD = cols["Gradivo"];
            PropertyDescriptor izposojevalecPD = cols["Izposojevalec"];
            PropertyDescriptor danIzposoje = cols["DatumIzposoje"];
            PropertyDescriptor danVracila = cols["DatumVracila"];

            PropertyDescriptorCollection gradivo_child = gradivoPD.GetChildProperties();
            PropertyDescriptorCollection izposojevalec_child = izposojevalecPD.GetChildProperties();

            PropertyDescriptor[] array = new PropertyDescriptor[cols.Count + 6];

            cols.CopyTo(array, 0);


            array[cols.Count] = new SubPropertyDescriptor(gradivoPD, gradivo_child["Naslov"], "Naslov");
            array[cols.Count + 1] = new SubPropertyDescriptor(gradivoPD, gradivo_child["Avtor"], "Avtor");

            array[cols.Count + 2] = new SubPropertyDescriptor(izposojevalecPD, izposojevalec_child["Ime"], "Ime");
            array[cols.Count + 3] = new SubPropertyDescriptor(izposojevalecPD, izposojevalec_child["Priimek"], "Priimek");

            array[cols.Count + 4] = danIzposoje;
            array[cols.Count + 5] = danVracila;




            PropertyDescriptorCollection newcols = new PropertyDescriptorCollection(array);


            return newcols;


        }
    }
}
