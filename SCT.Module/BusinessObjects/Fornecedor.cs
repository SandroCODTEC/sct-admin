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
    [DefaultClassOptions, ImageName("Travel_Bus"), DefaultProperty(nameof(Nome)),
        NavigationItem("Tabelas"), XafDisplayName("Fornecedores"), Persistent("Fornecedores")]
    public class Fornecedor : BaseObject, IExportDoc
    {
        public Fornecedor(Session session) : base(session)
        { }


        string dadosDeposito;
        string nome;

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value);
        }
        [XafDisplayName("Dados para Depósito")]
        [Size(SizeAttribute.Unlimited)]
        public string DadosDeposito
        {
            get => dadosDeposito;
            set => SetPropertyValue(nameof(DadosDeposito), ref dadosDeposito, value);
        }
        [Association("Fornecedor-Transportes"), DevExpress.Xpo.Aggregated]
        public XPCollection<Transporte> Transportes
        {
            get
            {
                return GetCollection<Transporte>(nameof(Transportes));
            }
        }
    }
}
