using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

class Program
{
    static void Main()
    {
        // Generar ciudadanos ficticios
        HashSet<string> ciudadanos = GenerarCiudadanos(500);
        HashSet<string> vacunadosPfizer = GenerarCiudadanos(75, "Pfizer");
        HashSet<string> vacunadosAstrazeneca = GenerarCiudadanos(75, "Astrazeneca");

        // Operaciones de conjuntos
        HashSet<string> vacunadosAmbas = new HashSet<string>(vacunadosPfizer);
        vacunadosAmbas.IntersectWith(vacunadosAstrazeneca);

        HashSet<string> noVacunados = new HashSet<string>(ciudadanos);
        noVacunados.ExceptWith(vacunadosPfizer);
        noVacunados.ExceptWith(vacunadosAstrazeneca);

        HashSet<string> soloPfizer = new HashSet<string>(vacunadosPfizer);
        soloPfizer.ExceptWith(vacunadosAstrazeneca);

        HashSet<string> soloAstrazeneca = new HashSet<string>(vacunadosAstrazeneca);
        soloAstrazeneca.ExceptWith(vacunadosPfizer);

        // Generar reporte en PDF
        GenerarPDF(noVacunados, vacunadosAmbas, soloPfizer, soloAstrazeneca);

        Console.WriteLine("Reporte generado: ReporteVacunación.pdf");
    }

    static HashSet<string> GenerarCiudadanos(int cantidad, string tipo = "")
    {
        HashSet<string> ciudadanos = new HashSet<string>();
        Random rnd = new Random();
        while (ciudadanos.Count < cantidad)
        {
            ciudadanos.Add($"Ciudadano {rnd.Next(1, 501)} {tipo}".Trim());
        }
        return ciudadanos;
    }

    static void GenerarPDF(HashSet<string> noVacunados, HashSet<string> vacunadosAmbas, HashSet<string> soloPfizer, HashSet<string> soloAstrazeneca)
    {
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream("ReporteVacunación.pdf", FileMode.Create));
        doc.Open();

        AgregarSeccion(doc, "Ciudadanos No Vacunados", noVacunados);
        AgregarSeccion(doc, "Ciudadanos con ambas vacunas", vacunadosAmbas);
        AgregarSeccion(doc, "Ciudadanos con solo Pfizer", soloPfizer);
        AgregarSeccion(doc, "Ciudadanos con solo Astrazeneca", soloAstrazeneca);

        doc.Close();
    }

    static void AgregarSeccion(Document doc, string titulo, HashSet<string> datos)
    {
        doc.Add(new Paragraph(titulo, new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
        foreach (var item in datos)
        {
            doc.Add(new Paragraph(item));
        }
        doc.Add(new Paragraph("\n"));
    }
}
