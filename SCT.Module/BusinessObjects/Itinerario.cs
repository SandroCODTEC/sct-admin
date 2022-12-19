using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [ImageName("InsertBubbleMap"), XafDefaultProperty(nameof(Descricao)),
        NavigationItem("Eventos"), XafDisplayName("Itinerários"), Persistent("Itinerarios")]
    public class Itinerario : BaseObject
    {
        public Itinerario(Session session) : base(session)
        {
            Passagens.CollectionChanged += Passagens_CollectionChanged;
        }

        private void Passagens_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            if (!IsLoading && e.CollectionChangedType == XPCollectionChangedType.AfterAdd)
            {
                ((Passagem)e.ChangedObject).GerarAssento();
            }
        }

        int parada;
        int totalPassageiros;
        Saida saida;
        Congregacao congregacao;
        DiaTransporte diaTransporte;

        [XafDisplayName("Descrição")]
        [PersistentAlias("concat(DiaTransporte, ' - ', Parada, 'º PARADA')")]
        public string Descricao
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Descricao))); }
        }
        [ImmediatePostData]
        [Association("DiaTransporte-Itinerarios")]
        public DiaTransporte DiaTransporte
        {
            get => diaTransporte;
            set => SetPropertyValue(nameof(DiaTransporte), ref diaTransporte, value);
        }

        public int Parada
        {
            get => parada;
            set => SetPropertyValue(nameof(Parada), ref parada, value);
        }
        //[DataSourceProperty("[DiaTransporte].[Dia].[Evento].[Congregacoes]")]
        [Required]
        [ImmediatePostData]
        [DataSourceCriteria("Eventos[Oid='@This.DiaTransporte.Dia.Evento.Oid']")]
        [XafDisplayName("Congregação")]
        public Congregacao Congregacao
        {
            get => congregacao;
            set => SetPropertyValue(nameof(Congregacao), ref congregacao, value);
        }

        [Required]
        [DataSourceProperty("Congregacao.Saidas")]
        [XafDisplayName("Saída")]
        public Saida Saida
        {
            get => saida;
            set => SetPropertyValue(nameof(Saida), ref saida, value);
        }

        [XafDisplayName("Passageiros"), ToolTip("Total de passageiros neste itinerário.")]
        public int TotalPassageiros
        {
            get => totalPassageiros;
            set => SetPropertyValue(nameof(TotalPassageiros), ref totalPassageiros, value);
        }
        [ToolTip("Total de passageiros realmente alocados no itinerário.")]
        [PersistentAlias("Passagens[].Count()")]
        public int Alocados
        {
            get { return Convert.ToInt32(EvaluateAlias(nameof(Alocados))); }
        }
        [DataSourceProperty("DiaTransporte.Dia.Passagens")]
        [DataSourceCriteria("[Congregacao.Oid] = '@This.Congregacao.Oid'")]
        [Association("Itinerario-Passagens")]
        public XPCollection<Passagem> Passagens
        {
            get
            {
                return GetCollection<Passagem>(nameof(Passagens));
            }
        }
    }
}
