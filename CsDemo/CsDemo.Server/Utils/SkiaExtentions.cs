

using SkiaSharp;
using System;
using System.Collections.Generic;

namespace CsDemo.Server.Utils;

public struct SkiaWrapText
{
    public float Height { get; set; }
    public string Content { get; set; }
}

public static class SkiaExtentions
{
    public static IEnumerable<SkiaWrapText> ParseTextWrap(this SKPaint paint, IEnumerable<string> contents, int maxWidth)
    {
        var result = new List<SkiaWrapText>();

        foreach (var item in contents)
        {
            var r = ParseTextWrap(paint, item, maxWidth);
            result.AddRange(r);
        }

        return result;
    }

    public static IEnumerable<SkiaWrapText> ParseTextWrap(this SKPaint paint, string text, int maxWidth)
    {
        var result = new List<SkiaWrapText>();

        var bound = SKRect.Empty;

        var head = 0;

        while (head < text.Length)
        {
            var count = (int)paint.BreakText(text.AsSpan(head, text.Length - head), maxWidth); // 每行个数
            var ts = text.Substring(head, count);
            paint.MeasureText(ts, ref bound); // 没有换行，但是可以获取行高
            result.Add(new SkiaWrapText
            {
                Height = bound.Height,
                Content = ts,
            });
            head += count;
        }

        return result;
    }
}
