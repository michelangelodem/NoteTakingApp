using NoteTakingApp.Services.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace NoteTakingApp.Services.Implementations.TextConverters
{
    public class MarkdownToHTML : ITextConverter
    {

        public string ConvertText(string input)
        {
            return ConvertMarkdownToHtml(input);
        }

        private string ConvertMarkdownToHtml(string markdown)
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

            html = ConvertLists(html);

            return html;
        }

        private string ConvertHeaders(string input)
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

        private string ConvertBoldAndItalic(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output = System.Text.RegularExpressions.Regex.Replace(input, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            output = System.Text.RegularExpressions.Regex.Replace(output, @"\*(.+?)\*", "<em>$1</em>");

            return output;
        }

        private string ConvertCode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output = System.Text.RegularExpressions.Regex.Replace(input, @"`(.+?)`", "<code>$1</code>");
            return output;
        }

        private string ConvertLinks(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var output =
                System.Text.RegularExpressions.Regex.Replace(input, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");
            return output;
        }

        private string ConvertLists(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var lines = input.Split('\n');
            var sb = new StringBuilder();

            bool inUl = false;
            bool inOl = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                // detect unordered list item: -, +, or *
                var ulMatch = Regex.Match(line, @"^\s*[-\+\*]\s+(.*)");
                // detect ordered list: 1. 2. etc.
                var olMatch = Regex.Match(line, @"^\s*\d+\.\s+(.*)");

                if (ulMatch.Success)
                {
                    // start ul if needed
                    if (!inUl)
                    {
                        // close ol if open
                        if (inOl) { sb.AppendLine("</ol>"); inOl = false; }
                        sb.AppendLine("<ul>");
                        inUl = true;
                    }
                    sb.AppendLine("<li>" + ulMatch.Groups[1].Value + "</li>");
                }
                else if (olMatch.Success)
                {
                    if (!inOl)
                    {
                        if (inUl) { sb.AppendLine("</ul>"); inUl = false; }
                        sb.AppendLine("<ol>");
                        inOl = true;
                    }
                    sb.AppendLine("<li>" + olMatch.Groups[1].Value + "</li>");
                }
                else
                {
                    // close any open list
                    if (inUl) { sb.AppendLine("</ul>"); inUl = false; }
                    if (inOl) { sb.AppendLine("</ol>"); inOl = false; }

                    // preserve normal line (no extra trimming so headings still match)
                    sb.AppendLine(line);
                }
            }

            // close lists at EOF
            if (inUl) sb.AppendLine("</ul>");
            if (inOl) sb.AppendLine("</ol>");

            return sb.ToString();
        }
    }
}