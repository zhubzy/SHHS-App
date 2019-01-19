using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SHHS.View
{
    public partial class SHHSTimer : ContentView
    {
        float _sweepingAngleSlider;
        float _startingAngleSlider;
        string _timeLeft = "Loading";
        string _periodInfo = " ";
        public string line1 = " ";
        public string line2 = " ";
        public Boolean isActive;

        public float SweepAngleSlider { get { return _sweepingAngleSlider; } set { _sweepingAngleSlider = value; canvasView.InvalidateSurface(); } }
        public float StartAngleSlider { get { return _startingAngleSlider; } set { _startingAngleSlider = value; canvasView.InvalidateSurface(); } }
        public string TimeLeft
        {
            get { return _timeLeft; }
            set
            {
                
                _timeLeft = value;
                textPaint.TextSize = 12;
                textWidth = textPaint.MeasureText(_timeLeft);
                canvasView.InvalidateSurface();

            }
        }
        public string PeriodInfo
        {
            get { return _periodInfo; }
            set
            {
                _periodInfo = value;
                infoPaint.TextSize = 12;
                infoWidth = infoPaint.MeasureText(_periodInfo);
                canvasView.InvalidateSurface();
            }
        }

        public int minutes;
        public int seconds;
        public int length;
        float infoWidth;
        float textWidth;


        SKPaint outlinePaint = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            StrokeWidth = 4,
            Color = SKColor.FromHsl(75, 47, 87),


        };
        SKPaint textPaint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Black,
            Typeface = SKTypeface.FromFile("OpenSans-Bold.ttf")
        };
        SKPaint infoPaint = new SKPaint
        {
            IsAntialias = true,
            Color = SKColors.Black,
            Typeface = SKTypeface.FromFile("OpenSans-Light.ttf")

        };
        SKPaint arcPaint = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 8,
            Color = SKColor.FromHsl(103, 59, 58),

        };

        public SKCanvasView canvasView;

        public SHHSTimer()
        {
            InitializeComponent();

            canvasView = new SKCanvasView
            {
                IgnorePixelScaling = false
            };
            canvasView.PaintSurface += Handle_PaintSurface;
            infoWidth = infoPaint.MeasureText(PeriodInfo);
            textWidth = textPaint.MeasureText(TimeLeft);
            Content = canvasView;

        }



        void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            SKRect rect = new SKRect(5, 5, info.Height - 5, info.Height - 5);
            float startAngle = StartAngleSlider;
            float sweepAngle = SweepAngleSlider;

            canvas.DrawOval(rect, outlinePaint);
            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path, arcPaint);
            }


            infoPaint.Color = SKColor.FromHsl(211, 14, 32);

            textPaint.TextSize = 0.8f * rect.Width * 12 / textWidth;
            infoPaint.TextSize = 0.7f * rect.Width * 12 / infoWidth;
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(TimeLeft, ref textBounds);
            SKRect infoBounds = new SKRect();
            infoPaint.MeasureText(PeriodInfo, ref infoBounds);
            // And draw the text
            canvas.DrawText(TimeLeft, rect.Width / 2 - textBounds.MidX, rect.Height / 2F - textBounds.MidY, textPaint);
            canvas.DrawText(PeriodInfo, rect.Width / 2 - infoBounds.MidX, rect.Height / 1.8F - infoBounds.MidY + textBounds.Height - infoBounds.Height, infoPaint);

            infoPaint.Color = SKColors.White;

            float scheduleWidth;
            infoPaint.TextSize = 12;

            if (line1.Length > line2.Length){

                scheduleWidth = infoPaint.MeasureText(line1);
                infoPaint.TextSize = 0.7f * (info.Width - rect.Width) * 12 / scheduleWidth;

            } else {

                scheduleWidth = infoPaint.MeasureText(line2);
                infoPaint.TextSize = 0.7f * (info.Width - rect.Width) * 12 / scheduleWidth;

            }


            SKRect scheduleBound = new SKRect();
            infoPaint.MeasureText(line1, ref scheduleBound);
            canvas.DrawText(line1, rect.Width + (info.Width - rect.Width) / 2 - scheduleBound.MidX, info.Height / 2F , infoPaint);

            scheduleBound = new SKRect();
            infoPaint.MeasureText(line2, ref scheduleBound);
            canvas.DrawText(line2, rect.Width + (info.Width - rect.Width) / 2 - scheduleBound.MidX, info.Height / 2F - scheduleBound.MidY + scheduleBound.Height, infoPaint);

        }


    }
}
