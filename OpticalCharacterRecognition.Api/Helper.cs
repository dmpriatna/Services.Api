using IronOcr;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api
{
    public static partial class Helper
    {
        public static IConfiguration Config
        {
            get
            {
                var builder = new ConfigurationBuilder();
                var envVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var envName = string.IsNullOrWhiteSpace(envVariable) ? "" : "." + envVariable;
                return builder
                    .AddJsonFile($"appsettings{envName}.json", true, true)
                    .Build();
            }
        }

        private static string[] ToArray(this IEnumerable<IConfigurationSection> child)
        {
            var _array = new string[child.Count()];
            Parallel.ForEach(child, each =>
            {
                _array[int.Parse(each.Key)] = each.Value;
            });
            return _array;
        }

        private static int Count<T>(this IEnumerable<T> source)
        {
            var _count = 0;
            using (var e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    _count++;
                }
            }
            return _count;
        }
        
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
        
        public static string GetBLNumber(this OcrResult.Line[] lines)
        {
            System.Diagnostics.Debug.WriteLine($"IronOcr ready\t\t{System.DateTime.Now}");
            var keys = Config.GetSection("Keys:Bill")
                .GetChildren()
                .ToArray();
            var result = lines.GetValue(keys);
            System.Diagnostics.Debug.WriteLine($"get result\t\t{System.DateTime.Now}");
            return result;
        }

        public static string GetAmount(this OcrResult.Line[] lines)
        {
            var keys = Config.GetSection("Keys:Amount")
                .GetChildren()
                .ToArray();
            return lines.GetValue(keys, true);
        }

        public static string GetDONumber(this OcrResult.Line[] lines)
        {
            var keys = Config.GetSection("Keys:Invoice")
                .GetChildren()
                .ToArray();
            return lines.GetValue(keys);
        }

        private static string GetDown(this OcrResult.Line line, OcrResult.Word keyWord, int treshould = 10)
        {
            if (line != null)
            {
                var nextLine = Array.FindAll(line.Block.Words, f =>
                    f.Y > keyWord.Y + keyWord.Height);
                var valWord = Array.Find(nextLine, f =>
                    f.X > keyWord.X - treshould);
                return valWord?.Text;
            }
            return null;
        }
        
        private static string GetValue(this OcrResult.Line[] lines, string[] keys, bool isAmount = false)
        {
            OcrResult.Word startWord = null, endWord = null, nextWord = null;
            OcrResult.Line line = null;
            string firstWord = null, lastWord = null, key = null;
            int start = 0;

            line = Array.Find(lines, l =>
            {
                key = Array.Find(keys, k => l.Text.ToLowerInvariant().Contains(k));
                return key != null;
            });
            System.Diagnostics.Debug.WriteLine($"key found\t\t{System.DateTime.Now}");

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
                    firstWord = blocks[0];
                    lastWord = blocks[blocks.Length - 1];
                    startWord = Array.Find(line.Words, f => f.Text.ToLowerInvariant() == firstWord);
                    endWord = Array.Find(line.Words, f => f.X > startWord.X
                        && f.Text.ToLowerInvariant().Contains(lastWord));
                    nextWord = Array.Find(line.Words, f => f.X > endWord.X);
                }
                else
                {
                    endWord = Array.Find(line.Words, f => f.Text.ToLowerInvariant() == key);
                    nextWord = Array.Find(line.Words, f => f.X > endWord.X);
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

        public static string GetBLNumber(this OcrResult source)
        {
            System.Diagnostics.Debug.WriteLine($"IronOcr ready\t\t{System.DateTime.Now}");
            var keys = Config.GetSection("Keys:Bill")
                .GetChildren()
                .ToArray();
            var result = source.GetValue(keys);
            System.Diagnostics.Debug.WriteLine($"get result\t\t{System.DateTime.Now}");
            return result;
        }

        public static string GetAmount(this OcrResult source)
        {
            var keys = Config.GetSection("Keys:Amount")
                .GetChildren()
                .ToArray();
            return source.GetValue(keys, true);
        }

        public static string GetDONumber(this OcrResult source)
        {
            var keys = Config.GetSection("Keys:Invoice")
                .GetChildren()
                .ToArray();
            return source.GetValue(keys);
        }

        private static string GetValue(this OcrResult source, string[] keys, bool isAmount = false)
        {
            OcrResult.Word startWord = null, endWord = null, nextWord = null;
            OcrResult.Line line = null;
            string firstWord = null, lastWord = null, key = null;
            int start = 0;

            line = Array.Find(source.Lines, l =>
            {
                key = Array.Find(keys, k => l.Text.ToLowerInvariant().Contains(k));
                return key != null;
            });
            System.Diagnostics.Debug.WriteLine($"key found\t\t{System.DateTime.Now}");

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
                    firstWord = blocks[0];
                    lastWord = blocks[blocks.Length - 1];
                    startWord = Array.Find(line.Words, f => f.Text.ToLowerInvariant().Contains(firstWord));
                    endWord = Array.Find(line.Words, f => f.X > startWord.X
                        && f.Text.ToLowerInvariant().Contains(lastWord));
                    nextWord = Array.Find(line.Words, f => f.X > endWord.X);
                }
                else
                {
                    startWord = Array.Find(line.Words, f => f.Text.ToLowerInvariant().Contains(key));
                    nextWord = Array.Find(line.Words, f => f.X > startWord.X);
                }

                if (nextWord == null)
                    return source.GetDown(startWord);

                // var gapX = nextWord.X - (endWord.X + endWord.Width);
                // if (gapX > 3)
                //     return source.GetDown(startWord);

                start = line.Text.ToLowerInvariant().IndexOf(key) + key.Length;
                var obs = line.Text.IndexOf("|");
                if (obs > 0)
                    return line.Text.Substring(start, obs - start).Trim();
                return line.Text.Substring(start).Trim();
            }

            return null;
        }

        private static string GetDown(this OcrResult source, OcrResult.Word keyWord, int treshould = 10)
        {
            var nextLine = Array.FindAll(source.Words, f =>
                f.Y > keyWord.Y + keyWord.Height);
            var valWord = Array.Find(nextLine, f =>
                f.X > keyWord.X - treshould);
            return valWord?.Text;
        }

        public static object[] List(this OcrResult source)
        {
            var keys = new[] { "serial no", "bale no", "moisture", "gross", "net", "bisfa" };
            var head = Array.Find(source.Lines, f => 
            {
                return Array.Find(keys, k => f.Text.ToLowerInvariant().Contains(k)) != null;
            });

            var body = Array.FindAll(source.Lines, f => 
            {
                return f.Y > head.Y;
            });

            return body.To();
        }

        private static object[] To(this OcrResult.Line[] sources)
        {
            var result = new List<object>();
            foreach (var line in sources)
            {
                var inResult = new List<string>();
                foreach (var item in line.Words)
                {
                    inResult.Add(item.Text);
                }
                result.Add(inResult);
            }
            return result.ToArray();
        }
    }
}
