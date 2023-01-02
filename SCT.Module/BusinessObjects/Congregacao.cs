using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SCT.Module.Controllers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("Actions_Home"), XafDefaultProperty(nameof(Nome)),
        NavigationItem("Tabelas"), XafDisplayName("Congregações"), Persistent("Congregacoes")]
    public class Congregacao : BaseObject, IExportDoc
    {
        public Congregacao(Session session) : base(session)
        {
            Coordenador = new Contato(Session);
            Ajudante = new Contato(Session);
        }

        Contato ajudante;
        Contato coordenador;
        Cidade cidade;
        int previsaoPassagens;
        Circuito circuito;
        string nome;

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value);
        }

        public Cidade Cidade
        {
            get => cidade;
            set => SetPropertyValue(nameof(Cidade), ref cidade, value);
        }
        [Association("Circuito-Congregacoes")]
        public Circuito Circuito
        {
            get => circuito;
            set => SetPropertyValue(nameof(Circuito), ref circuito, value);
        }

        [XafDisplayName("Previsão"), ToolTip("Previsão de passageiros no arranjo.")]
        public int PrevisaoPassagens
        {
            get => previsaoPassagens;
            set => SetPropertyValue(nameof(PrevisaoPassagens), ref previsaoPassagens, value);
        }

        [DevExpress.Xpo.Aggregated]
        public Contato Coordenador
        {
            get => coordenador;
            set => SetPropertyValue(nameof(Coordenador), ref coordenador, value);
        }
        [DevExpress.Xpo.Aggregated]
        public Contato Ajudante
        {
            get => ajudante;
            set => SetPropertyValue(nameof(Ajudante), ref ajudante, value);
        }
        [XafDisplayName("Saídas")]
        [Association("Congregacao-Saidas")]
        public XPCollection<Saida> Saidas
        {
            get
            {
                return GetCollection<Saida>(nameof(Saidas));
            }
        }
        [Association("Evento-Congregacoes")]
        public XPCollection<Evento> Eventos
        {
            get
            {
                return GetCollection<Evento>(nameof(Eventos));
            }
        }
        [Association("Congregacao-Passageiros")]
        public XPCollection<Passageiro> Passageiros
        {
            get
            {
                return GetCollection<Passageiro>(nameof(Passageiros));
            }
        }
    }
}
