using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraEditors;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Design;

namespace SCT.Win;

public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) {
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<SCTWindowsFormsApplication>();
        builder.Modules
            .AddConditionalAppearance()
            .AddDashboards(options => {
                options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
                options.DesignerFormStyle = DevExpress.XtraBars.Ribbon.RibbonFormStyle.Ribbon;
            })
            .AddFileAttachments(options =>
            {
                options.CustomOpenFileWithDefaultProgram += (object sender, DevExpress.ExpressApp.FileAttachments.Win.CustomFileOperationEventArgs e) =>
    {
        if ((e.FileData is FileSystemData.BusinessObjects.FileSystemLinkObject)
            || (e.FileData is FileSystemData.BusinessObjects.FileSystemStoreObject))
        {
            e.FileData.SaveToStream(null);
            e.Handled = true;
        }
    };
            })
            .AddOffice()
            .AddReports(options => {
                options.EnableInplaceReports = true;
                options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
                options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
                options.ShowAdditionalNavigation = true;
            })
            .AddValidation(options => {
                options.AllowValidationDetailsAccess = false;
            })
            .Add<SCT.Module.SCTModule>()
        	.Add<SCTWinModule>();
        builder.ObjectSpaceProviders
            .AddSecuredXpo((application, options) => {
                options.ConnectionString = connectionString;
            })
            .AddNonPersistent();
        builder.Security
            .UseIntegratedMode(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(SCT.Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(SCT.Module.BusinessObjects.ApplicationUserLoginInfo);
                options.UseXpoPermissionsCaching();
            })
            .UsePasswordAuthentication();
        builder.AddBuildStep(application => {
            application.ConnectionString = connectionString;
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        });
        var winApplication = builder.Build();
        return winApplication;
    }

    XafApplication IDesignTimeApplicationFactory.Create()
        => BuildApplication(XafApplication.DesignTimeConnectionString);
}
