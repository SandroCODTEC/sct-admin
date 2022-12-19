using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FileSystemData.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SCT.Module.BusinessObjects
{

    [DefaultClassOptions, ImageName("AssignTask"), XafDefaultProperty(nameof(Congregacao)),
        NavigationItem("Eventos"), XafDisplayName("Importações"), Persistent("Importacoes")]
    [FileAttachment(nameof(Arquivo))]
    public class Importacao : BaseObject
    {                  
        public Importacao(Session session)
            : base(session)
        {
        }

        string chave;
        bool processado;
        DateTime dataImportacao;
        Congregacao congregacao;
        Evento evento;
        FileSystemStoreObject arquivo;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public Evento Evento
        {
            get => evento;
            set => SetPropertyValue(nameof(Evento), ref evento, value);
        }

        [Required]
        [DataSourceProperty("Evento.Congregacoes")]
        [XafDisplayName("Congregação")]
        public Congregacao Congregacao
        {
            get => congregacao;
            set => SetPropertyValue(nameof(Congregacao), ref congregacao, value);
        }

        public DateTime DataImportacao
        {
            get => dataImportacao;
            set => SetPropertyValue(nameof(DataImportacao), ref dataImportacao, value);
        }

        [Size(32)]
        [Persistent]
        [Required]
        public string Chave
        {
            get => chave;
            set => SetPropertyValue(nameof(Chave), ref chave, value);
        }
        [DevExpress.Xpo.Aggregated, ImmediatePostData, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileSystemStoreObject Arquivo
        {
            get { return arquivo; }
            set { SetPropertyValue(nameof(Arquivo), ref arquivo, value); }
        }
        
        public bool Processado
        {
            get => processado;
            set => SetPropertyValue(nameof(Processado), ref processado, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}