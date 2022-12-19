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
    [ImageName("Travel_MapPointer"), XafDefaultProperty(nameof(Descricao)),
    NavigationItem("Eventos"), XafDisplayName("Saídas"), Persistent("Saidas")]
    public class Saida : BaseObject
    {
        public Saida(Session session) : base(session)
        { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Endereco = new Endereco(Session);
        }

        Guid saidaApp;
        int parada;
        TimeSpan horario;
        Endereco endereco;
        Congregacao congregacao;

        [VisibleInListView(false)]
        public Guid SaidaApp
        {
            get => saidaApp;
            set => SetPropertyValue(nameof(SaidaApp), ref saidaApp, value);
        }

        [Association("Congregacao-Saidas")]
        public Congregacao Congregacao
        {
            get => congregacao;
            set => SetPropertyValue(nameof(Congregacao), ref congregacao, value);
        }
        
        [Required]
        public int Parada
        {
            get => parada;
            set => SetPropertyValue(nameof(Parada), ref parada, value);
        }

        [XafDisplayName("Descrição")]
        [PersistentAlias("concat(Substring(Horario,0,5), ' - ', Endereco)")]
        public string Descricao
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Descricao))); }
        }
        
        [Required]
        [XafDisplayName("Horário")]
        [ModelDefault("EditMask", "HH:mm")]
        [ModelDefault("DisplayFormat", "HH:mm")]
        public TimeSpan Horario
        {
            get => horario;
            set => SetPropertyValue(nameof(Horario), ref horario, value);
        }
        [XafDisplayName("Endereço")]
        [DevExpress.Xpo.Aggregated]
        public Endereco Endereco
        {
            get => endereco;
            set => SetPropertyValue(nameof(Endereco), ref endereco, value);
        }
    }
}
