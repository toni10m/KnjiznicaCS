using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace Knjiznica_AB.Entity
{
    [Serializable]
    [Class(Schema = "knjiznica2", Table = "Izposoje", NameType = typeof(Izposoja))]
    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    public class Izposoja
    {
        [Id(Name = "Id", Column = "ID", Type = "int"), Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [ManyToOne(ClassType = typeof(Gradivo), Column = "FK_Gradivo_Id", NotNull = true)]
        public virtual Gradivo Gradivo { get; set; }

        [ManyToOne(ClassType = typeof(Zaposlen), Column = "FK_Zaposlen_Id")]
        public virtual Zaposlen Zaposlen { get; set; }

        [ManyToOne(ClassType = typeof(Izposojevalec), Column = "FK_Izposojevalec_Id", NotNull = true)]
        public virtual Izposojevalec Izposojevalec { get; set; }

        [Property(Column = "DatumIzposoje", Type = "date")]
        public virtual DateTime DatumIzposoje { get; set; }

        [Property(Column = "DatumVracila", Type = "date")]
        public virtual DateTime DatumVracila { get; set; }
        


        
    }
}
