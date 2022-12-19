using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SCT.Module.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO Criar relatório: Itinerário
//TODO Criar relatório: Relatório de avaliação
//TODO Criar relatório: Relatório financeiro
//TODO Criar relatório: Relação de depósitos
//TODO Criar relatório: Relação de depósitos com imagens
//TODO Criar dashboard: Acompanhamento financeiro
//TODO Criar dashboard: Acompanhamento logístico

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("BO_Position"), NavigationItem("Tabelas"), XafDisplayName("Coordenação"), Persistent("Coordenacao")]
    [DefaultProperty(nameof(Descricao))]
    [RuleObjectExists("CoordenacaoSingletonExists", DefaultContexts.Save, "True", InvertResult = true,
    CustomMessageTemplate = "O cadastro de Coordenação já existe.")]
    [RuleCriteria("CoordencaoCannotDeleteSingleton", DefaultContexts.Delete, "False",
    CustomMessageTemplate = "Você não pode excluir o cadastro de Coordenação.")]
    public class Coordenacao : BaseObject
    {
        public Coordenacao(Session session) : base(session)
        { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Coordenador = new Contato(Session);
            Ajudante = new Contato(Session);
        }
        int chegada;
        string descricao;
        Contato ajudante;
        Contato coordenador;


        [XafDisplayName("Descrição")]
        [Required, Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Descricao
        {
            get => descricao;
            set => SetPropertyValue(nameof(Descricao), ref descricao, value);
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
        [ToolTip("Informa com quantos minutos o transporte deve chegar no local do primeiro embarque.")]
        public int Chegada
        {
            get => chegada;
            set => SetPropertyValue(nameof(Chegada), ref chegada, value);
        }
        [Association("Coordenacao-Circuitos")]
        public XPCollection<Circuito> Circuitos
        {
            get
            {
                return GetCollection<Circuito>(nameof(Circuitos));
            }
        }
    }
}
