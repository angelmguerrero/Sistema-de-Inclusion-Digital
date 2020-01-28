using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Printer
/// </summary>
public class Printer
{
    String num_rec, nombre_al, correo_al, fecha_rec,
                    domicilio, colonia, municipio, nom_escu,
                    plantel, grupo;

    public Printer(String nombre_al, String correo_al,
                   String domicilio, String colonia, String municipio, String nom_escu,
                   String plantel, String grupo)
    {
        this.nombre_al = nombre_al;
        this.correo_al = correo_al;
        fecha_rec = DateTime.Now.ToString();
        this.domicilio = domicilio;
        this.colonia = colonia;
        this.municipio = municipio;
        this.nom_escu = nom_escu;
        this.plantel = plantel;
        this.grupo = grupo;
    }

    public Printer(String nombre_al, String correo_al,
                   String domicilio, String colonia, String municipio)
    {
        this.nombre_al = nombre_al;
        this.correo_al = correo_al;
        fecha_rec = DateTime.Now.ToString();
        this.domicilio = domicilio;
        this.colonia = colonia;
        this.municipio = municipio;
    }

    public void FillPDFBenMay(string templateFile, Stream stream)
    {
        // Abrimos la plantilla y creamos una copia, sobre
        // la cual trabajaremos...
        PdfReader reader = new PdfReader(templateFile);
        PdfStamper stamp = new PdfStamper(reader, stream);

        // Introducimos el valor en los campos del formulario...
        stamp.AcroFields.SetField("num_rec", "RC2133");
        stamp.AcroFields.SetField("nombre_al", nombre_al);
        stamp.AcroFields.SetField("correo_al", correo_al);
        stamp.AcroFields.SetField("fecha_rec", fecha_rec);
        stamp.AcroFields.SetField("domicilio", domicilio);
        stamp.AcroFields.SetField("colonia", colonia);
        stamp.AcroFields.SetField("municipio", municipio);
        stamp.AcroFields.SetField("nom_escu", "Tec");
        stamp.AcroFields.SetField("plantel", "Leon");
        stamp.AcroFields.SetField("grupo", "A");

        // Fijamos los valores y enviamos el resultado al stream...
        stamp.FormFlattening = true;
        stamp.Close();
    }

    public void FillPDFBenMen(string templateFile, Stream stream)
    {
        // Abrimos la plantilla y creamos una copia, sobre
        // la cual trabajaremos...
        PdfReader reader = new PdfReader(templateFile);
        PdfStamper stamp = new PdfStamper(reader, stream);

        // Introducimos el valor en los campos del formulario...
        stamp.AcroFields.SetField("num_rec", "RC2133");
        stamp.AcroFields.SetField("nombre_al", nombre_al);
        stamp.AcroFields.SetField("correo_al", correo_al);
        stamp.AcroFields.SetField("fecha_rec", fecha_rec);
        stamp.AcroFields.SetField("domicilio", domicilio);
        stamp.AcroFields.SetField("colonia", colonia);
        stamp.AcroFields.SetField("municipio", municipio);
        stamp.AcroFields.SetField("nom_escu", "Tec");
        stamp.AcroFields.SetField("plantel", "Leon");
        stamp.AcroFields.SetField("grupo", "A");

        // Fijamos los valores y enviamos el resultado al stream...
        stamp.FormFlattening = true;
        stamp.Close();
    }

    public void FillPDFDoc(string templateFile, Stream stream)
    {
        // Abrimos la plantilla y creamos una copia, sobre
        // la cual trabajaremos...
        PdfReader reader = new PdfReader(templateFile);
        PdfStamper stamp = new PdfStamper(reader, stream);

        // Introducimos el valor en los campos del formulario...
        stamp.AcroFields.SetField("num_rec", "RC2133");
        stamp.AcroFields.SetField("nombre_al", nombre_al);
        stamp.AcroFields.SetField("correo_al", correo_al);
        stamp.AcroFields.SetField("fecha_rec", fecha_rec);
        stamp.AcroFields.SetField("domicilio", domicilio);
        stamp.AcroFields.SetField("colonia", colonia);
        stamp.AcroFields.SetField("municipio", municipio);
        stamp.AcroFields.SetField("nom_escu", "Tec");
        stamp.AcroFields.SetField("plantel", "Leon");
        stamp.AcroFields.SetField("grupo", "A");

        // Fijamos los valores y enviamos el resultado al stream...
        stamp.FormFlattening = true;
        stamp.Close();
    }
}
