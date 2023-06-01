using App.BookingOnline.Service.IService.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace App.BookingOnline.Service.Service.Common
{
    public class FileService : IFileService
    {
        IWebHostEnvironment _webHostingEnvironment;

        public FileService(IWebHostEnvironment webHostingEnvironment)
        {
            _webHostingEnvironment = webHostingEnvironment;
        }

        public string GetRootPath()
        {
            return _webHostingEnvironment.ContentRootPath;
        }

        /// <summary>
        /// Save file to disk, filename = new Guid
        /// </summary>
        /// <param name="rootPath">Root path</param>
        /// <param name="fileName">Original file name</param>
        /// <param name="folderPath">Config path</param>
        /// <param name="content">File content</param>
        /// <param name="dateTrans">Date sub folder</param>
        /// <returns>File path</returns>
        public string SaveFile(string fileName, string folderPath, byte[] content, DateTime? dateTrans)
        {
            // Path by year/month/date if date trans is not null
            var saveFolderPath = dateTrans.HasValue ? folderPath + "\\" + DateTime.Now.ToString("yyyy-MM").Replace("-", "\\") : folderPath;

            // xoa ky tu dau '\\' de combine path
            var tFolderPath = saveFolderPath.Remove(0, 1);

            var path = Path.Combine(_webHostingEnvironment.ContentRootPath, tFolderPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Guid.NewGuid() + Path.GetExtension(fileName);
            }            

            var filePath = path + "\\" + fileName;
            //var isFileExist = File.Exists(filePath);
            
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(content);
            };

            return saveFolderPath + "\\" + fileName;
        }
    }
}
