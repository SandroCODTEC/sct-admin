using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SCT.Module.Controllers;
using System;
using System.ComponentModel;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("BO_Contact"), XafDefaultProperty(nameof(Descricao)),
        NavigationItem("Eventos"), XafDisplayName("Transporte do dia"), Persistent("DiaTransportes")]
    public class DiaTransporte : BaseObject, IExportDoc
    {
        public DiaTransporte(Session session) : base(session)
        { }

        string motorista;
        TimeSpan horarioPartida;
        string celular;
        string responsavel;
        TimeSpan retorno;
        string identificacao;
        decimal valor;
        int assentos;
        Transporte transporte;
        DiaEvento dia;

        [XafDisplayName("Descrição")]
        [PersistentAlias("concat(Identificacao, ' (', Assentos,'|', Alocados,')')")]
        public string Descricao
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Descricao))); }
        }

        [Association("DiaEvento-Transportes")]
        public DiaEvento Dia
        {
            get => dia;
            set => SetPropertyValue(nameof(Dia), ref dia, value);
        }

        [Required]
        [DataSourceCriteria("Ativo=True")]
        public Transporte Transporte
        {
            get => transporte;
            set
            {
                if (transporte == value)
                {
                    return;
                }
                Assentos = value == null ? 0 : value.Assentos;
                SetPropertyValue(nameof(Transporte), ref transporte, value);
            }
        }

        [Required]
        [XafDisplayName("Identificação"), Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Identificacao
        {
            get => identificacao;
            set => SetPropertyValue(nameof(Identificacao), ref identificacao, value);
        }
        public int Assentos
        {
            get => assentos;
            set => SetPropertyValue(nameof(Assentos), ref assentos, value);
        }
        [PersistentAlias("Itinerarios[].Sum(Alocados)")]
        public int Alocados
        {
            get { return Convert.ToInt32(EvaluateAlias(nameof(Alocados))); }
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Motorista
        {
            get => motorista;
            set => SetPropertyValue(nameof(Motorista), ref motorista, value);
        }
        [XafDisplayName("Horário de Partida")]
        public TimeSpan HorarioPartida
        {
            get => horarioPartida;
            set => SetPropertyValue(nameof(HorarioPartida), ref horarioPartida, value);
        }
        public TimeSpan Retorno
        {
            get => retorno;
            set => SetPropertyValue(nameof(Retorno), ref retorno, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Responsavel
        {
            get => responsavel;
            set => SetPropertyValue(nameof(Responsavel), ref responsavel, value);
        }

        [Required, Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [ModelDefault("EditMask", "(000)90000-0000")]
        [ModelDefault("DisplayFormat", "{0:(###)#####-####}")]
        public string Celular
        {
            get => celular;
            set => SetPropertyValue(nameof(Celular), ref celular, value);
        }
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [XafDisplayName("Itinerários")]
        [Association("DiaTransporte-Itinerarios"), DevExpress.Xpo.Aggregated]
        public XPCollection<Itinerario> Itinerarios
        {
            get
            {
                return GetCollection<Itinerario>(nameof(Itinerarios));
            }
        }
    }
}
