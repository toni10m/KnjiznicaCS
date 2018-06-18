using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace Knjiznica_AB.Entity
{
    [Serializable]
    [Class(Schema = "knjiznica2", Table = "Zaposleni", NameType = typeof(Zaposlen))]
    public class Zaposlen
    {
        [Id(Name = "Id", Column = "ID", Type = "int"), Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [Property(Column = "UporabniskoIme", Type = "string", NotNull = true, Length = 128)]
        public virtual string UporabniskoIme { get; set; }

        [Property(Column = "Geslo", Type = "string", NotNull = true, Length = 128)]
        public virtual string Geslo { get; set; }

        //cascade na parent -avtomatski update childov
        [Bag(Table = "Izposoje", Cascade = "all-delete-orphan", Inverse = true)]
        [Key(1, Column = "FK_Zaposlen_Id")]
        [OneToMany(2, ClassType = typeof(Izposoja))]

        public virtual IList<Izposoja> Izposoje { get; set; }
    }
}
