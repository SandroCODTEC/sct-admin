using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using SCT.Module.BusinessObjects;
using System.Drawing;
using DevExpress.XtraReports.Native;
using System.Runtime.ConstrainedExecution;
using DevExpress.XtraReports.UI;
using System.Xml.Linq;
using DevExpress.ExpressApp.Dashboards;

namespace SCT.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater
{
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion)
    {
    }
    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();
        PermissionPolicyRole defaultRole = CreateDefaultRole();

        ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "SCT");
        if (userAdmin == null)
        {
            userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "SCT";
            // Set a password if the standard authentication type is used
            userAdmin.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
        }
        // If a role with the Administrators name doesn't exist in the database, create this role
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrador");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrador";
        }
        adminRole.IsAdministrative = true;
        userAdmin.Roles.Add(adminRole);
        ObjectSpace.CommitChanges(); //This line persists created object(s).

        var grupos = new[] {
            (1, Color.IndianRed) ,
            (2, Color.Red) ,
            (3, Color.Maroon),
            (4, Color.Tomato),
            (5, Color.Chocolate),
            (6, Color.Peru),
            (7, Color.SaddleBrown),
            (8, Color.OrangeRed),
            (9, Color.DarkOrange),
            (10, Color.Tan),
            (11, Color.Orange),
            (12, Color.Goldenrod),
            (13, Color.DarkKhaki),
            (14, Color.Olive),
            (15, Color.OliveDrab),
            (16, Color.DarkOliveGreen),
            (17, Color.DarkSeaGreen),
            (18, Color.Green),
            (19, Color.Teal),
            (20, Color.CadetBlue),
            (21, Color.SteelBlue),
            (22, Color.RoyalBlue),
            (23, Color.Blue),
            (24, Color.DarkBlue),
            (25, Color.BlueViolet),
            (26, Color.MediumOrchid),
            (27, Color.Purple),
            (28, Color.DarkViolet),
            (29, Color.DeepPink),
            (30, Color.PaleVioletRed),
        };
        NovoGrupo("NENHUM", Color.Black);
        foreach (var g in grupos)
            NovoGrupo($"GRUPO {g.Item1}", g.Item2);

        if (ObjectSpace.GetObjectsCount(typeof(Coordenacao), null) == 0)
        {
            Coordenacao singleton = ObjectSpace.CreateObject<Coordenacao>();
            singleton.Descricao = "SISTEMA COORDENADO DE TRANSPORTE";
            CreateTest(singleton);
        }

        var report1 = CreateReport(userAdmin.Session, "IDENTIFICAÇÃO DOS TRANSPORTES", typeof(DiaTransporte));
        var report2 = CreateReport(userAdmin.Session, "ITINERÁRIO DOS TRANSPORTES", typeof(DiaTransporte));
        var report3 = CreateReport(userAdmin.Session, "LISTA DE PASSAGEIROS", typeof(Passagem));

        var dashboard1 = CreateDashboard("ACOMPANHAMENTO FINANCEIRO");
        var dashboard2 = CreateDashboard("ACOMPANHAMENTO LOGÍSTICO");

        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }
    ReportDataV2 CreateReport(Session session, string defaultUrl, Type type)
    {
        ReportDataV2 item = ObjectSpace.GetObjects<ReportDataV2>(new BinaryOperator("DisplayName", defaultUrl)).FirstOrDefault();

        if (item != null)
            return item;

        var fileName = $"{Environment.CurrentDirectory}\\Reports\\{defaultUrl}.repx";

        if (!System.IO.File.Exists(fileName))
            throw new Exception($"Relatório '{fileName}' não encontrado!");

        XtraReport report = new XtraReport();
        report.LoadLayout(fileName);

        //item = ObjectSpace.CreateObject<ReportDataV2>();
        item = new ReportDataV2(session, type);

        item.DisplayName = defaultUrl;
        using (MemoryStream ms = new MemoryStream())
        {
            report.SaveLayoutToXml(ms);
            item.Content = ms.ToArray();
        }

        ObjectSpace.CommitChanges();

        return item;
    }

    DashboardData CreateDashboard(string defaultUrl)
    {
        DashboardData item = ObjectSpace.GetObjects<DashboardData>(new BinaryOperator("Title", defaultUrl)).FirstOrDefault();

        if (item != null)
            return item;

        var fileName = $"Dashboards\\{defaultUrl}.xml";

        if (!System.IO.File.Exists(fileName))
            throw new Exception($"Dashboard '{fileName}' não encontrado!");

        XDocument dashboardXml = XDocument.Load(fileName);
        item = ObjectSpace.CreateObject<DashboardData>();

        item.Content = dashboardXml.ToString();
        item.Title = defaultUrl;
        ObjectSpace.CommitChanges();

        return item;
    }

    private void NovoGrupo(string descricao, Color cor)
    {
        GrupoViagem grupo = ObjectSpace.FirstOrDefault<GrupoViagem>(r => r.Descricao == descricao);
        if (grupo == null)
        {
            grupo = ObjectSpace.CreateObject<GrupoViagem>();
            grupo.Descricao = descricao;
            grupo.Cor = cor;
        }
    }
    void CreateTest(Coordenacao coordenacao)
    {
        var circuitos = new[] {
            "YX1",
            "YX2",
        };
        foreach (var cir in circuitos)
        {
            Circuito circuito = ObjectSpace.FirstOrDefault<Circuito>(r => r.Nome == cir);
            if (circuito == null)
            {
                circuito = ObjectSpace.CreateObject<Circuito>();
                circuito.Nome = cir;

                if (cir == "YX1")
                {
                    var congregacoes = new[] {
                        "CONGREGAÇÃO 1",
                    };
                    foreach (var cong in congregacoes)
                    {
                        Congregacao congregacao = ObjectSpace.FirstOrDefault<Congregacao>(r => r.Nome == cong);
                        if (congregacao == null)
                        {
                            congregacao = ObjectSpace.CreateObject<Congregacao>();
                            congregacao.Circuito = circuito;
                            congregacao.Nome = cong;
                            Saida saida = ObjectSpace.CreateObject<Saida>();
                            saida.Horario = new TimeSpan(8, 0, 0);
                            saida.Endereco.Logradouro = "LOCAL 1";
                            congregacao.Saidas.Add(saida);

                            for (int i = 1; i <= 2; i++)
                            {
                                Passageiro passageiro = ObjectSpace.CreateObject<Passageiro>();
                                passageiro.Nome = $"PASSAGEIRO {i} {cong}";
                                passageiro.Save();
                                congregacao.Passageiros.Add(passageiro);
                            }
                        }
                        congregacao.Save();
                    }
                }
                circuito.Save();
            }
            coordenacao.Circuitos.Add(circuito);
            coordenacao.Save();
        }
        ObjectSpace.CommitChanges(); //This line persists created object(s).

        Fornecedor fornecedor = ObjectSpace.CreateObject<Fornecedor>();
        fornecedor.Nome = "FORNECEDOR 1";
        Transporte transporte = ObjectSpace.CreateObject<Transporte>();
        transporte.Descricao = "ÔNIBUS 1";
        transporte.Assentos = 50;
        fornecedor.Transportes.Add(transporte);

        Evento evento = ObjectSpace.CreateObject<Evento>();
        evento.Descricao = "CONGRESSO 2023";
        evento.Tema = "TEMA CONGRESSO 2023";
        evento.DataInicial = new DateTime(2023, 9, 15);
        evento.DataFinal = new DateTime(2023, 9, 17);
        evento.Lista = new DateTime(2023, 9, 15);
        evento.Pagamento = new DateTime(2023, 9, 30);
        evento.CorFundo = Color.Purple;
        evento.CorContraste = Color.MediumPurple;
        evento.CorFonte = Color.Black;
        evento.Coordenacao = coordenacao;

        var congs = ObjectSpace.GetObjects<Congregacao>();
        evento.Congregacoes.AddRange(congs);

        for (DateTime data = evento.DataInicial; data <= evento.DataFinal; data.AddDays(1))
        {
            var diaEvento = ObjectSpace.CreateObject<DiaEvento>();
            diaEvento.Evento = evento;
            diaEvento.Data = data;
            for (int x = 1; x <= 9; x++)
            {
                var diaTransporte = ObjectSpace.CreateObject<DiaTransporte>();
                diaTransporte.Dia = diaEvento;
                diaTransporte.Transporte = transporte;
                diaTransporte.Valor = 2000;
            }
        }
    }

    public override void UpdateDatabaseBeforeUpdateSchema()
    {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateDefaultRole()
    {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null)
        {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
