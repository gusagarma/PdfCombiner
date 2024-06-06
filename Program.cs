using System;
using System.IO;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

class Program
{
    static void Main(string[] args)
    {
        string[] pdfFiles = Directory.GetFiles("documents", "*.pdf");
        string outputPdfPath = "combined_output.pdf";
        CombinePdfsWithPlaceholders(pdfFiles, outputPdfPath);
    }

    static void CombinePdfsWithPlaceholders(string[] pdfFiles, string outputPdfPath)
    {
        PdfDocument outputDocument = new PdfDocument();

        foreach (var pdfFile in pdfFiles)
        {
            if (!File.Exists(pdfFile))
            {
                Console.WriteLine($"Arquivo não encontrado: {pdfFile}");
                continue;
            }

            PdfDocument inputDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);

            // Adicionar páginas do documento original
            foreach (var page in inputDocument.Pages)
            {
                outputDocument.AddPage(page);
            }

            // Adicionar página em branco com demarcação
            PdfPage placeholderPage = outputDocument.AddPage();
            using (XGraphics gfx = XGraphics.FromPdfPage(placeholderPage))
            {
                // Configurar posição do texto no canto inferior direito
                double margin = 10; // margem da borda da página
                double x = placeholderPage.Width.Point - margin;
                double y = placeholderPage.Height.Point - margin;

                gfx.DrawString("End of Document", new XFont("Arial", 12), XBrushes.Gray,
                               new XRect(x, y, 0, 0),
                               XStringFormats.BottomRight);
            }
        }

        // Salvar documento combinado
        outputDocument.Save(outputPdfPath);
        Console.WriteLine($"PDF combinado salvo em: {outputPdfPath}");
    }
}
