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
    [ImageName("BO_Contact"), XafDefaultProperty(nameof(Descricao)),
        NavigationItem("Tabelas"), XafDisplayName("Endereços"), Persistent("Enderecos")]
    public class Endereco : BaseObject
    {
        public Endereco(Session session) : base(session)
        { }

        decimal latitude;
        decimal longitude;
        string uF;
        string cidade;
        string bairro;
        string complemento;
        string numero;
        string logradouro;

        [XafDisplayName("Descrição")]
        [PersistentAlias("concat(Logradouro, ', ', Numero, ' ',Complemento, ', ', Bairro, ' - ', Cidade, ' - ', UF)")]
        public string Descricao
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Descricao))); }
        }
        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Logradouro
        {
            get => logradouro;
            set => SetPropertyValue(nameof(Logradouro), ref logradouro, value?.ToUpper());
        }

        [Required]
        [XafDisplayName("Número")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value?.ToUpper());
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Complemento
        {
            get => complemento;
            set => SetPropertyValue(nameof(Complemento), ref complemento, value?.ToUpper());
        }

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Bairro
        {
            get => bairro;
            set => SetPropertyValue(nameof(Bairro), ref bairro, value?.ToUpper());
        }

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Cidade
        {
            get => cidade;
            set => SetPropertyValue(nameof(Cidade), ref cidade, value?.ToUpper());
        }

        [Required]
        [Size(2)]
        public string UF
        {
            get => uF;
            set => SetPropertyValue(nameof(UF), ref uF, value?.ToUpper());
        }

        public decimal Longitude
        {
            get => longitude;
            set => SetPropertyValue(nameof(Longitude), ref longitude, value);
        }

        public decimal Latitude
        {
            get => latitude;
            set => SetPropertyValue(nameof(Latitude), ref latitude, value);
        }
    }
}
