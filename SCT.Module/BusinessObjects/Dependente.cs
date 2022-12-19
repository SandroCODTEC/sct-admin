using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
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
    [ImageName("BO_Department"), XafDefaultProperty(nameof(Nome)),
        NavigationItem("Tabelas"), XafDisplayName("Dependentes"), Persistent("Dependentes")]
    public class Dependente : BaseObject
    {
        public Dependente(Session session) : base(session)
        { }


        Guid dependenteApp;
        Passageiro passageiro;
        DateTime dataNascimento;
        TipoDocumento tipoDocumento;
        string documento;
        string nome;

        [VisibleInListView(false)]
        public Guid DependenteApp
        {
            get => dependenteApp;
            set => SetPropertyValue(nameof(DependenteApp), ref dependenteApp, value);
        }

        [Association("Passageiro-Dependentes")]
        public Passageiro Passageiro
        {
            get => passageiro;
            set => SetPropertyValue(nameof(Passageiro), ref passageiro, value);
        }

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value?.ToUpper());
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

        [Required]
        public DateTime DataNascimento
        {
            get => dataNascimento;
            set => SetPropertyValue(nameof(DataNascimento), ref dataNascimento, value);
        }
        [Browsable(false)]
        [Association("Passagem-Dependentes")]
        public XPCollection<Passagem> Passagens
        {
            get
            {
                return GetCollection<Passagem>(nameof(Passagens));
            }
        }
    }
}
