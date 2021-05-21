using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api
{
    public static partial class HelperExtensions
    {
        private static string GetValue(this IronOcr.OcrResult.Line[] lines, string[] keys)
        {
            IronOcr.OcrResult.Line line = null, next = null;
            string key = string.Empty, result = null;
            int index = 0, start = 0;

            Parallel.ForEach(lines, eachLine => {
                Parallel.ForEach(keys, eachKey =>
                {
                    if (eachLine.Text.ToLowerInvariant().Contains(eachKey))
                    {
                        key = eachKey;
                        line = eachLine;
                    }
                });
            });

            if (line != null)
            {
                start = line.Text.ToLowerInvariant().IndexOf(key) + key.Length;
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

            };
            return lines.GetValue(keys);
        }


        public static string GetDONumber(this IronOcr.OcrResult.Line[] lines)
        {
            var keys = new string[]{

            };
            return lines.GetValue(keys);
        }
    }
}
