using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SkiaSharp.DisplayList
{
    public class SKDisplayList
    {

        public bool Paused
        {
            get { return running; }
            set
            {

                if (!running)
                {

                    running = true;
                    frames = 0;

                    Task.Run(async () =>
                    {

                        stopWatch.Start();
                        while (running)
                        {
                            frames++;

                            renderInfo.Elapsed = (float)stopWatch.Elapsed.TotalSeconds;
                            renderInfo.Delta = renderInfo.Elapsed - lastFrameTime;
                            lastFrameTime = renderInfo.Elapsed;

                            renderInfo.FrameRate = frames / renderInfo.Elapsed;

                            invalidateAction();

                            if (renderInfo.Delta < targetFrameTime)
                                await Task.Delay(TimeSpan.FromSeconds(targetFrameTime - renderInfo.Delta));

                        }
                        stopWatch.Stop();

                    });

                }
                else
                    running = false;

            }
        }

        public SKColor ClearColor = SKColors.DarkCyan;
        public float FPSTarget { set { targetFrameTime = 1.0f / value; } }
        public SKDisplayObject Stage = new SKDisplayObject { isStage = true };

        private Stopwatch stopWatch = new Stopwatch();
        private float lastFrameTime;
        private bool running = false;
        private int frames;
        private float targetFrameTime;
        private SKDisplayObjectRenderInfo renderInfo = new SKDisplayObjectRenderInfo();
        private Action invalidateAction;

        public SKDisplayList(Action InvalidateAction)
        {
            FPSTarget = 60;
            invalidateAction = InvalidateAction;
            
            Paused = false;
        }

        public void Update(SKCanvas canvas)
        {
            
            canvas.Clear(ClearColor);

            renderInfo.canvas = canvas;

            Stage.internalRender(Stage, renderInfo);
        }

    }
}
