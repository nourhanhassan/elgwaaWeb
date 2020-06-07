using System;
using System.IO;
using System.Web;

namespace MobileApplication.UI.InfraStructure
{
    public static class Utilities
    {
        public static bool SaveFile(HttpPostedFileBase file, HttpServerUtilityBase server, string folderPath, out string path)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(fileName);
                    var name = Guid.NewGuid();
                    var serverPath = Path.Combine(server.MapPath(folderPath), name + extension);
                    file.SaveAs(serverPath);
                    path = name + extension;
                    return true;
                }
                path = string.Empty;
                return false;
            }
            catch (Exception)
            {
                path = string.Empty;
                return false;
            }
        }

        public static bool DeleteFile(HttpServerUtilityBase server, string folderPath, string filePath)
        {
            var fi = new FileInfo(server.MapPath(folderPath + filePath));
            if (fi.Exists) { fi.Delete(); return true; }
            return false;
        }

        public static bool SaveFiles(HttpPostedFile file, HttpServerUtility server, string folderPath, out string path)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(fileName);
                    var name = Guid.NewGuid();
                    var serverPath = Path.Combine(server.MapPath(folderPath), name + extension);
                    file.SaveAs(serverPath);
                    path = name + extension;
                    return true;
                }
                path = string.Empty;
                return false;
            }
            catch (Exception)
            {
                path = string.Empty;
                return false;
            }
        }

        public static bool DeleteFiles(HttpServerUtility server, string folderPath, string filePath)
        {
            var fi = new FileInfo(server.MapPath(folderPath + filePath));
            if (fi.Exists) { fi.Delete(); return true; }
            return false;
        }
    }
}