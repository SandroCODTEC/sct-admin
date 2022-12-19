using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using FileSystemData.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("Business_DollarCircled"), XafDefaultProperty(nameof(Descricao)),
        NavigationItem("Eventos"), XafDisplayName("Comprovantes"), Persistent("Comprovantes")]
    [FileAttachment(nameof(Arquivo))]
    public class Comprovante : BaseObject
    {
        public Comprovante(Session session) : base(session)
        { }


        Fornecedor fornecedor;
        decimal valor;
        DateTime data;
        Congregacao congregacao;
        Evento evento;
        FileSystemStoreObject arquivo;

        [XafDisplayName("Descrição")]
        [PersistentAlias("concat(Congregacao, ' - ', Valor)")]
        public string Descricao
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Descricao))); }
        }

        [Association("Evento-Comprovantes")]
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
        
        public Fornecedor Fornecedor
        {
            get => fornecedor;
            set => SetPropertyValue(nameof(Fornecedor), ref fornecedor, value);
        }

        [Required]
        public DateTime Data
        {
            get => data;
            set => SetPropertyValue(nameof(Data), ref data, value);
        }

        [Required]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }
        [DevExpress.Xpo.Aggregated, ImmediatePostData, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileSystemStoreObject Arquivo
        {
            get { return arquivo; }
            set { SetPropertyValue(nameof(Arquivo), ref arquivo, value); }
        }
    }
}
