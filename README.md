# SkiaSharpDisplayList
### A simple display list for **SkiaSharp**
------

A display list implementation for SkiaSharp that's similar to the Adobe Flash/Starling Display List approach.

The hierarchical structure of display lists have the following benefits:
* More efficient rendering and reduced memory usage
* Improved depth management
* Full traversal of the display list
* Off-list display objects
* Easier subclassing of display objects

The current release is very rudimentary, but is probably usable for simple User Interface scenarios; with the goal being a "platform" for advanced UI and perhaps 2D games with continued optimization and feature addition. The SkiaSharp.DisplayList package is decoupled from any platform view and only requires a valid SKCanvas each update to work its magic.

Here's some example code.

```csharp
//in your form constructor
var displayList = new SKDisplayListControl();
displayList.ClearColor = SKColors.Black;
displayList.Dock = DockStyle.Fill;
Controls.Add(displayList);

//create red test object
var redCircle = new SKDisplayObject();
redCircle.Position.Offset(200, 200);
redCircle.Render = (info, graphics) =>
{

	graphics.DrawCircle(50 * (float)Math.Sin(info.Elapsed), 0, 10, new SKPaint { Color = SKColors.Red });

};
displayList.Stage.Children.Add(redCircle);

//create blue test object
var blueCircle = new SKDisplayObject();
blueCircle.Position.X = 200;
blueCircle.Render = (info, graphics) =>
{

	graphics.DrawCircle(0, 0, 10, new SKPaint { Color = SKColors.Blue });
	blueCircle.Position.Y = 200 + (-50 * (float)Math.Sin(info.Elapsed));

};
displayList.Stage.Children.Add(blueCircle);

//create yellow test object
var orbit = new SKDisplayObject();
orbit.Render = (info, graphics) =>
{

	graphics.DrawCircle(0, 0, 2, new SKPaint { Color = SKColors.Yellow });
	orbit.Position.X = 15 * (float)Math.Cos(info.Elapsed);
	orbit.Position.Y = 15 * (float)Math.Sin(info.Elapsed);

};
blueCircle.Children.Add(orbit);

//a subclass of SKControl for convenience
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

```
