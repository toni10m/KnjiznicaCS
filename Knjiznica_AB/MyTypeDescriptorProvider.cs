using Knjiznica_AB.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knjiznica_AB
{
    public class MyTypeDescriptionProvider : TypeDescriptionProvider
    {
        private ICustomTypeDescriptor td;





        public MyTypeDescriptionProvider()


           : this(TypeDescriptor.GetProvider(typeof(Izposoja)))


        {


        }


        public MyTypeDescriptionProvider(TypeDescriptionProvider parent)


            : base(parent)


        {


        }


        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)


        {


            if (td == null)


            {


                td = base.GetTypeDescriptor(objectType, instance);


                td = new MyCustomTypeDescriptor(td);


            }


            return td;


        }
    }
}
