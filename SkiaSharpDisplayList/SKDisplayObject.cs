using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SkiaSharp.DisplayList
{
    public class SKDisplayObject
    {
        public ObservableCollection<SKDisplayObject> Children { get; } = new ObservableCollection<SKDisplayObject>();

        public SKPoint Position;
        public SKPoint Scale = new SKPoint(1, 1);
        public SKPoint Pivot;
        public SKSize Size;
        public float Rotation;
        public bool CalculateBounds = true;

        internal SKRect boundsInternal = new SKRect();
        public SKRect Bounds
        {
            get { return boundsInternal; }
        }

        internal List<(float, float)> boundingPoints = new List<(float, float)>();

        public bool DrawDebug = false;
        public SKPaint BoundingBoxPaint = new SKPaint { Color = SKColors.Red, IsStroke = true, StrokeWidth = 1 };
        public SKPaint SizePaint = new SKPaint { Color = SKColors.Blue, IsStroke = true, StrokeWidth = 2 };
        public SKPaint PositionPaint = new SKPaint { Color = SKColors.Yellow };
        public SKPaint PivotPaint = new SKPaint { Color = SKColors.LimeGreen };

        internal SKDisplayObject parentInternal;
        public SKDisplayObject Parent { get { return parentInternal; } }

        internal bool isStage = false;

        internal SKDisplayObjectGraphics graphics;

        public RenderMethod Render { get; set; }
        public Action Removed { get; set; }
        public Action Added { get; set; }

        public delegate void RenderMethod(SKDisplayObjectRenderInfo info, SKDisplayObjectGraphics graphics);

        public SKDisplayObject()
        {
            Children.CollectionChanged += ChildrenChanged;

            graphics = new SKDisplayObjectGraphics { displayObject = this };

        }

        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var i in e.NewItems)
                    ((SKDisplayObject)i).Added?.Invoke();
            else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset)
                foreach (var i in e.OldItems)
                    ((SKDisplayObject)i).Removed?.Invoke();
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var i in e.NewItems)
                    ((SKDisplayObject)i).Added?.Invoke();
                foreach (var i in e.OldItems)
                    ((SKDisplayObject)i).Removed?.Invoke();
            }
        }

        internal void internalRender(SKDisplayObject parent, SKDisplayObjectRenderInfo info)
        {

            parentInternal = parent;

            graphics.canvas = info.canvas;
            graphics.calculateBounds = CalculateBounds;

            if (parent.isStage)
                graphics.canvas.ResetMatrix();

            graphics.canvas.Scale(Scale.X, Scale.Y, Pivot.X, Pivot.Y);
            graphics.canvas.RotateRadians(Rotation, Pivot.X, Pivot.Y);
            graphics.canvas.Translate(Position.X, Position.Y);

            boundingPoints.Clear();

            if (parent.isStage)
                addBoundingPoint(0, 0);

            Render?.Invoke(info, graphics);

            foreach (var child in Children)
            {
                child.internalRender(this, info);

                if (CalculateBounds)
                {
                    boundingPoints.AddRange(child.boundingPoints);
                    updateBounds();
                }

            }

            if (DrawDebug)
            {
                graphics.DrawRect(0, 0, Size.Width, Size.Height, SizePaint);
                graphics.DrawRect(boundsInternal, BoundingBoxPaint);
                graphics.DrawCircle(0, 0, 2, PositionPaint);
                graphics.DrawCircle(0, 0, 2, PivotPaint);
            }

        }

        internal void addBoundingCircle(float x, float y, float radius)
        {

            addBoundingRect(x - radius, y - radius, radius * 2, radius * 2);

        }

        internal void addBoundingRect(SKRect rect)
        {
            addBoundingRect(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        internal void addBoundingRect(float x, float y, float width, float height)
        {

            boundingPoints.AddRange(new[] {
                (x, y),
                (x + width, y),
                (x + width, y + height),
                (x, y + height)
            });

            updateBounds();

        }

        internal void addBoundingPoint(float x, float y)
        {

            boundingPoints.Add((x, y));
            updateBounds();

        }

        internal void addBoundingPoints(SKPoint[] points)
        {
            boundingPoints.AddRange(points.Select(x => (x.X, x.Y)));
            updateBounds();
        }

        internal void updateBounds()
        {

            boundsInternal.Left = boundingPoints.Min(x => x.Item1);
            boundsInternal.Top = boundingPoints.Min(x => x.Item2);
            boundsInternal.Right = boundingPoints.Max(x => x.Item1);
            boundsInternal.Bottom = boundingPoints.Max(x => x.Item2);

        }


    }
}
