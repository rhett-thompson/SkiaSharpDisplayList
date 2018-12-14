namespace SkiaSharp.DisplayList
{
    public sealed class SKDisplayObjectRenderInfo
    {
        public float Delta { get; set; }
        public float Elapsed { get; set; }

        internal SKCanvas canvas { get; set; }

    }
}
