using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Educafin.Clases
{
    public class Metodos
    {
        
        public static string generatePassword(int lenght)
        {
            Random obj = new Random();
            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = posibles.Length;
            char letra;
            int longitudnuevacadena = lenght;
            string nuevacadena = "";
            for (int i = 0; i < longitudnuevacadena; i++)
            {
                letra = posibles[obj.Next(longitud)];
                nuevacadena += letra.ToString();
            }
            return nuevacadena;
        }

        public static Boolean sendMail(string content, string destination, string subject)
        {
            try
            {
                SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                server.Credentials = new System.Net.NetworkCredential("tabletasubes@itleon.edu.mx", "sube!tl31");
                server.EnableSsl = true;
                MailMessage mnsj = new MailMessage();
                mnsj.Subject = subject;
                mnsj.To.Add(new MailAddress(destination));
                mnsj.From = new MailAddress("tabletasubes@itleon.edu.mx", "Programa de inclusión digital");
                mnsj.Body = content;
                mnsj.IsBodyHtml = true;
                
                server.Send(mnsj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}