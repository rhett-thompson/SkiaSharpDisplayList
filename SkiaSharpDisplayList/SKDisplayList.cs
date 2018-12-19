using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

                            lastFrameTime = renderInfo.Elapsed;
                            renderInfo.Elapsed = stopWatch.Elapsed.TotalSeconds;
                            renderInfo.Delta = renderInfo.Elapsed - lastFrameTime;

                            renderInfo.FrameRate = frames / renderInfo.Elapsed;

                            updateAction();

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
        public double FPSTarget { set { targetFrameTime = 1.0 / value; } }
        public SKDisplayObject Stage = new SKDisplayObject { isStage = true };

        private Stopwatch stopWatch = new Stopwatch();
        private double lastFrameTime;
        private bool running = false;
        private int frames;
        private double targetFrameTime;
        private SKDisplayObjectRenderInfo renderInfo = new SKDisplayObjectRenderInfo();
        private Action updateAction;

        private List<ActiveTween> activeTweens = new List<ActiveTween>();

        public SKDisplayList(Action UpdateAction)
        {
            FPSTarget = 60;
            updateAction = UpdateAction;

            Paused = false;
        }

        public void Update(SKCanvas canvas)
        {

            foreach (var tween in activeTweens.Where(x => !x.complete))
            {

                if (renderInfo.Elapsed > tween.startTime + tween.duration)
                {
                    tween.complete = true;
                    continue;
                }

                double v = tween.ease(renderInfo.Elapsed - tween.startTime, tween.startValue, tween.changeInValue, tween.duration);
                tween.propertyInfo.SetValue(tween.parent, Convert.ChangeType(v, tween.propertyInfo.PropertyType));

            }

            activeTweens.RemoveAll(x => x.complete);

            canvas.Clear(ClearColor);

            renderInfo.canvas = canvas;

            Stage.internalRender(Stage, renderInfo);
        }

        /// <summary>
        /// Fire and forget tween
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Change in value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public delegate double Ease(double t, double b, double c, double d);

        public void Tween<ParentT, TargetT>(ParentT parent, Expression<Func<ParentT, TargetT>> property, TargetT target, double duration, Ease ease)
        {

            var member = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)member.Member;

            var startValue = Convert.ToDouble(propertyInfo.GetValue(parent));
            var targetValue = Convert.ToDouble(target);

            var tween = new ActiveTween
            {
                duration = duration,
                startTime = renderInfo.Elapsed,
                ease = ease,
                parent = parent,
                propertyInfo = propertyInfo,
                startValue = startValue,
                changeInValue = targetValue - startValue,
                complete = false
            };

            activeTweens.Add(tween);

        }

        private class ActiveTween
        {
            public double duration;
            public double startTime;
            public double startValue;
            public double changeInValue;
            public Ease ease;
            public PropertyInfo propertyInfo;
            public object parent;
            public bool complete;
        }

    }
}
