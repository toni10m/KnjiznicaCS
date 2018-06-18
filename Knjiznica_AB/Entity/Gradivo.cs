using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.Attributes;

namespace Knjiznica_AB.Entity
{
    [Serializable]
    [Class(Schema = "knjiznica2", Table = "Gradiva", NameType = typeof(Gradivo))]

    public class Gradivo
    {
        [Id(Name = "Id", Column = "ID", Type = "int"), Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [Property(Column = "Naslov", Type = "string", NotNull = true, Length = 128)]
        public virtual string Naslov { get; set; }

        [Property(Column = "Avtor", Type = "string", Length = 128)]
        public virtual string Avtor { get; set; }

        [Property(Column = "ZvrstGradiva", Type = "string",  Length = 128)]
        public virtual string ZvrstGradiva { get; set; }

        [Property(Column = "LetoIzdaje", Type = "int")]
        public virtual int LetoIzdaje { get; set; }

        [Property(Column = "NaZalogi", Type = "int")]
        public virtual int NaZalogi { get; set; }

        //cascade na parent -avtomatski update childov
        [Bag(Table = "Izposoje", Cascade = "all-delete-orphan", Inverse = true)]
        [Key(1, Column = "FK_Gradivo_Id")]
        [OneToMany(2, ClassType = typeof(Izposoja))]

        public virtual IList<Izposoja> Izposoje { get; set; }

        //cascade na parent -avtomatski update childov
        
    }
}
