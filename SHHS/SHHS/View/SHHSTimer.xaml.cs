using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SHHS.View
{
    public partial class SHHSTimer : ContentView
    {
        SKPaint outlinePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 3,
            Color = SKColors.Black
        };
        
        SKPaint arcPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 15,
            Color = SKColors.Red,
            
        };

        SKPaint a = new SKPaint {   };

        SKCanvasView canvasView;

        public SHHSTimer()
        {
            InitializeComponent();

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += Handle_PaintSurface;
            canvasView.HeightRequest = 600;
            canvasView.BackgroundColor = Color.Blue;
            Stack.Children.Add(canvasView);
        }



        void Handle_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            SKRect rect = new SKRect((float)first.Value, (float)second.Value, info.Width - 100, info.Width - 100);
            float startAngle = (float)startAngleSlider.Value;
            float sweepAngle = (float)sweepAngleSlider.Value;

            canvas.DrawOval(rect, outlinePaint);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path, arcPaint);
            }
        }


        void SliderValueChanged(object sender, ValueChangedEventArgs args)
        {

                
                canvasView.InvalidateSurface();

        }


    }
}
