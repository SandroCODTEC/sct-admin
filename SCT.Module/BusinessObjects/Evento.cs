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
    [DefaultClassOptions, ImageName("GeoPointMaps"), NavigationItem("Eventos"), XafDisplayName("Eventos"), Persistent("Eventos")]
    [DefaultProperty(nameof(Descricao))]
    public class Evento : BaseObject
    {
        public Evento(Session session) : base(session)
        { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Coordenacao = Session.Query<Coordenacao>().FirstOrDefault();
        }

        DateTime dataFinal;
        DateTime dataInicial;
        decimal valorPassagem;
        Color corFonte;
        Color corContraste;
        Color corFundo;
        DateTime pagamento;
        DateTime lista;
        string tema;
        string descricao;
        Coordenacao coordenacao;

        [Required]
        [XafDisplayName("Descrição")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Descricao
        {
            get => descricao;
            set => SetPropertyValue(nameof(Descricao), ref descricao, value);
        }

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Tema
        {
            get => tema;
            set => SetPropertyValue(nameof(Tema), ref tema, value);
        }

        [Required]
        public DateTime DataInicial
        {
            get => dataInicial;
            set => SetPropertyValue(nameof(DataInicial), ref dataInicial, value.Date);
        }
        
        [Required]
        public DateTime DataFinal
        {
            get => dataFinal;
            set => SetPropertyValue(nameof(DataFinal), ref dataFinal, value.Date);
        }
        [Required]
        [ToolTip("Data limite para as congregações entregarem a lista de passageiros.")]
        public DateTime Lista
        {
            get => lista;
            set => SetPropertyValue(nameof(Lista), ref lista, value);
        }

        [Required]
        [ToolTip("Data limite para as congregações efetuarem o pagamento das passagens.")]
        public DateTime Pagamento
        {
            get => pagamento;
            set => SetPropertyValue(nameof(Pagamento), ref pagamento, value);
        }
        
        public decimal ValorPassagem
        {
            get => valorPassagem;
            set => SetPropertyValue(nameof(ValorPassagem), ref valorPassagem, value);
        }
        [Required]
        [ToolTip("Cor do fundo das passagens.")]
        [ValueConverter(typeof(ColorValueConverter))]
        public Color CorFundo
        {
            get => corFundo;
            set => SetPropertyValue(nameof(CorFundo), ref corFundo, value);
        }

        [Required]
        [ToolTip("Cor de contraste das passagens.")]
        [ValueConverter(typeof(ColorValueConverter))]
        public Color CorContraste
        {
            get => corContraste;
            set => SetPropertyValue(nameof(CorContraste), ref corContraste, value);
        }

        [Required]
        [ToolTip("Cor da fonte das passagens.")]
        [ValueConverter(typeof(ColorValueConverter))]
        public Color CorFonte
        {
            get => corFonte;
            set => SetPropertyValue(nameof(CorFonte), ref corFonte, value);
        }
        [XafDisplayName("Coordenação")]
        public Coordenacao Coordenacao
        {
            get => coordenacao;
            set => SetPropertyValue(nameof(Coordenacao), ref coordenacao, value);
        }

        [Association("Evento-Dias"), DevExpress.Xpo.Aggregated]
        public XPCollection<DiaEvento> Dias
        {
            get
            {
                return GetCollection<DiaEvento>(nameof(Dias));
            }
        }
        [Association("Evento-Comprovantes"), DevExpress.Xpo.Aggregated]
        public XPCollection<Comprovante> Comprovantes
        {
            get
            {
                return GetCollection<Comprovante>(nameof(Comprovantes));
            }
        }
        [XafDisplayName("Congregações")]
        [Association("Evento-Congregacoes")]
        public XPCollection<Congregacao> Congregacoes
        {
            get
            {
                return GetCollection<Congregacao>(nameof(Congregacoes));
            }
        }
    }
}
