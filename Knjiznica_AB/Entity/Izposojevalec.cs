using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace Knjiznica_AB.Entity
{
    [Serializable]
    [Class(Schema = "knjiznica2", Table = "Izposojevalci", NameType = typeof(Izposojevalec))]
    public class Izposojevalec
    {
        [Id(Name = "Id", Column = "ID", Type = "int"), Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [Property(Column = "IdentifikacijskaStevilka", Type = "int", NotNull = true)]
        public virtual int IdentifikacijskaStevilka { get; set; }

        [Property(Column = "Ime", Type = "string", NotNull = true,  Length = 128)]
        public virtual string Ime { get; set; }

        [Property(Column = "Priimek", Type = "string", NotNull = true, Length = 128)]
        public virtual string Priimek { get; set; }

        [Property(Column = "TelSt", Type = "string", Length = 128)]
        public virtual string TelSt { get; set; }

        
        //cascade na parent -avtomatski update childov
        [Bag(Table = "Izposoje", Cascade = "all-delete-orphan", Inverse = true)]
        [Key(1, Column = "FK_Izposojevalec_Id")]
        [OneToMany(2, ClassType = typeof(Izposoja))]

        public virtual IList<Izposoja> Izposoje { get; set; }


        
    }
}
