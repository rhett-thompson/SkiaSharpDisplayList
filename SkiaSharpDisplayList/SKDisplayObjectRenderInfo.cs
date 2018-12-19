namespace SkiaSharp.DisplayList
{
    public sealed class SKDisplayObjectRenderInfo
    {
        public double Delta { get; set; }
        public double Elapsed { get; set; }
        public double FrameRate { get; set; }

        internal SKCanvas canvas { get; set; }

    }
}
