using Grpc.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Google.Protobuf;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text;
using CsDemo.Server.Properties;
using System;

namespace CsDemo.Server.Implements;

public class ImageSharpDemoImplement : ImageSharpDemo.ImageSharpDemoBase
{
    private ILogger<ImageSharpDemoImplement> logger;

    public ImageSharpDemoImplement(ILogger<ImageSharpDemoImplement> logger)
    {
        this.logger = logger;
    }

    public override async Task<DrawByImageSharpReply> DrawByImageSharp(DrawByImageSharpRequest request, ServerCallContext context)
    {
        try
        {
            using var image = Image.Load(request.Picture.Span);
            var pad = 4;
            FontCollection fonts = new();
            fonts.AddSystemFonts();
            var ff = fonts.Add(new MemoryStream(Resources.SourceHanSerifCN_Light));

            Font f = ff.CreateFont(24);

            var fff = new List<FontFamily>();
            foreach (var n in new string[] { "微软雅黑", "宋体" })
            {
                if (fonts.TryGet(n, out FontFamily fsong))
                {
                    logger.LogInformation("加载备选字体 {}", n);
                    fff.Add(fsong);
                }
            }

            TextOptions options = new(f)
            {
                Origin = new Point(pad, pad),
                Font = f,
                WrappingLength = image.Width - pad * 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                FallbackFontFamilies = fff,
            };

            var text = string.Join("\n", request.Contents);
            FontRectangle fr = TextMeasurer.Measure(text, options);
            IBrush brush = Brushes.Horizontal(Color.Red, Color.Blue);
            IPen pen = Pens.Solid(Color.Green, 1);

            using var dimage = new Image<Rgba32>(image.Width, (int)(image.Height + pad * 2 + fr.Height));
            dimage.Mutate(ctx =>
            {
                logger.LogInformation("text length: {} byte count: {}", text.Length, Encoding.Default.GetByteCount(text));
                // 这个库目前是 beta 阶段，使用了固定的缓存，太长的字体就缓冲区溢出，真坑。
                ctx.DrawText(options, text.Substring(0, 111), brush, pen);
                //ctx.DrawText("aaa", f, brush, new PointF(pad, pad));
                var point = new Point(0, (int)(fr.Height + pad * 2));
                ctx.DrawImage(image, point, 1);
            });
            using var m = new MemoryStream();
            await dimage.SaveAsJpegAsync(m);
            m.Seek(0, SeekOrigin.Begin);
            return new DrawByImageSharpReply
            {
                Code = 0,
                Message = $"Ok: {m.Length}",
                Result = await ByteString.FromStreamAsync(m),
            };
        }
        catch (Exception e)
        {
            logger.LogError("生成失败 {}", e);
            return new DrawByImageSharpReply
            {
                Code = -1,
                Message = e.Message,
            };
        }
    }
}

