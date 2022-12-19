using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraPrinting.Native;
using SCT.Module.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Channels;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("Actions_Label"), XafDefaultProperty(nameof(Passageiro)),
    NavigationItem("Eventos"), XafDisplayName("Passagens"), Persistent("Passagens")]
    [Appearance("PassagemNaoAlocada", AppearanceItemType = "ViewItem", TargetItems = "*",
        Criteria = "Itinerario is null", Context 
        = "ListView", FontStyle = FontStyle.Bold)]
    public class Passagem : BaseObject
    {

        public Passagem(Session session) : base(session)
        { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Grupo = Session.Query<GrupoViagem>().FirstOrDefault(w => w.Descricao == "NENHUM");
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (Itinerario != null && (DiaTransporte == null || DiaTransporte != Itinerario.DiaTransporte))
                DiaTransporte = Itinerario.DiaTransporte;

        }
        public void GerarAssento()
        {
            if (Itinerario != null)
            {

                var totalAssentos = Itinerario.DiaTransporte.Transporte.Assentos;
                var assentos = Itinerario.Passagens
                    .Where(w => w.Oid != Oid)
                    .OrderBy(o => o.Assento)
                    .Select(s => s.Assento).ToArray();

                if (Assento == 0 || assentos.Any(a => a == Assento))
                {
                    for (int i = 1; i <= totalAssentos; i++)
                    {
                        if (!assentos.Any(a => a == i))
                        {
                            Assento = i;
                            break;
                        }
                    }
                }
            }
        }

        int diaApp;
        Guid passagemApp;
        DiaTransporte diaTransporte;
        GrupoViagem grupo;
        int assento;
        Congregacao congregacao;
        Saida saida;
        DiaEvento diaEvento;
        Passageiro passageiro;
        Itinerario itinerario;


        [VisibleInListView(false)]
        public Guid PassagemApp
        {
            get => passagemApp;
            set => SetPropertyValue(nameof(PassagemApp), ref passagemApp, value);
        }
        
        [VisibleInListView(false)]
        public int DiaApp
        {
            get => diaApp;
            set => SetPropertyValue(nameof(DiaApp), ref diaApp, value);
        }
        [Association("DiaEvento-Passagens")]
        public DiaEvento DiaEvento
        {
            get => diaEvento;
            set => SetPropertyValue(nameof(DiaEvento), ref diaEvento, value);
        }
        
        public DiaTransporte DiaTransporte
        {
            get => diaTransporte;
            set => SetPropertyValue(nameof(DiaTransporte), ref diaTransporte, value);
        }
        [XafDisplayName("Itinerário")]
        [Association("Itinerario-Passagens")]
        [DataSourceCriteria("[DiaTransporte].[Dia].[Oid] = '@This.DiaEvento.Oid'")]
        [Appearance(id: "ItinerarioEnabled", Enabled = false)]
        public Itinerario Itinerario
        {
            get => itinerario;
            set => SetPropertyValue(nameof(Itinerario), ref itinerario, value);
        }
        [Required]
        [DataSourceProperty("DiaEvento.Evento.Congregacoes")]
        [XafDisplayName("Congregação")]
        public Congregacao Congregacao
        {
            get => congregacao;
            set => SetPropertyValue(nameof(Congregacao), ref congregacao, value);
        }
        [Required]
        [DataSourceProperty("Congregacao.Passageiros")]
        public Passageiro Passageiro
        {
            get => passageiro;
            set => SetPropertyValue(nameof(Passageiro), ref passageiro, value);
        }
        
        [ToolTip("Você pode definir quais pessoas viajarão juntas no mesmo transporte.")]
        public GrupoViagem Grupo
        {
            get => grupo;
            set => SetPropertyValue(nameof(Grupo), ref grupo, value);
        }
        [Required]
        [DataSourceProperty("Passageiro.Congregacao.Saidas")]
        [XafDisplayName("Saída")]
        public Saida Saida
        {
            get => saida;
            set => SetPropertyValue(nameof(Saida), ref saida, value);
        }
        
        public int Assento
        {
            get => assento;
            set => SetPropertyValue(nameof(Assento), ref assento, value);
        }
        [Association("Passagem-Dependentes")]
        public XPCollection<Dependente> Dependentes
        {
            get
            {
                return GetCollection<Dependente>(nameof(Dependentes));
            }
        }
    }
}
