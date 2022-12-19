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
    [ImageName("BO_Contact"), XafDefaultProperty(nameof(Nome)),
    NavigationItem("Tabelas"), XafDisplayName("Contatos"), Persistent("Contatos")]
    public class Contato : BaseObject
    {
        public Contato(Session session) : base(session)
        { }

        string celular;
        string email;
        string nome;

        [Required]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nome
        {
            get => nome;
            set => SetPropertyValue(nameof(Nome), ref nome, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        [Required, Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [ModelDefault("EditMask", "(000)90000-0000")]
        [ModelDefault("DisplayFormat", "{0:(###)#####-####}")]
        public string Celular
        {
            get => celular;
            set => SetPropertyValue(nameof(Celular), ref celular, value);
        }
    }
}
