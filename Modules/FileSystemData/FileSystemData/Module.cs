using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Utils;
using System.Diagnostics;

namespace FileSystemData;

[Description("This module provides the FileSystemStoreObject and FileSystemLinkObject classes that enable you to store uploaded files in a file system instead of the database.")]
public sealed class FileSystemDataModule : ModuleBase {
    public static int ReadBytesSize = 0x1000;
    public static string FileSystemStoreLocation = String.Format("{0}FileData", PathHelper.GetApplicationFolder());
    public FileSystemDataModule() {
        BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
    }
    public static void CopyFileToStream(string sourceFileName, Stream destination)
    {
        if (string.IsNullOrEmpty(sourceFileName) || destination == null) return;
        using (Stream source = File.OpenRead(sourceFileName))
            CopyStream(source, destination);
    }
    public static void OpenFileWithDefaultProgram(string sourceFileName)
    {
        Guard.ArgumentNotNullOrEmpty(sourceFileName, "sourceFileName");
        Process.Start(new ProcessStartInfo { FileName = sourceFileName, UseShellExecute = true });
    }
    public static void CopyStream(Stream source, Stream destination)
    {
        if (source == null || destination == null) return;
        byte[] buffer = new byte[ReadBytesSize];
        int read = 0;
        while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            destination.Write(buffer, 0, read);
    }
}
