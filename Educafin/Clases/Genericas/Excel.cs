using Educafin.Models;
using MvcApplication8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;

namespace Institucion.Models
{
    public class Excel
    {


        public void tabletasNoEntregadas(string Path, List<MvcApplication8.Models.Tableta> lis)
        {



            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "No.";
            xlWorkSheet.Cells[1, 2] = "No. de Serie";
            xlWorkSheet.Cells[1, 3] = "CCT Institución.";
            xlWorkSheet.Cells[1, 4] = "Nombre Instución.";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.No;
                xlWorkSheet.Cells[i, 2] = item.serie;
                xlWorkSheet.Cells[i, 3] = item.institucion;
                xlWorkSheet.Cells[i, 4] = item.nombre_institucion;
                i++;
            }


            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }




        public void cargaarexcel(string Path, List<Educafin.Models.Alumno> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "Curp";
            xlWorkSheet.Cells[1, 2] = "Matricula";
            xlWorkSheet.Cells[1, 3] = "Nombre";
            xlWorkSheet.Cells[1, 4] = "Error";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.curp;
                xlWorkSheet.Cells[i, 2] = item.matricula;
                xlWorkSheet.Cells[i, 3] = item.nombre;
                xlWorkSheet.Cells[i, 4] = item.status;
                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaBeneficiario(string Path, List<Educafin.Models.Alumno> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "Curp";
            xlWorkSheet.Cells[1, 2] = "Matricula";
            xlWorkSheet.Cells[1, 3] = "Apellido Paterno";
            xlWorkSheet.Cells[1, 4] = "Apellido Materno";
            xlWorkSheet.Cells[1, 5] = "Nombre(s)";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.curp;
                xlWorkSheet.Cells[i, 2] = item.matricula;
                xlWorkSheet.Cells[i, 3] = item.apePat;
                xlWorkSheet.Cells[i, 4] = item.apeMat;
                xlWorkSheet.Cells[i, 5] = item.nombreTemp;
                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaBeneficiariosinsesion(string Path, List<BeneficiarioSinsesion > lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "Curp";
            xlWorkSheet.Cells[1, 2] = "Correo";
            xlWorkSheet.Cells[1, 3] = "Password";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.curp;
                xlWorkSheet.Cells[i, 2] = item.correo;
                xlWorkSheet.Cells[i, 3] = item.password;
                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaBeneficiarioTablet(string Path, List<Educafin.Models.Alumno> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "Curp";
            xlWorkSheet.Cells[1, 2] = "Matricula";
            xlWorkSheet.Cells[1, 3] = "Apellido Paterno";
            xlWorkSheet.Cells[1, 4] = "Apellido Materno";
            xlWorkSheet.Cells[1, 5] = "Nombre(s)";
            xlWorkSheet.Cells[1, 6] = "Tablet";
            xlWorkSheet.Cells[1, 7] = "Fecha de Entrega";
            xlWorkSheet.Cells[1, 8] = "Correo Electronico";
            xlWorkSheet.Cells[1, 9] = "Folio";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.curp;
                xlWorkSheet.Cells[i, 2] = item.matricula;
                xlWorkSheet.Cells[i, 3] = item.apePat;
                xlWorkSheet.Cells[i, 4] = item.apeMat;
                xlWorkSheet.Cells[i, 5] = item.nombreTemp;
                xlWorkSheet.Cells[i, 6] = item.status;
                xlWorkSheet.Cells[i, 7] = item.fecha;
                xlWorkSheet.Cells[i, 8] = item.correo;
                xlWorkSheet.Cells[i, 9] = item.folio;
                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaFormato(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "CURP";
            xlWorkSheet.Cells[1, 2] = "Matricula";


            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }


        public void cargaFormatoPlanteles(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "CURP";
            xlWorkSheet.Cells[1, 2] = "Matricula";
            xlWorkSheet.Cells[1, 3] = "CCT al que sera asignado";

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaFormatoTableta(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "CCT";
            xlWorkSheet.Cells[1, 2] = "N° de serie";

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaarexcelTableta(string Path, List<Tableta> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "N°";
            xlWorkSheet.Cells[1, 2] = "Nombre";
            xlWorkSheet.Cells[1, 3] = "N° serie";
            xlWorkSheet.Cells[1, 4] = "Estatus";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.No;
                xlWorkSheet.Cells[i, 2] = item.nombre;
                xlWorkSheet.Cells[i, 3] = item.serie;
                xlWorkSheet.Cells[i, 4] = item.estado;
                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaarexcelTabletaLista(string Path, List<Tableta> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 2] = "N° serie";
            xlWorkSheet.Cells[1, 1] = "CCT°";


            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 2] = item.serie;

                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }


        public void cargaProveedorMasivo(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            else {
                fi.Create();
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "RFC";
            xlWorkSheet.Cells[1, 2] = "Nombre";
            xlWorkSheet.Cells[1, 3] = "Representante";
            xlWorkSheet.Cells[1, 4] = "C.P.";
            xlWorkSheet.Cells[1, 5] = "Estado";
            xlWorkSheet.Cells[1, 6] = "Municipio";
            xlWorkSheet.Cells[1, 7] = "Colonia";
            xlWorkSheet.Cells[1, 8] = "Calle";
            xlWorkSheet.Cells[1, 9] = "Numero Exterior";
            xlWorkSheet.Cells[1, 10] = "Numero Interior";
            xlWorkSheet.Cells[1, 11] = "Telefono";
            xlWorkSheet.Cells[1, 12] = "Correo";

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaProveedorMasivoErrores(string Path, List<InstitucionMasivoModel> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            else
            {
                fi.Create();
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "RFC";
            xlWorkSheet.Cells[1, 2] = "Error";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.CCT;
                xlWorkSheet.Cells[i, 2] = item.nombre;
                i++;
            }

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }



        public void cargaInstitucionMasivo(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            else
            {
                fi.Create();
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "CCT";
            xlWorkSheet.Cells[1, 2] = "Nombre";
            xlWorkSheet.Cells[1, 3] = "Grado [S para Superior, M para Medio Superior]";
            xlWorkSheet.Cells[1, 4] = "Estado";
            xlWorkSheet.Cells[1, 5] = "Municipio";
            xlWorkSheet.Cells[1, 6] = "Colonia";
            xlWorkSheet.Cells[1, 7] = "Calle";
            xlWorkSheet.Cells[1, 8] = "Numero Exterior";
            xlWorkSheet.Cells[1, 9] = "C.P.";
            xlWorkSheet.Cells[1, 10] = "Telefono";
            xlWorkSheet.Cells[1, 11] = "Correo";
            xlWorkSheet.Cells[1, 12] = "Oficinas (Si o No)";
            xlWorkSheet.Cells[1, 13] = "Dependencia";

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaInstitucionMasivoErrores(string Path, List<InstitucionMasivoModel> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            else
            {
                fi.Create();
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "CCT";
            xlWorkSheet.Cells[1, 2] = "Nombre";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.CCT;
                xlWorkSheet.Cells[i, 2] = item.nombre;
                i++;
            }

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }





        public void cargaUsuariosMasivo(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            else
            {
                fi.Create();
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "CURP";
            xlWorkSheet.Cells[1, 2] = "RFC ó CCT donde pertenece";
            xlWorkSheet.Cells[1, 3] = "C.P.";
            xlWorkSheet.Cells[1, 4] = "Estado Recidencia";
            xlWorkSheet.Cells[1, 5] = "Municipio";
            xlWorkSheet.Cells[1, 6] = "Colonia";
            xlWorkSheet.Cells[1, 7] = "Calle";
            xlWorkSheet.Cells[1, 8] = "Numero Interior";
            xlWorkSheet.Cells[1, 9] = "Numero Exterior";
            xlWorkSheet.Cells[1, 10] = "Telefono";
            xlWorkSheet.Cells[1, 11] = "Telefono Celular";
            xlWorkSheet.Cells[1, 12] = "Correo";


            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaUsuariosMasivoErrores(string Path, List<InstitucionMasivoModel> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            else
            {
                fi.Create();
            }

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "CCT";
            xlWorkSheet.Cells[1, 2] = "Nombre";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.CCT;
                xlWorkSheet.Cells[i, 2] = item.nombre;
                i++;
            }

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }


        public void cargaFormatoTabletaMP(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "No. Serie";

            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        public void cargaarexcelTabletaMPError(string Path, List<Tableta> lis)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 2;

            xlWorkSheet.Cells[1, 1] = "N° Serie";
            xlWorkSheet.Cells[1, 2] = "Nombre";
            xlWorkSheet.Cells[1, 3] = "Estatus";

            foreach (var item in lis)
            {
                xlWorkSheet.Cells[i, 1] = item.serie;
                xlWorkSheet.Cells[i, 2] = item.nombre;
                xlWorkSheet.Cells[i, 3] = item.estado;
                i++;
            }
            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }


        public void cargaFormatoEntregasAnteriores(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "CURP";
            xlWorkSheet.Cells[1, 2] = "CCT";
            xlWorkSheet.Cells[1, 3] = "Matricula";
            xlWorkSheet.Cells[1, 4] = "No. Serie";


            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }


        public void cargaFormatoBenefDatos(string Path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
            {
                fi.Delete();
            }
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "CURP";
            xlWorkSheet.Cells[1, 2] = "Matricula";
            xlWorkSheet.Cells[1, 3] = "CCT al que pertenecera";
            xlWorkSheet.Cells[1, 4] = "Calle";
            xlWorkSheet.Cells[1, 5] = "Num. Int.";
            xlWorkSheet.Cells[1, 6] = "Num. Ext.";
            xlWorkSheet.Cells[1, 7] = "Colonia";
            xlWorkSheet.Cells[1, 8] = "Estado";
            xlWorkSheet.Cells[1, 9] = "Municipio";
            xlWorkSheet.Cells[1, 10] = "C.P.";
            xlWorkSheet.Cells[1, 11] = "Tel. Casa";
            xlWorkSheet.Cells[1, 12] = "Tel. Cel.";
            xlWorkSheet.Cells[1, 13] = "Tutor (Nombre o dejar en blanco)";
            xlWorkSheet.Cells[1, 14] = "Credencial (Si es mayor de Edad indicar con: 'SI' ";
            xlWorkSheet.Cells[1, 15] = "Correo";


            xlWorkBook.SaveAs(Path);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }




    }
}