using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SCT.Module.Controllers;
using SCT.Module.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("BO_Lead"), XafDefaultProperty(nameof(Nome)),
    NavigationItem("Tabelas"), XafDisplayName("Passageiros"), Persistent("Passageiros")]
    public class Passageiro : BaseObject, IExportDoc
    {
        public Passageiro(Session session) : base(session)
        { }

        Guid passageiroApp;
        Congregacao congregacao;
        TipoDocumento tipoDocumento;
        string documento;
        string nome;


        [Required]
        [Association("Congregacao-Passageiros")]
        public Congregacao Congregacao
        {
            get => congregacao;
            set => SetPropertyValue(nameof(Congregacao), ref congregacao, value);
        }
        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value?.ToUpper());
        }
        
        [VisibleInListView(false)]
        public Guid PassageiroApp
        {
            get => passageiroApp;
            set => SetPropertyValue(nameof(PassageiroApp), ref passageiroApp, value);
        }
        [Required]
        public TipoDocumento TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Documento
        {
            get => documento;
            set => SetPropertyValue(nameof(Documento), ref documento, value);
        }
        [Association("Passageiro-Dependentes")]
        public XPCollection<Dependente> Dependentes
        {
            get
            {
                return GetCollection<Dependente>(nameof(Dependentes));
            }
        }
    }
}
