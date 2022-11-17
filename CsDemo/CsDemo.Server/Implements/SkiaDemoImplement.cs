using Grpc.Core;
using SkiaSharp;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using System.Linq;
using CsDemo.Server.Properties;
using CsDemo.Server.Utils;

namespace CsDemo.Server.Implements;

public class SkiaDemoImplement : SkiaDemo.SkiaDemoBase
{
    private ILogger<SkiaDemoImplement> logger;

    public SkiaDemoImplement(ILogger<SkiaDemoImplement> logger)
    {
        this.logger = logger;
    }

    public override async Task<DrawBySkiaReply> DrawBySkia(DrawBySkiaRequest request, ServerCallContext context)
    {
        using var m = new MemoryStream(Resources.Test);
        using var skms = new SKManagedStream(m);
        using var bitmap = SKBitmap.Decode(skms);


        var skii = new SKImageInfo(bitmap.Width, bitmap.Height);
        using var surface = SKSurface.Create(skii);
        using var paint = new SKPaint
        {
            Color = SKColors.Cyan,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Left,
            TextSize = 40f,
            Typeface = SKTypeface.FromFamilyName("微软雅黑", SKFontStyleWeight.Light, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
            //Typeface = SkiaSharp.SKTypeface.Default,
            //Typeface = SkiaSharp.SKTypeface.FromFile("fontpath", 0), // 加载字体
        };

        var pad = 8;
        var tws = paint.ParseTextWrap(request.Contents, bitmap.Width - pad / 2);

        var height = tws.Select(i => i.Height + pad).Aggregate((a, b) => a + b) + pad;
        var drawImageInfo = new SKImageInfo(bitmap.Width, (int)(height + bitmap.Height));
        using var drawSuface = SKSurface.Create(drawImageInfo);
        drawSuface.Canvas.Clear(SKColors.White);

        var point = new SKPoint(pad, pad);
        foreach (var tw in tws)
        {
            point.Y += tw.Height;
            drawSuface.Canvas.DrawText(tw.Content, point, paint);
            point.Y += pad;
        }
        
        drawSuface.Canvas.DrawBitmap(bitmap, new SKRect
        {
            Left = 0,
            Top = point.Y,
            Right = bitmap.Width,
            Bottom = (int)(point.Y + bitmap.Height),
        });

        using var r = drawSuface.Snapshot();
        using var d = r.Encode(SKEncodedImageFormat.Jpeg, 90);

        return new DrawBySkiaReply
        {
            Code = 0,
            Message = $"imageSize: {bitmap.Width} {bitmap.Height}, height: {height}",
            Result = await ByteString.FromStreamAsync(d.AsStream()),
        };
    }
}
