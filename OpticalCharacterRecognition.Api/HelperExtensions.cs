using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api
{
    public static partial class HelperExtensions
    {
        public static string GetBLNumber(this IronOcr.OcrResult.Word[] source)
        {
            var keys = new string[] { "bl", "bill", "b/l" };
            var obx = source.OrderBy(ob => ob.X);
            var oby = source.OrderBy(ob => ob.Y);
            var res = source.FirstOrDefault(fod => keys.Contains(fod.Text.ToLowerInvariant()));

            var lookX = obx.FirstOrDefault(fod => fod.X > res.X);
            var lookY = oby.FirstOrDefault(fod => fod.Y > res.Y);

            return lookX.Text;
        }
    }
}
