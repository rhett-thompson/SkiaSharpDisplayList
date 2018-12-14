using System;

namespace SkiaSharp.DisplayList
{
    public class SKDisplayObjectGraphics
    {

        internal bool calculateBounds;
        internal SKCanvas canvas;
        internal SKDisplayObject displayObject;

        public void DrawLine(SKPoint p0, SKPoint p1, SKPaint paint)
        {
            DrawLine(p0.X, p0.Y, p1.X, p1.Y, paint);
        }

        public void DrawLine(float x0, float y0, float x1, float y1, SKPaint paint)
        {
            canvas.DrawLine(x0, y0, x1, y1, paint);

            if (calculateBounds)
                displayObject.addBoundingRect(x0, y0, x0 + x1, y0 + y1);

        }

        public void DrawRect(float x, float y, float w, float h, SKPaint paint)
        {
            DrawRect(SKRect.Create(x, y, w, h), paint);
        }

        public void DrawRect(SKRect rect, SKPaint paint)
        {
            canvas.DrawRect(rect, paint);

            if (calculateBounds)
                displayObject.addBoundingRect(rect);

        }

        public void DrawRoundRect(SKRoundRect rect, SKPaint paint)
        {
            canvas.DrawRoundRect(rect, paint);

            if (calculateBounds)
                displayObject.addBoundingRect(rect.Rect);

        }

        public void DrawRoundRect(float x, float y, float w, float h, float rx, float ry, SKPaint paint)
        {
            DrawRoundRect(SKRect.Create(x, y, w, h), rx, ry, paint);
        }

        public void DrawRoundRect(SKRect rect, float rx, float ry, SKPaint paint)
        {
            canvas.DrawRoundRect(rect, rx, ry, paint);

            if (calculateBounds)
                displayObject.addBoundingRect(rect);

        }

        public void DrawRoundRect(SKRect rect, SKSize r, SKPaint paint)
        {
            DrawRoundRect(rect, r.Width, r.Height, paint);
        }

        public void DrawOval(float cx, float cy, float rx, float ry, SKPaint paint)
        {
            DrawOval(new SKRect(cx - rx, cy - ry, cx + rx, cy + ry), paint);
        }

        public void DrawOval(SKPoint c, SKSize r, SKPaint paint)
        {
            DrawOval(c.X, c.Y, r.Width, r.Height, paint);
        }

        public void DrawOval(SKRect rect, SKPaint paint)
        {
            canvas.DrawOval(rect, paint);

            if (calculateBounds)
                displayObject.addBoundingRect(rect);

        }

        public void DrawCircle(float cx, float cy, float radius, SKPaint paint)
        {
            canvas.DrawCircle(cx, cy, radius, paint);

            if (calculateBounds)
                displayObject.addBoundingCircle(cx, cy, radius);

        }

        public void DrawCircle(SKPoint c, float radius, SKPaint paint)
        {
            DrawCircle(c.X, c.Y, radius, paint);
        }

