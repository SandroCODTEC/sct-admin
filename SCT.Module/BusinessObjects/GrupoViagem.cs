using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SCT.Module.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [ImageName("Reviewers"), XafDefaultProperty(nameof(Descricao)),
    NavigationItem("Tabelas"), XafDisplayName("Grupo de viagens"), Persistent("GrupoViagens")]
    public class GrupoViagem : BaseObject
    {
        public GrupoViagem(Session session) : base(session)
        { }

        Color cor;
        string descricao;

        [XafDisplayName("Descrição")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Descricao
        {
            get => descricao;
            set => SetPropertyValue(nameof(Descricao), ref descricao, value);
        }

        [ValueConverter(typeof(ColorValueConverter))]
        public Color Cor
        {
            get => cor;
            set => SetPropertyValue(nameof(Cor), ref cor, value);
        }
    }
}
