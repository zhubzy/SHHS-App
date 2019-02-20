using System;
using CoreGraphics;
using SHHS.Controller;
using SHHS.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]
namespace SHHS.iOS
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
            Control.ProgressTintColor = Color.FromRgb(169, 169, 169).ToUIColor(); 
            Control.TrackTintColor = Color.FromRgb(255, 255, 255).ToUIColor();

        }


      

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var X = 1f;
            var Y = 10.0f;
            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            // this.Control.Transform = transform;
            this.Transform = transform;

            //this.ClipsToBounds = true;
            //this.Layer.MasksToBounds = true;

        }
    }
}
