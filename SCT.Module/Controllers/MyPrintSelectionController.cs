using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.XtraReports.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SCT.Module.Controllers
{
    public class MyPrintSelectionController : PrintSelectionBaseController
    {
        protected override void ShowInReport(SingleChoiceActionExecuteEventArgs e, string handle)
        {
            ReportsModuleV2 reportsModule = Application.Modules.FindModule<ReportsModuleV2>();
            try
            {
                reportsModule.ReportsDataSourceHelper.BeforeShowPreview += ReportsDataSourceHelper_BeforeShowPreview;
                base.ShowInReport(e, handle);
            }
            finally
            {
                reportsModule.ReportsDataSourceHelper.BeforeShowPreview -= ReportsDataSourceHelper_BeforeShowPreview;
            }
        }
        void ReportsDataSourceHelper_BeforeShowPreview(object sender, BeforeShowPreviewEventArgs e)
        {
            foreach (Parameter parameter in e.Report.Parameters)
            {
                parameter.Visible = false;
            }
            e.Report.FilterString = String.Empty;
        }
    }
}
