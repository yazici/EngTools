using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Lib.Models
{
    public static class Settings
    {
        public const string MetadataExtension = "gdometa";

        public static string ProgramFiles = @"c:\Program Files";// Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static string ProgramFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

        private static readonly string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string RoamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string WorkspaceRoot = @"d:\Workspace";
        //public static string WorkspaceRoot = @"c:\MySrc";

        public static string EngTools => $"{WorkspaceRoot}\\EngTools";
        public static string EngData => $"{EngTools}\\Data";

        
        public static string EngTemp => $"{EngTools}\\Temp";

        public static string EngContent => $"{EngTools}\\Content";

        public static string EngTempUnpackRoot => $"{EngTemp}\\UnPack";

        public static string EngHarvestRoot => $"{EngContent}\\Harvest";

        public static string LocalHtmlPath => $"{EngData}\\local.html";

        public static Uri LocalHtmlUri => new Uri(LocalHtmlPath);

        public static string VoiceSaveDirectory = $"{EngTools}\\Voice";

        // unityEditorLogsPath  = C:\Users\Baris\AppData\Local\Unity\Editor
        // C:\Users\Baris\AppData\LocalLow\Unity
        //C:\Users\Baris\AppData\LocalLow\Unity Technologies\Tanks!!!
        //C:\Users\Baris\AppData\Roaming\Unity\Asset Store-5.x
        // C:\Users\Baris\AppData\Roaming\Unity\Editor-5.x\Preferences
        // C:\Users\Baris\AppData\Roaming\Unity\Packages (nuget npm)

        //LinkLabel.Link link = new LinkLabel.Link();
        //link.LinkData = "https://onedrive.live.com/?cid=5F18BACBA5E0611C&id=5F18BACBA5E0611C%212202";
        //lblOneDrive.Links.Add(link);

        //link = new LinkLabel.Link();
        //link.LinkData = "https://onedrive.live.com/edit.aspx?cid=5F18BACBA5E0611C&resid=5F18BACBA5E0611C%212206&app=OneNote";
        //lblOneNote.Links.Add(link);

        //link = new LinkLabel.Link();
        //link.LinkData = "https://onedrive.live.com/?cid=5F18BACBA5E0611C&id=5F18BACBA5E0611C%212205";
        //lblMindMap.Links.Add(link);

        //link = new LinkLabel.Link();
        //link.LinkData = "https://onedrive.live.com/edit.aspx?cid=5F18BACBA5E0611C&resid=5F18BACBA5E0611C%212204&app=Excel&wdo=1";
        //lblCheckLists.Links.Add(link);

        public static string unity3DAppFolderPath = $"{ProgramFiles}\\Unity";
        public static string Unity3DAppFolderPath
        {
            get
            {
                return unity3DAppFolderPath;
            }

            set
            {
                if (value == unity3DAppFolderPath)
                {
                    return;
                }
                unity3DAppFolderPath = value;
            }
        }


        public static string SevenZipCli = $"{ProgramFiles}\\7-Zip\\7z.exe";

        public static string AppsRoot = "c:\\Apps";
        public static string GzipExe = $"{AppsRoot}\\gzip-1.3.12-1-bin\\bin\\gzip.exe";

        public static string unity3DAssetsFolderPath = $"{RoamingAppData}\\Unity\\Asset Store-5.x";
        public static string Unity3DAssetsFolderPath
        {
            get
            {
                return unity3DAssetsFolderPath;
            }

            set
            {
                if (value == unity3DAssetsFolderPath)
                {
                    return;
                }
                unity3DAssetsFolderPath = value;
            }
        }

        public static string Unity3DAssetDatabaseFolderPath => $"{EngData}\\UnityAssetStore";

        public static string SketchupSearchUrl = @"https://3dwarehouse.sketchup.com/search.html?backendClass=entity&q={0}";
    }
}