        public void DrawPath(SKPath path, SKPaint paint)
        {
            canvas.DrawPath(path, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(path.GetRect());
        }

        public void DrawPoints(SKPointMode mode, SKPoint[] points, SKPaint paint)
        {
            canvas.DrawPoints(mode, points, paint);
            if (calculateBounds)
                displayObject.addBoundingPoints(points);
        }

        public void DrawPoint(SKPoint p, SKPaint paint)
        {
            DrawPoint(p.X, p.Y, paint);
        }

        public void DrawPoint(float x, float y, SKPaint paint)
        {
            canvas.DrawPoint(x, y, paint);

            if (calculateBounds)
                displayObject.addBoundingPoint(x, y);

        }

        public void DrawPoint(SKPoint p, SKColor color)
        {
            DrawPoint(p.X, p.Y, color);
        }

        public void DrawPoint(float x, float y, SKColor color)
        {
            using (var paint = new SKPaint { Color = color })
            {
                DrawPoint(x, y, paint);
            }
        }

        public void DrawImage(SKImage image, SKPoint p, SKPaint paint = null)
        {
            DrawImage(image, p.X, p.Y, paint);

            if (calculateBounds)
                displayObject.addBoundingRect(p.X, p.Y, image.Width, image.Height);

        }

        public void DrawImage(SKImage image, float x, float y, SKPaint paint = null)
        {
            canvas.DrawImage(image, x, y, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(x, y, image.Width, image.Height);
        }

        public void DrawImage(SKImage image, SKRect dest, SKPaint paint = null)
        {
            canvas.DrawImage(image, dest, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(dest);
        }

        public void DrawImage(SKImage image, SKRect source, SKRect dest, SKPaint paint = null)
        {
            canvas.DrawImage(image, source, dest, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(dest);
        }

        public void DrawPicture(SKPicture picture, float x, float y, SKPaint paint = null)
        {
            var matrix = SKMatrix.MakeTranslation(x, y);
            DrawPicture(picture, ref matrix, paint);
        }

        public void DrawPicture(SKPicture picture, SKPoint p, SKPaint paint = null)
        {
            DrawPicture(picture, p.X, p.Y, paint);
        }

        public void DrawPicture(SKPicture picture, ref SKMatrix matrix, SKPaint paint = null)
        {
            canvas.DrawPicture(picture, ref matrix, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(matrix.TransX, matrix.TransY, picture.CullRect.Width, picture.CullRect.Height);
        }

        public void DrawPicture(SKPicture picture, SKPaint paint = null)
        {
            canvas.DrawPicture(picture, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(picture.CullRect);
        }

        public void DrawBitmap(SKBitmap bitmap, SKPoint p, SKPaint paint = null)
        {
            DrawBitmap(bitmap, p.X, p.Y, paint);
        }

        public void DrawBitmap(SKBitmap bitmap, float x, float y, SKPaint paint = null)
        {
            canvas.DrawBitmap(bitmap, x, y, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(x, y, bitmap.Width, bitmap.Height);
        }

        public void DrawBitmap(SKBitmap bitmap, SKRect dest, SKPaint paint = null)
        {
            canvas.DrawBitmap(bitmap, dest, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(dest);
        }

        public void DrawBitmap(SKBitmap bitmap, SKRect source, SKRect dest, SKPaint paint = null)
        {
            canvas.DrawBitmap(bitmap, source, dest, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(dest);
        }

        public void DrawSurface(SKSurface surface, SKPoint p, SKPaint paint = null)
        {
            DrawSurface(surface, p.X, p.Y, paint);
        }

        public void DrawSurface(SKSurface surface, float x, float y, SKPaint paint = null)
        {
            surface.Draw(canvas, x, y, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(x, y, canvas.LocalClipBounds.Width, canvas.LocalClipBounds.Height);
        }

        public void DrawText(SKTextBlob text, float x, float y, SKPaint paint)
        {
            canvas.DrawText(text, x, y, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(text.Bounds);
        }

        public void DrawText(string text, SKPoint p, SKPaint paint)
        {
            DrawText(text, p.X, p.Y, paint);
        }

        public void DrawText(string text, float x, float y, SKPaint paint)
        {
            var bytes = StringUtilities.GetEncodedText(text, paint.TextEncoding);
            DrawText(bytes, x, y, paint);
        }

        public void DrawText(byte[] text, SKPoint p, SKPaint paint)
        {
            DrawText(text, p.X, p.Y, paint);
        }

        public void DrawText(byte[] text, float x, float y, SKPaint paint)
        {
            canvas.DrawText(text, x, y, paint);
            if (calculateBounds)
                displayObject.addBoundingPoint(x, y);
        }

        public void DrawPositionedText(string text, SKPoint[] points, SKPaint paint)
        {

            var bytes = StringUtilities.GetEncodedText(text, paint.TextEncoding);
            DrawPositionedText(bytes, points, paint);
        }

        public void DrawPositionedText(byte[] text, SKPoint[] points, SKPaint paint)
        {

            canvas.DrawPositionedText(text, points, paint);
            if (calculateBounds)
                displayObject.addBoundingPoints(points);
        }

        public void DrawTextOnPath(IntPtr buffer, int length, SKPath path, SKPoint offset, SKPaint paint)
        {
            DrawTextOnPath(buffer, length, path, offset.X, offset.Y, paint);
        }

        public void DrawTextOnPath(IntPtr buffer, int length, SKPath path, float hOffset, float vOffset, SKPaint paint)
        {

            canvas.DrawTextOnPath(buffer, length, path, hOffset, vOffset, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(path.GetRect());
        }

        public void DrawText(IntPtr buffer, int length, SKPoint p, SKPaint paint)
        {
            DrawText(buffer, length, p.X, p.Y, paint);
        }

        public void DrawText(IntPtr buffer, int length, float x, float y, SKPaint paint)
        {

            canvas.DrawText(buffer, length, x, y, paint);
            if (calculateBounds)
                displayObject.addBoundingPoint(x, y);
        }

        public void DrawPositionedText(IntPtr buffer, int length, SKPoint[] points, SKPaint paint)
        {

            canvas.DrawPositionedText(buffer, length, points, paint);
            if (calculateBounds)
                displayObject.addBoundingPoints(points);
        }

        public void DrawTextOnPath(string text, SKPath path, SKPoint offset, SKPaint paint)
        {
            DrawTextOnPath(text, path, offset.X, offset.Y, paint);
        }

        public void DrawTextOnPath(string text, SKPath path, float hOffset, float vOffset, SKPaint paint)
        {

            var bytes = StringUtilities.GetEncodedText(text, paint.TextEncoding);
            DrawTextOnPath(bytes, path, hOffset, vOffset, paint);
        }

        public void DrawTextOnPath(byte[] text, SKPath path, SKPoint offset, SKPaint paint)
        {
            DrawTextOnPath(text, path, offset.X, offset.Y, paint);
        }

        public void DrawTextOnPath(byte[] text, SKPath path, float hOffset, float vOffset, SKPaint paint)
        {

            canvas.DrawTextOnPath(text, path, hOffset, vOffset, paint);
            if (calculateBounds)
                displayObject.addBoundingRect(path.GetRect());
        }


    }
}
