using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api
{
    public static partial class HelperExtensions
    {
        private static IronOcr.OcrResult.Line GetLine(this IronOcr.OcrResult.Line[] lines, string[] keys, out string key)
        {
            IronOcr.OcrResult.Line line = null;
            string outKey = null;
            Parallel.ForEach(lines, eachLine => {
                Parallel.ForEach(keys, eachKey =>
                {
                    if (eachLine.Text.ToLowerInvariant().Contains(eachKey))
                    {
                        outKey = eachKey;
                        line = eachLine;
                    }
                });
            });
            key = outKey;
            return line;
        }
        
        private static string GetValue(this IronOcr.OcrResult.Line[] lines, string[] keys, int type = 0)
        {
            IronOcr.OcrResult.Line next = null;
            string result = null;
            int index = 0, start = 0;

            var line = lines.GetLine(keys, out string key);

            if (line != null)
            {
                start = line.Text.ToLowerInvariant().IndexOf(key) + key.Length;
                if (type == 1)
                    start = line.Text.LastIndexOfAny(new char[] { ' ' });
                result = line.Text.Substring(start).Trim();
                
                if (string.IsNullOrEmpty(result))
                {
                    index = Array.IndexOf(lines, line);
                    next = lines[index + 1];
                    start = next.Text.LastIndexOfAny(new char[] { ' ' });
                    start = start > 0 ? start : 0;
                    return next.Text.Substring(start);
                }
                
                return result;
            }

            return null;
        }

        public static string GetBLNumber(this IronOcr.OcrResult.Line[] lines)
        {
            var keys = new string[] {
                //"no:",
                "bl no.",
                "b/l no.",
                "b/l no.:",
                "b.l. no.",
                "bill of lading no.",
                "bill of lading no.:"
            };
            return lines.GetValue(keys);
        }

        public static string GetAmount(this IronOcr.OcrResult.Line[] lines)
        {
            var keys = new string[]{
                "total"
            };
            return lines.GetValue(keys, 1);
        }

        public static string GetDONumber(this IronOcr.OcrResult.Line[] lines)
        {
            var keys = new string[]{
                "invoice number"
            };

            var line = lines.GetLine(keys, out string key);

            if (line != null)
            {
                var keyWord = line.Words
                    .FirstOrDefault(fod => fod.Text.ToLowerInvariant() == "invoice");
                var xRange = Enumerable.Range(keyWord.X - 4, 4).ToList();
                xRange.AddRange(Enumerable.Range(keyWord.X, 5));
                var valWord = line.Block.Words
                    .FirstOrDefault(fod => xRange.Contains(fod.X) && fod.Y > keyWord.Y);
                return valWord.Text;
            }

            return null;
        }
    }
}
