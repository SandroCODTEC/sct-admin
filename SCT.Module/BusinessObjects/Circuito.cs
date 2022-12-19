using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [ImageName("Actions_FolderClose"), XafDefaultProperty(nameof(Nome)),
        NavigationItem("Tabelas"), XafDisplayName("Circuitos"), Persistent("Circuitos")]
    public class Circuito : BaseObject
    {
        public Circuito(Session session) : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Coordenacao = Session.Query<Coordenacao>().FirstOrDefault();
        }
        Coordenacao coordenacao;
        string nome;

        
        [XafDisplayName("Coordenação")]
        [Association("Coordenacao-Circuitos")]
        public Coordenacao Coordenacao
        {
            get => coordenacao;
            set => SetPropertyValue(nameof(Coordenacao), ref coordenacao, value);
        }
        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value);
        }

        [XafDisplayName("Congregações")]
        [Association("Circuito-Congregacoes")]
        public XPCollection<Congregacao> Congregacoes
        {
            get
            {
                return GetCollection<Congregacao>(nameof(Congregacoes));
            }
        }
    }
}
