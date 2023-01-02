using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraReports.Native;
using Newtonsoft.Json;
using SCT.Module.BusinessObjects;
using SCT.Module.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace SCT.Module.Controllers
{
    public partial class ImportViewController : ViewController
    {
        public ImportViewController()
        {
            InitializeComponent();

            TargetViewType = ViewType.DetailView;
            TargetObjectType = typeof(Importacao);

            SimpleAction clearTasksAction = new SimpleAction(this, "ClearTaskAction", PredefinedCategory.View)
            {
                Caption = "Processar Arquivo",
                ImageName = "ProductSpecificationsSummary",
                TargetObjectsCriteria = "Processado = False",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };
            //This event fires when a user clicks the Simple Action control.
            clearTasksAction.Execute += Import_Execute;
        }
        bool ValidarFile(string file)
        {
            switch (file)
            {
                case "keyError":
                    Application.ShowViewStrategy.ShowMessage("A chafe utilizada não está correta!", InformationType.Error, 6000, InformationPosition.Bottom);
                    return false;
                case "":
                    Application.ShowViewStrategy.ShowMessage("Não foi possível decriptografar os dados!", InformationType.Error);
                    return false;
                case null:
                    Application.ShowViewStrategy.ShowMessage("Não foi possível decriptografar os dados. O resultado foi nulo!", InformationType.Error);
                    return false;
                default:
                    return true;
            }
        }
        bool ValidarData(SCTData sctData, Importacao item)
        {
            if (item.Evento.DataInicial.Date != sctData.Evento.DataInicial.Date)
            {
                Application.ShowViewStrategy.ShowMessage("A data inicial do evento difere da data informada pelo SCT!", InformationType.Error, 6000, InformationPosition.Bottom);
                return false;
            }
            else if (item.Evento.DataFinal.Date != sctData.Evento.DataFinal.Date)
            {
                Application.ShowViewStrategy.ShowMessage("A data final do evento difere da data informada pelo SCT!", InformationType.Error, 6000, InformationPosition.Bottom);
                return false;
            }
            return true;
        }
        private void Import_Execute(Object sender, SimpleActionExecuteEventArgs e)
        {
            var result = false;
            var item = e.CurrentObject as Importacao;


            var file = Cripto.DecryptStringAES(item.Chave, File.ReadAllText(item.Arquivo.RealFileName, Encoding.UTF8));
            if (!ValidarFile(file)) return;

            SCTData sctData = JsonConvert.DeserializeObject<SCTData>(file);
            if (!ValidarData(sctData, item)) return;


            //var listPassageiros = new List<Passageiro>();
            //var listDependentes = new List<Dependente>();
            //var listSaidas = new List<Saida>();
            try
            {

                foreach (var saida in sctData.Saidas)
                {
                    var novaSaida = ObjectSpace.GetObjects<Saida>(new BinaryOperator(nameof(Saida.SaidaApp), saida.Oid), true).FirstOrDefault();
                    if (novaSaida == null)
                        novaSaida = ObjectSpace.CreateObject<Saida>();

                    novaSaida.Congregacao = item.Congregacao;
                    novaSaida.SaidaApp = saida.Oid;
                    novaSaida.Parada = saida.Parada;
                    novaSaida.Horario = saida.Horario.TimeOfDay;
                    novaSaida.Endereco.Logradouro = saida.Logradouro;
                    novaSaida.Endereco.Numero = saida.Numero;
                    novaSaida.Endereco.Complemento = saida.Complemento;
                    novaSaida.Endereco.Bairro = saida.Bairro;
                    novaSaida.Endereco.Cidade = saida.Cidade;
                    novaSaida.Endereco.UF = saida.UF;
                    novaSaida.Endereco.Longitude = saida.Longitude;
                    novaSaida.Endereco.Latitude = saida.Latitude;
                }
                item.Congregacao.Coordenador.Nome = sctData.Congregacao.Responsavel;
                item.Congregacao.Coordenador.Celular = sctData.Congregacao.CelularResponsavel;
                item.Congregacao.Coordenador.Email = sctData.Congregacao.EmailResponsavel;
                item.Congregacao.Ajudante.Nome = sctData.Congregacao.Ajudante;
                item.Congregacao.Ajudante.Celular = sctData.Congregacao.CelularAjudante;
                item.Congregacao.Ajudante.Email = sctData.Congregacao.EmailAjudante;

                foreach (var passageiro in sctData.Passageiros)
                {
                    var passageiroImport = ObjectSpace.FindObject<Passageiro>(new BinaryOperator(nameof(Passageiro.PassageiroApp), passageiro.Oid), true);
                    if (passageiroImport == null)
                        passageiroImport = ObjectSpace.CreateObject<Passageiro>();
                    passageiroImport.PassageiroApp = passageiro.Oid;
                    passageiroImport.Congregacao = item.Congregacao;
                    passageiroImport.Nome = passageiro.Nome;
                    passageiroImport.TipoDocumento = (TipoDocumento)Enum.Parse(typeof(TipoDocumento), passageiro.TipoDocumento);
                    passageiroImport.Documento = passageiro.Documento;

                    foreach (var dependente in sctData.Dependentes.Where(w => w.Passageiro.Oid == passageiro.Oid))
                    {
                        var dependenteImport = ObjectSpace.FindObject<Dependente>(new BinaryOperator(nameof(Dependente.DependenteApp), dependente.Oid), true);
                        if (dependenteImport == null)
                            dependenteImport = ObjectSpace.CreateObject<Dependente>();
                        dependenteImport.DependenteApp = dependente.Oid;
                        dependenteImport.Passageiro = passageiroImport;
                        dependenteImport.Nome = dependente.Nome;
                        dependenteImport.TipoDocumento = (TipoDocumento)Enum.Parse(typeof(TipoDocumento), dependente.TipoDocumento);
                        dependenteImport.Documento = dependente.Documento;
                        passageiroImport.Dependentes.Add(dependenteImport);
                    }
                }

                foreach (var passagem in sctData.Passagens)
                {
                    foreach (var dia in passagem.Dias)
                    {
                        int vAno = int.Parse(dia.ToString().Substring(0, 4));
                        int vMes = int.Parse(dia.ToString().Substring(4, 2)) + 1;
                        int vDia = int.Parse(dia.ToString().Substring(6, 2));
                        var dataDia = new DateTime(vAno, vMes, vDia);

                        var diaEvento = item.Evento.Dias.FirstOrDefault(d => d.Data == dataDia);

                        CriteriaOperator op = CriteriaOperator.And(new BinaryOperator(nameof(Passagem.PassagemApp), passagem.Oid), new BinaryOperator(nameof(Passagem.DiaApp), dia));

                        var passagemImport = ObjectSpace.FindObject<Passagem>(op, true);
                        if (passagemImport == null)
                            passagemImport = ObjectSpace.CreateObject<Passagem>();

                        passagemImport.PassagemApp = passagem.Oid;
                        passagemImport.DiaApp = dia;
                        passagemImport.DiaEvento = diaEvento;
                        passagemImport.Congregacao = item.Congregacao;
                        passagemImport.Passageiro = ObjectSpace.FindObject<Passageiro>(new BinaryOperator(nameof(Passageiro.PassageiroApp), passagem.Passageiro.Oid), true);
                        passagemImport.Grupo = ObjectSpace.FindObject<GrupoViagem>(new BinaryOperator(nameof(GrupoViagem.Descricao), passagem.Grupo.Oid == 0 ? "NENHUM" : $"GRUPO {passagem.Grupo.Oid}"));
                        passagemImport.Saida = ObjectSpace.FindObject<Saida>(new BinaryOperator(nameof(Saida.SaidaApp), passagem.Saida.Oid), true);
                        AddDependentes(passagemImport, passagem);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, 6000, InformationPosition.Bottom);
                ObjectSpace.Rollback();
                return;
            }
            if (result)
            {
                item.DataImportacao = DateTime.Now;
                item.Processado = true;
                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage("Dados importados com sucesso!", InformationType.Success, 10000, InformationPosition.Bottom);
            }
        }
        void AddDependentes(Passagem passagemImport, EPassagem passagem)
        {
            foreach (var dependente in passagem.Dependentes)
            {
                if (!passagemImport.Dependentes.Any(d => d.Oid == dependente))
                    passagemImport.Dependentes.Add(
                        ObjectSpace.FindObject<Dependente>(new BinaryOperator(nameof(Dependente.DependenteApp), dependente), true));
            }
            foreach (var dependente in passagemImport.Dependentes)
            {
                if (!passagem.Dependentes.Any(d => d == dependente.DependenteApp))
                    passagemImport.Dependentes.Remove(dependente);
            }
        }

        public string EncodeBase64(string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public string DecodeBase64(string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            var text = Encoding.UTF8.GetString(valueBytes);
            return text;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
