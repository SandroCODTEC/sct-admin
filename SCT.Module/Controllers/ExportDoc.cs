using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SCT.Module.BusinessObjects;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace SCT.Module.Controllers
{
    public interface IExportDoc { }

    public class DocumentsExportController : ViewController
    {
        SingleChoiceAction documentstFilterAction;
        public DocumentsExportController()
        {
            TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(IExportDoc);
            documentstFilterAction = new SingleChoiceAction(this, "DocumentsFilter", "View")
            {
                Caption = "Gerar Documentos",
                ImageMode = ImageMode.UseActionImage,
                ImageName = "CustomizeMergeField",
                PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage,
                ConfirmationMessage = "Tem certeza que deseja gerar os documentos agora?",
                ItemType = SingleChoiceActionItemType.ItemIsOperation,
            };
            documentstFilterAction.Execute += documentstFilterAction_Execute;

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            documentstFilterAction.Items.Clear();
            foreach (Documento document in ObjectSpace.GetObjects<Documento>().Where(w => w.DataType.Name == View.ObjectTypeInfo.Name))
            {
                documentstFilterAction.Items.Add(new ChoiceActionItem(document.Name, document.Oid) { ImageName = "ExportToPDF" });
            }
        }
        void documentstFilterAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var oid = Guid.Parse(e.SelectedChoiceActionItem.Data.ToString());
            var document = ObjectSpace.FirstOrDefault<Documento>(w => w.Oid == oid);
            ExportFiles(document, e);
        }
        private void ExportFiles(Documento document, SingleChoiceActionExecuteEventArgs e)
        {
            if (!Directory.Exists("Docs"))
            {
                Directory.CreateDirectory("Docs");
            }

            using (RichEditDocumentServer docTemp = new RichEditDocumentServer())
            using (RichEditDocumentServer docServer = new RichEditDocumentServer())
            {
                var datasource = e.SelectedObjects;
                docTemp.Options.MailMerge.DataSource = datasource;
                docTemp.Options.MailMerge.ViewMergedData = true;
                docTemp.LoadDocument(document.Template);

                MailMergeOptions options = docTemp.CreateMailMergeOptions();

                for (int i = 0; i < e.SelectedObjects.Count; i++)
                {
                    var item = e.SelectedObjects[i];
                    string filename = $"Docs\\{string.Concat(document.Name.Split(Path.GetInvalidFileNameChars()))} - {string.Concat(item.ToString().Split(Path.GetInvalidFileNameChars()))}.pdf";

                    options.FirstRecordIndex = options.LastRecordIndex = i;

                    using (FileStream fs = new FileStream(filename, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        docTemp.MailMerge(options, docServer.Document);
                        docServer.ExportToPdf(fs);
                    }
                }
            }

            Process.Start("explorer.exe", "Docs");
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

    //public partial class ExportDocController : ViewController
    //{
    //    public ExportDocController()
    //    {
    //        InitializeComponent();

    //        TargetViewType = ViewType.ListView;
    //        TargetObjectType = typeof(IExportDoc);

    //        SimpleAction exportDocAction = new SimpleAction(this, "ExportDocAction", PredefinedCategory.View)
    //        {
    //            Caption = "Exportar Documentos",
    //            ImageName = "CustomizeMergeField",
    //        };
    //        exportDocAction.Execute += export_Execute;
    //    }

    //    private void export_Execute(Object sender, SimpleActionExecuteEventArgs e)
    //    {
    //        if(e.SelectedObjects.Count == 0)
    //        {
    //            Application.ShowViewStrategy.ShowMessage("Selecione pelo menos 1 registro!", InformationType.Warning);
    //            return;
    //        }
    //        ExportFiles(e);
    //    }

    //    private void ExportFiles(SimpleActionExecuteEventArgs e)
    //    {
    //        if (!Directory.Exists("Docs"))
    //        {
    //            Directory.CreateDirectory("Docs");
    //        }

    //        using (RichEditDocumentServer docTemp = new RichEditDocumentServer())
    //        using (RichEditDocumentServer docServer = new RichEditDocumentServer())
    //        {
    //            var datasource = e.SelectedObjects;
    //            docTemp.Options.MailMerge.DataSource = datasource;
    //            docTemp.Options.MailMerge.ViewMergedData = true;
    //            docTemp.LoadDocument("MailMergeSimple.rtf");

    //            MailMergeOptions options = docTemp.CreateMailMergeOptions();

    //            for (int i = 0; i < e.SelectedObjects.Count; i++)
    //            {
    //                var item = e.SelectedObjects[i];
    //                string filename = $"{string.Concat(item.ToString().Split(Path.GetInvalidFileNameChars()))}.pdf";

    //                options.FirstRecordIndex = options.LastRecordIndex = i;

    //                using (FileStream fs = new FileStream(filename, FileMode.Create, System.IO.FileAccess.Write))
    //                {
    //                    docTemp.MailMerge(options, docServer.Document);
    //                    docServer.ExportToPdf(fs);
    //                }
    //            }
    //        }
    //    }

    //    protected override void OnActivated()
    //    {
    //        base.OnActivated();
    //    }
    //    protected override void OnViewControlsCreated()
    //    {
    //        base.OnViewControlsCreated();
    //    }
    //    protected override void OnDeactivated()
    //    {
    //        base.OnDeactivated();
    //    }
    //}
}
