using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2
{
    public class Constants
    {
        public static readonly List<string> AllowedFileExtensions =
               new List<string> { ".jpg", ".jpeg", ".png", ".pdf" };

        public static readonly string UploadFolder = "~/Upload/";

        public static readonly string MappedUploadFolder = HttpContext.Current.Server.MapPath(UploadFolder);
    }
}