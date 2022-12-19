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
    [ImageName("PivotTableGroupField"), XafDefaultProperty(nameof(Data)),
        NavigationItem("Eventos"), XafDisplayName("Dia do Evento"), Persistent("DiaEventos")]
    public class DiaEvento : BaseObject
    {
        public DiaEvento(Session session) : base(session)
        { }


        DateTime data;
        Evento evento;


        [Required]
        public DateTime Data
        {
            get => data;
            set => SetPropertyValue(nameof(Data), ref data, value.Date);
        }

        [Association("Evento-Dias")]
        public Evento Evento
        {
            get => evento;
            set => SetPropertyValue(nameof(Evento), ref evento, value);
        }
        [DevExpress.Xpo.Aggregated]
        [Association("DiaEvento-Transportes")]
        public XPCollection<DiaTransporte> Transportes
        {
            get
            {
                return GetCollection<DiaTransporte>(nameof(Transportes));
            }
        }
        [DevExpress.Xpo.Aggregated]
        [Association("DiaEvento-Passagens")]
        public XPCollection<Passagem> Passagens
        {
            get
            {
                return GetCollection<Passagem>(nameof(Passagens));
            }
        }
    }
}
