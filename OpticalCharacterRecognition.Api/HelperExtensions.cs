using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api
{
    public static partial class HelperExtensions
    {
        private static OcrResult.Line GetLine(this OcrResult.Line[] lines, string[] keys, out string key)
        {
            OcrResult.Line line = null;
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
        
        private static string GetDown(this OcrResult.Line line, OcrResult.Word keyWord)
        {
            if (line != null)
            {
                var xRange = Enumerable.Range(keyWord.X - 4, 4).ToList();
                xRange.AddRange(Enumerable.Range(keyWord.X, 5));
                var valWord = line.Block.Words
                    .FirstOrDefault(fod => xRange.Contains(fod.X) && fod.Y > keyWord.Y);
                return valWord.Text;
            }
            return null;
        }
        
        private static string GetValue(this OcrResult.Line[] lines, string[] keys, bool isAmount = false)
        {
            OcrResult.Word startWord = null, endWord = null, nextWord = null;
            OcrResult.Line line = null;
            string firstWord = null, lastWord = null, key = null;
            int start = 0;

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
                if (isAmount)
                {
                    start = line.Text.LastIndexOfAny(new char[] { ' ' });
                    return line.Text.Substring(start).Trim();
                }

                var blocks = key.Split(" ");
                if (blocks.Length > 1)
                {
                    firstWord = blocks.FirstOrDefault();
                    lastWord = blocks.LastOrDefault();
                    startWord = line.Words.FirstOrDefault(fod => fod.Text.ToLowerInvariant() == firstWord);
                    endWord = line.Words.FirstOrDefault(fod => fod.X > startWord.X
                        && fod.Text.ToLowerInvariant().Contains(lastWord));
                    nextWord = line.Words.FirstOrDefault(fod => fod.X > endWord.X);
                }
                else
                {
                    endWord = line.Words.FirstOrDefault(fod => fod.Text.ToLowerInvariant() == key);
                    nextWord = line.Words.FirstOrDefault(fod => fod.X > endWord.X);
                }

                if (nextWord == null)
                    return line.GetDown(startWord);

                var gapX = nextWord.X - (endWord.X + endWord.Width);
                if (gapX > 3)
                    return line.GetDown(startWord);

                start = line.Text.ToLowerInvariant().IndexOf(key) + key.Length;
                return line.Text.Substring(start).Trim();
            }

            return null;
        }

        public static string GetBLNumber(this OcrResult.Line[] lines)
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

        public static string GetAmount(this OcrResult.Line[] lines)
        {
            var keys = new string[]{
                "total"
            };
            return lines.GetValue(keys, true);
        }

        public static string GetDONumber(this OcrResult.Line[] lines)
        {
            var keys = new string[]{
                "invoice number"
            };
            return lines.GetValue(keys);
        }
    }
}
