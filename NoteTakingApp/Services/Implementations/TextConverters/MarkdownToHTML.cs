using NoteTakingApp.Services.Interfaces;

namespace NoteTakingApp.Services.Implementations.TextConverters
{
    public class MarkdownToHTML : ITextConverter
    {

        public string ConvertText(string input)
        {
            return ConvertMarkdownToHtml(input);
        }

        private static string ConvertMarkdownToHtml(string markdown)
        {
            if (string.IsNullOrEmpty(markdown)) return string.Empty;

            // Basic Markdown to HTML conversion
            var html = markdown;

            html = ConvertHeaders(html);

            html = ConvertBoldAndItalic(html);

            html = ConvertCode(html);

            html = ConvertLinks(html);

            // Paragraphs
            html = System.Text.RegularExpressions.Regex.Replace(html, @"\n\n+", "</p><p>");

            // Highlight 
            html = System.Text.RegularExpressions.Regex.Replace(html, @"==(.+?)==", "<mark>$1</mark>");

            // Line breaks
            html = html.Replace("\n", "<br/>");

            html = ConvertList(html, true); // Unordered lists

            html = ConvertList(html, false); // Ordered lists

            return html;
        }

        private static string ConvertHeaders(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output = System.Text.RegularExpressions.Regex.Replace(input, @"^### (.+)$", "<h3>$1</h3>",
                System.Text.RegularExpressions.RegexOptions.Multiline);
            output = System.Text.RegularExpressions.Regex.Replace(output, @"^## (.+)$", "<h2>$1</h2>",
                System.Text.RegularExpressions.RegexOptions.Multiline);
            output = System.Text.RegularExpressions.Regex.Replace(output, @"^# (.+)$", "<h1>$1</h1>",
                System.Text.RegularExpressions.RegexOptions.Multiline);

            return output;
        }

        private static string ConvertBoldAndItalic(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output = System.Text.RegularExpressions.Regex.Replace(input, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            output = System.Text.RegularExpressions.Regex.Replace(output, @"\*(.+?)\*", "<em>$1</em>");

            return output;
        }

        private static string ConvertCode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output = System.Text.RegularExpressions.Regex.Replace(input, @"`(.+?)`", "<code>$1</code>");
            return output;
        }

        private static string ConvertLinks(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output =
                System.Text.RegularExpressions.Regex.Replace(input, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");
            return output;
        }

        private static string ConvertList(string input, bool isUL = true)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var lines = input.Split('\n');
            var insideList = false;
            var output = "";

            foreach (var line in lines)
            {
                if (line.StartsWith("-"))
                {
                    if (!insideList)
                    {
                        if (isUL) output += "<ul>";
                        else output += "<ol>";
                        
                        insideList = true;
                    }
                    output += "<li>" + line.Substring(2) + "</li>";
                }
                else 
                {
                    if (insideList)
                    {
                        if (isUL) output += "</ul>";
                        else output += "</ol>";

                        insideList = false;
                    }
                }
            }

            return output;
        }
    }
}
