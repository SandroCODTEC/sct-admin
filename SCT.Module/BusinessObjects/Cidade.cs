using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Drawing;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("CustomerQuickLocations"), XafDefaultProperty(nameof(Nome)),
        NavigationItem("Tabelas"), XafDisplayName("Cidades"), Persistent("Cidades")]
    public class Cidade : BaseObject
    {
        public Cidade(Session session) : base(session)
        { }


        string nome;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value);
        }
    }
}
