using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace MBBS_Teacher
{
    class PDFConverter
    {
        string text = 0;
        text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetSubsectionNames?languageID={languageID}");
    }
}
