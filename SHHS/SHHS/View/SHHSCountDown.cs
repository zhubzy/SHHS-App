using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SHHS.View
{
    public class SHHSCountDown : SHHSTimer
    {
         float COUNTDOWN_LABEL_HEIGHT = 130;
        const int COUNTDOWN_TIMER_SPACING = 10;
        DateTime CountDownDate;
        String EventTitle;

        public SHHSCountDown(DateTime date, string title)
        {
            CountDownDate = date;
            EventTitle = title;
            canvasView = new SKCanvasView
            {
                IgnorePixelScaling = false
            };
            Content = canvasView;

            canvasView.PaintSurface += CanvasView_PaintSurface;


            Device.StartTimer(TimeSpan.FromSeconds(1/60), () => 
            {
                canvasView.InvalidateSurface();
                return true;
            
            });
        }

        void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {


            CalculateTime();
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            infoPaint.Color = SKColors.White;
            SKRect titileBound = GetTextBound(infoPaint, info.Rect, EventTitle, 0.5F);
            canvas.DrawText(EventTitle, (info.Rect.Right + info.Rect.Left) / 2F - titileBound.MidX,  titileBound.Height+15, infoPaint);
            COUNTDOWN_LABEL_HEIGHT = (info.Height - titileBound.Bottom) / 2F;



            SKRect rect = new SKRect(COUNTDOWN_TIMER_SPACING, COUNTDOWN_LABEL_HEIGHT-45, info.Width / 5 , info.Width / 5  - COUNTDOWN_TIMER_SPACING + COUNTDOWN_LABEL_HEIGHT-45);
            SKRect rect2 = new SKRect((info.Width * 2 / 5) , COUNTDOWN_LABEL_HEIGHT-45, (info.Width * 2 / 5 ) - (info.Width/5) + COUNTDOWN_TIMER_SPACING , info.Width / 5 - COUNTDOWN_TIMER_SPACING + COUNTDOWN_LABEL_HEIGHT-45);
            SKRect rect3 = new SKRect((info.Width * 3 / 5) , COUNTDOWN_LABEL_HEIGHT-45, (info.Width  * 3 / 5 ) - (info.Width / 5) + COUNTDOWN_TIMER_SPACING, info.Width / 5 - COUNTDOWN_TIMER_SPACING + COUNTDOWN_LABEL_HEIGHT-45);
            SKRect rect4 = new SKRect((info.Width * 4 / 5), COUNTDOWN_LABEL_HEIGHT-45, (info.Width * 4 / 5) - (info.Width / 5 ) + COUNTDOWN_TIMER_SPACING, info.Width / 5 - COUNTDOWN_TIMER_SPACING + COUNTDOWN_LABEL_HEIGHT-45);
            SKRect rect5 = new SKRect((info.Width * 5 / 5), COUNTDOWN_LABEL_HEIGHT-45, (info.Width * 5 / 5) - (info.Width / 5) + COUNTDOWN_TIMER_SPACING, info.Width / 5 - COUNTDOWN_TIMER_SPACING + COUNTDOWN_LABEL_HEIGHT-45);


            canvas.DrawRoundRect(new SKRoundRect(rect,  10, 10), outlinePaint);
            canvas.DrawRoundRect(new SKRoundRect(rect2, 10, 10), outlinePaint);
            canvas.DrawRoundRect(new SKRoundRect(rect3, 10, 10), outlinePaint);
            canvas.DrawRoundRect(new SKRoundRect(rect4, 10, 10), outlinePaint);
            canvas.DrawRoundRect(new SKRoundRect(rect5, 10, 10), outlinePaint);

            // And draw the text


            //canvas.DrawText("123", rect.Left, rect.Height / 2F - bounds.MidY, textPaint);
            SKRect infoBound = GetTextBound(textPaint, rect, "Milliseconds", 0.8F);
            textPaint.Color = SKColors.Black.WithAlpha((byte)(0xFF * (1 - 0.5)));
            textPaint.MeasureText("Days", ref infoBound);
            canvas.DrawText("Days", (rect.Right + rect.Left) / 2F - infoBound.MidX, rect.Top - 10 , textPaint);
            textPaint.MeasureText("Hours", ref infoBound);
            canvas.DrawText("Hours", (rect2.Right + rect2.Left) / 2F - infoBound.MidX, rect.Top - 10 , textPaint);
            textPaint.MeasureText("Minutes", ref infoBound);
            canvas.DrawText("Minutes", (rect3.Right + rect3.Left) / 2F - infoBound.MidX, rect.Top - 10, textPaint);
            textPaint.MeasureText("Seconds", ref infoBound);
            canvas.DrawText("Seconds", (rect4.Right + rect4.Left) / 2F - infoBound.MidX, rect.Top - 10, textPaint);
            textPaint.MeasureText("Milliseconds", ref infoBound);
            canvas.DrawText("Miliseconds", (rect5.Right + rect5.Left) / 2F - infoBound.MidX, rect.Top - 10, textPaint);
            SKRect bounds = GetTextBound(textPaint, rect, "999", 0.8F);
            canvas.DrawText(miliSecs.ToString("D3"), (rect5.Right + rect5.Left) / 2F - bounds.MidX, rect.Height / 2F - bounds.MidY + COUNTDOWN_LABEL_HEIGHT-45, textPaint);

            if (days > 99)
            {
                canvas.DrawText(days.ToString("D3"), (rect.Right + rect.Left) / 2F - bounds.MidX, rect.Height / 2F - bounds.MidY + COUNTDOWN_LABEL_HEIGHT-45, textPaint);
           
           
           } else {
                bounds = GetTextBound(textPaint, rect, "00", 0.8F);
                canvas.DrawText(days.ToString("D2"), (rect.Right + rect.Left) / 2F - bounds.MidX, rect.Height / 2F - bounds.MidY + COUNTDOWN_LABEL_HEIGHT-45, textPaint);
            }


            textPaint.MeasureText("00", ref bounds);
            canvas.DrawText(hours.ToString("D2"), (rect2.Right + rect2.Left) / 2F - bounds.MidX, rect.Height / 2F - bounds.MidY + COUNTDOWN_LABEL_HEIGHT-45, textPaint);
            canvas.DrawText(minutes.ToString("D2"), (rect3.Right + rect3.Left) / 2F - bounds.MidX , rect.Height / 2F - bounds.MidY + COUNTDOWN_LABEL_HEIGHT-45 , textPaint);
            canvas.DrawText(seconds.ToString("D2"), (rect4.Right + rect4.Left) / 2F - bounds.MidX , rect.Height / 2F - bounds.MidY + COUNTDOWN_LABEL_HEIGHT-45, textPaint);


        }


        private SKRect GetTextBound(SKPaint paint, SKRect rect, string text, float perecent) {
            paint.TextSize = 12;
            textWidth = paint.MeasureText(text);
            paint.TextSize = perecent * rect.Width * 12 / textWidth;
            SKRect textBounds = new SKRect();
            paint.MeasureText(text, ref textBounds);
            return textBounds;
        
        
        }


        private void CalculateTime() {

            var timeSpan = CountDownDate.Subtract(DateTime.Now);
            days = timeSpan.Days;
            hours = timeSpan.Hours;
            minutes = timeSpan.Minutes;
            seconds = timeSpan.Seconds;
            miliSecs = timeSpan.Milliseconds;
        }
    }
}
