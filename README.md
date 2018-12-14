# SkiaSharpDisplayList
A simple display list for **SkiaSharp**

*Currently only supports supports WinForms*

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
```
