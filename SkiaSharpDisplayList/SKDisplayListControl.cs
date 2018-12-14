using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkiaSharp.DisplayList
{
    
    public class SKDisplayListControl : SKControl
    {
        
        public bool Paused;
        public SKColor ClearColor = SKColors.DarkCyan;
        public double FPSTarget = 60;
        public SKDisplayObject Stage = new SKDisplayObject { isStage = true };

        private Stopwatch stopWatch = new Stopwatch();
        private float lastFrameTime;

        public SKDisplayListControl()
        {

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);

            PaintSurface += (object sender, SKPaintSurfaceEventArgs e) =>
            {

                float elapsed = (float)stopWatch.Elapsed.TotalSeconds;
                float delta = elapsed - lastFrameTime;
                lastFrameTime = elapsed;

                e.Surface.Canvas.Clear(ClearColor);

                var renderInfo = new SKDisplayObjectRenderInfo { canvas = e.Surface.Canvas, Delta = delta, Elapsed = elapsed };
                Stage.internalRender(Stage, renderInfo);

            };

            Task.Run(async () =>
            {

                stopWatch.Start();
                while (!Paused)
                {

                    Invalidate();
                    await Task.Delay(TimeSpan.FromSeconds(1.0 / FPSTarget));

                }

            });

        }

    }
}
