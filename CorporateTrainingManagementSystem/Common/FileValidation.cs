namespace CorporateTrainingManagementSystem.Common
{
    public static class FileValidation
    {
        public const long MaxPdfSize = 10 * 1024 * 1024; //10 MB

        public static readonly string[] PdfExtensions =
        {
            ".pdf"
        };

        public static readonly string[] ImageExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        public const long MaxImageSize = 2 * 1024 * 1024;
    }
}
