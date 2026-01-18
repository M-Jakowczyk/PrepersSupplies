using Plugin.Maui.OCR;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PrepersSupplies.Services
{
    public class OcrDateService
    {
        private readonly IOcrService _ocrService;

        public OcrDateService()
        {
            _ocrService = OcrPlugin.Default;
        }

        public async Task<(bool success, DateTime? date, string rawText)> RecognizeDateFromImageAsync(string imagePath)
        {
            try
            {
                Console.WriteLine($"?? Rozpoczynam rozpoznawanie OCR z pliku: {imagePath}");

                // Wczytaj obraz jako byte[]
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

                // Rozpoznaj tekst z obrazu
                var ocrResult = await _ocrService.RecognizeTextAsync(imageBytes);

                if (ocrResult?.AllText == null || string.IsNullOrWhiteSpace(ocrResult.AllText))
                {
                    Console.WriteLine("? Nie rozpoznano ¿adnego tekstu");
                    return (false, null, string.Empty);
                }

                var recognizedText = ocrResult.AllText;
                Console.WriteLine($"? Rozpoznany tekst: {recognizedText}");

                // Spróbuj wyodrêbniæ datê z tekstu
                var date = ExtractDate(recognizedText);

                if (date.HasValue)
                {
                    Console.WriteLine($"? Znaleziono datê: {date.Value:yyyy-MM-dd}");
                    return (true, date.Value, recognizedText);
                }
                else
                {
                    Console.WriteLine("?? Nie znaleziono daty w rozpoznanym tekœcie");
                    return (false, null, recognizedText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? B³¹d podczas rozpoznawania OCR: {ex.Message}");
                return (false, null, string.Empty);
            }
        }

        private DateTime? ExtractDate(string text)
        {
            // Wzorce dat do rozpoznania
            var datePatterns = new[]
            {
                // Format: dd.MM.yyyy lub dd-MM-yyyy lub dd/MM/yyyy
                @"(\d{1,2})[.\-/](\d{1,2})[.\-/](\d{4})",
                // Format: yyyy.MM.dd lub yyyy-MM-dd lub yyyy/MM/dd
                @"(\d{4})[.\-/](\d{1,2})[.\-/](\d{1,2})",
                // Format: dd.MM.yy lub dd-MM-yy lub dd/MM/yy
                @"(\d{1,2})[.\-/](\d{1,2})[.\-/](\d{2})",
                // Format: ddMMyyyy (bez separatorów)
                @"(\d{2})(\d{2})(\d{4})",
                // Format: yyyyMMdd (bez separatorów)
                @"(\d{4})(\d{2})(\d{2})"
            };

            foreach (var pattern in datePatterns)
            {
                var matches = Regex.Matches(text, pattern);
                foreach (Match match in matches)
                {
                    var date = TryParseDate(match, pattern);
                    if (date.HasValue)
                    {
                        return date;
                    }
                }
            }

            // Spróbuj znaleŸæ tekst typu "Best Before" lub "EXP" lub "Use By"
            var expiryKeywords = new[] { "exp", "best before", "use by", "bb", "wa¿ne do", "data wa¿noœci", "przydatne do" };
            foreach (var keyword in expiryKeywords)
            {
                var index = text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    var textAfterKeyword = text.Substring(index + keyword.Length);
                    foreach (var pattern in datePatterns)
                    {
                        var matches = Regex.Matches(textAfterKeyword, pattern);
                        foreach (Match match in matches)
                        {
                            var date = TryParseDate(match, pattern);
                            if (date.HasValue && date.Value.Year >= DateTime.Now.Year && date.Value.Year <= DateTime.Now.Year + 10)
                            {
                                return date;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private DateTime? TryParseDate(Match match, string pattern)
        {
            try
            {
                if (pattern.Contains(@"(\d{4})[.\-/](\d{1,2})[.\-/](\d{1,2})")) // yyyy-MM-dd
                {
                    var year = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var day = int.Parse(match.Groups[3].Value);
                    return new DateTime(year, month, day);
                }
                else if (pattern.Contains(@"(\d{1,2})[.\-/](\d{1,2})[.\-/](\d{4})")) // dd-MM-yyyy
                {
                    var day = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var year = int.Parse(match.Groups[3].Value);
                    return new DateTime(year, month, day);
                }
                else if (pattern.Contains(@"(\d{1,2})[.\-/](\d{1,2})[.\-/](\d{2})")) // dd-MM-yy
                {
                    var day = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var year = int.Parse(match.Groups[3].Value);
                    year += (year > 50 ? 1900 : 2000); // Konwersja roku 2-cyfrowego
                    return new DateTime(year, month, day);
                }
                else if (pattern.Contains(@"(\d{2})(\d{2})(\d{4})")) // ddMMyyyy
                {
                    var day = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var year = int.Parse(match.Groups[3].Value);
                    return new DateTime(year, month, day);
                }
                else if (pattern.Contains(@"(\d{4})(\d{2})(\d{2})")) // yyyyMMdd
                {
                    var year = int.Parse(match.Groups[1].Value);
                    var month = int.Parse(match.Groups[2].Value);
                    var day = int.Parse(match.Groups[3].Value);
                    return new DateTime(year, month, day);
                }
            }
            catch
            {
                // Ignoruj niepoprawne daty
            }

            return null;
        }
    }
}
