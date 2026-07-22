using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace CorporateTrainingManagementSystem.Documents
{
    public class CertificateDocument : IDocument
    {
        private readonly Certificate _certificate;

        public CertificateDocument(Certificate certificate)
        {
            _certificate = certificate;
        }

        public DocumentMetadata GetMetadata()
            => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(35);

                page.PageColor(Colors.White);

                page.DefaultTextStyle(x =>
                    x.FontFamily("Times New Roman")
                     .FontSize(16));

                page.Content()
                    .Padding(8)
                    .Border(4)
                    .BorderColor(Colors.Amber.Darken2)
                    .Padding(6)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(28)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        // =========================
                        // Header
                        // =========================

                        column.Item()
                            .AlignCenter()
                            .Text("Corporate Training Management System")
                            .Bold()
                            .FontSize(30)
                            .FontColor(Colors.Blue.Darken3);

                        column.Item()
                            .PaddingTop(28)
                            .LineHorizontal(2)
                            .LineColor(Colors.Amber.Darken2);

                        column.Item()
                            .PaddingTop(28)
                            .AlignCenter()
                            .Text("CERTIFICATE OF COMPLETION")
                            .Bold()
                            .FontSize(30);

                        // =========================
                        // Body
                        // =========================

                        column.Item()
                            .PaddingTop(28)
                            .AlignCenter()
                            .Text("This certificate is proudly presented to")
                            .FontSize(18);

                        column.Item()
                            .AlignCenter()
                            .Text(_certificate.User.FullName)
                            .Bold()
                            .FontSize(30)
                            .FontColor(Colors.Green.Darken2);

                        column.Item()
                            .PaddingTop(8)
                            .AlignCenter()
                            .Text("for successfully completing")
                            .FontSize(18);

                        column.Item()
                            .AlignCenter()
                            .Text(_certificate.Course.Title)
                            .Bold()
                            .FontSize(24)
                            .FontColor(Colors.Blue.Medium);

                        column.Item()
                            .PaddingTop(28)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten2);

                        // =========================
                        // Certificate Info
                        // =========================

                        column.Item()
                            .PaddingTop(28)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(col =>
                                    {
                                        col.Item()
                                            .Text("Issue Date")
                                            .Bold()
                                            .FontSize(14);

                                        col.Item()
                                            .Text(
                                                _certificate.IssueDate
                                                    .ToString("dd MMMM yyyy"))
                                            .FontSize(16);
                                    });

                                row.RelativeItem()
                                    .AlignRight()
                                    .Column(col =>
                                    {
                                        col.Item()
                                            .AlignRight()
                                            .Text("Certificate Number")
                                            .Bold()
                                            .FontSize(14);

                                        col.Item()
                                            .AlignRight()
                                            .Text(_certificate.CertificateNumber)
                                            .FontSize(16)
                                            .Bold();

                                        col.Item()
                                            .PaddingTop(6)
                                            .AlignRight()
                                            //.Hyperlink(
                                            //    $"https://localhost:7181/Verify/{_certificate.CertificateNumber}")
                                            .Hyperlink(
                                                $"https://corporatetraining.runasp.net/Verify/{_certificate.CertificateNumber}")
                                            .Text("🔗 Verify Certificate")
                                            .FontSize(12)
                                            .FontColor(Colors.Blue.Medium)
                                            .Underline();
                                    });
                            });

                        // =========================
                        // Footer
                        // =========================

                        column.Item()
                            .PaddingTop(25)
                            .Row(row =>
                            {
                                // VERIFIED SEAL
                                row.ConstantItem(100)
                                    .Height(100)
                                    .Border(3)
                                    .BorderColor(Colors.Green.Darken2)
                                    .CornerRadius(50)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Column(col =>
                                    {
                                        col.Item()
                                            .AlignCenter()
                                            .Text("✓")
                                            .FontSize(24)
                                            .Bold()
                                            .FontColor(Colors.Green.Darken2);

                                        col.Item()
                                            .AlignCenter()
                                            .Text("VERIFIED")
                                            .Bold()
                                            .FontSize(12)
                                            .FontColor(Colors.Green.Darken2);

                                        col.Item()
                                            .AlignCenter()
                                            .Text("OFFICIAL")
                                            .FontSize(9)
                                            .FontColor(Colors.Green.Darken2);
                                    });

                                row.RelativeItem();

                                // Signature
                                row.ConstantItem(170)
                                    .AlignBottom()
                                    .Column(col =>
                                    {
                                        col.Item()
                                            .AlignCenter()
                                            .LineHorizontal(1);

                                        col.Item()
                                            .PaddingTop(5)
                                            .AlignCenter()
                                            .Text("Training Manager")
                                            .Bold()
                                            .FontSize(14);
                                    });
                            });
                    });
            });
        }
    }
}