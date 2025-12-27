using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

public class PdfService
{
    public byte[] GenererCarteBibliotheque(User membre, DateTime dateInscription)
    {
        if (membre == null) throw new ArgumentNullException(nameof(membre));

        using (var ms = new MemoryStream())
        {
            // Créer un document PDF
            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Titre
            Paragraph titre = new Paragraph("Carte de Bibliothèque")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);
            document.Add(titre);

            // Nom de la bibliothèque
            Paragraph bibliotheque = new Paragraph("Bibliophilia")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16);
            document.Add(bibliotheque);

            document.Add(new Paragraph("\n")); // espace

            // Informations du membre
            Table table = new Table(2, true);
            table.AddCell(new Cell().Add(new Paragraph("Nom :")));
            table.AddCell(new Cell().Add(new Paragraph(membre.Nom)));
            table.AddCell(new Cell().Add(new Paragraph("Prénom :")));
            table.AddCell(new Cell().Add(new Paragraph(membre.Prenom)));
            table.AddCell(new Cell().Add(new Paragraph("Date de naissance :")));
            table.AddCell(new Cell().Add(new Paragraph(membre.DateDeNaissance?.ToString("dd/MM/yyyy") ?? "-")));
            table.AddCell(new Cell().Add(new Paragraph("Date d'inscription :")));
            table.AddCell(new Cell().Add(new Paragraph(membre.Dateinscription.ToString("dd/MM/yyyy"))));
            document.Add(table);

            document.Add(new Paragraph("\nMerci de respecter les règles de la bibliothèque !"));

            document.Close();

            return ms.ToArray(); // retourne le PDF sous forme de byte[]
        }
    }
}
