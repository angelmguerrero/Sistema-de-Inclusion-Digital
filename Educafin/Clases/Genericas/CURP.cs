using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Educafin.Clases
{
    public class CURP
    {
        public string get_Curp(string curp)
        {
            
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create("http://sube.educafin.com/includes/uys87eryyu_datos_curp_renapo_sibec_talentum_xyzdkhgfuers.php?txtCurpIngresado=" + curp);
            wrGETURL.ContentType = "application/json";
            wrGETURL.Credentials = new NetworkCredential("x", "reallylongstring");
            Stream objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string responseFromServer = objReader.ReadToEnd();

            return responseFromServer;
        }
    }
}