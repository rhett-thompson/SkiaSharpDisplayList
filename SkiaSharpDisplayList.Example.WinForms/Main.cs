using SkiaSharp.Views.Desktop;
using System;
using System.Windows.Forms;

namespace SkiaSharp.DisplayList.Example.WinForms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            var view = new SKDisplayListControl();
            view.DisplayList.FPSTarget = 100;
            view.DisplayList.ClearColor = SKColors.Black;
            view.Dock = DockStyle.Fill;
            Controls.Add(view);

            //create red test object
            var redCircle = new SKDisplayObject();
            redCircle.Position.Offset(200, 200);
            redCircle.Render = (info, graphics) =>
            {

                graphics.DrawCircle(50 * (float)Math.Sin(info.Elapsed), 0, 10, new SKPaint { Color = SKColors.Red });

            };
            view.DisplayList.Stage.Children.Add(redCircle);

            //create blue test object
            var blueCircle = new SKDisplayObject();
            blueCircle.Position.X = 200;
            blueCircle.Render = (info, graphics) =>
            {

                graphics.DrawCircle(0, 0, 10, new SKPaint { Color = SKColors.Blue });
                blueCircle.Position.Y = 200 + (-50 * (float)Math.Sin(info.Elapsed));

            };
            view.DisplayList.Stage.Children.Add(blueCircle);

            //create yellow test object
            var orbit = new SKDisplayObject();
            orbit.Render = (info, graphics) =>
            {

                graphics.DrawCircle(0, 0, 2, new SKPaint { Color = SKColors.Yellow });
                orbit.Position.X = 15 * (float)Math.Cos(info.Elapsed);
                orbit.Position.Y = 15 * (float)Math.Sin(info.Elapsed);

            };
            blueCircle.Children.Add(orbit);

            //draw framerate
            view.DisplayList.Stage.Render = (i, g) =>
            {

                g.DrawText(i.FrameRate.ToString(), 30, 30, new SKPaint { Color = SKColors.White, TextSize = 20 });

            };

        }
    }

    public class SKDisplayListControl : SKControl
    {

        public SKDisplayList DisplayList;

        public SKDisplayListControl()
        {

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);

            DisplayList = new SKDisplayList(() => { Invalidate(); });
            PaintSurface += (object sender, SKPaintSurfaceEventArgs e) => { DisplayList.Update(e.Surface.Canvas); };

        }

    }

}
