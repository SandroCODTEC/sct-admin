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
    [ImageName("Travel_Bus"), XafDefaultProperty(nameof(Descricao)),
    NavigationItem("Tabelas"), XafDisplayName("Transportes"), Persistent("Transportes")]
    public class Transporte : BaseObject
    {
        public Transporte(Session session) : base(session)
        { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Ativo = true;
        }

        bool ativo;
        int assentos;
        Fornecedor fornecedor;
        string descricao;

        [XafDisplayName("Nome")]
        [PersistentAlias("concat(Descricao, ' (', Assentos, ')')")]
        public string Nome
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Nome))); }
        }

        [Required]
        [XafDisplayName("Descrição")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Descricao
        {
            get => descricao;
            set => SetPropertyValue(nameof(Descricao), ref descricao, value);
        }

        [Association("Fornecedor-Transportes")]
        public Fornecedor Fornecedor
        {
            get => fornecedor;
            set => SetPropertyValue(nameof(Fornecedor), ref fornecedor, value);
        }

        public int Assentos
        {
            get => assentos;
            set => SetPropertyValue(nameof(Assentos), ref assentos, value);
        }

        public bool Ativo
        {
            get => ativo;
            set => SetPropertyValue(nameof(Ativo), ref ativo, value);
        }
    }
}
