using System;
using SHHS.Controller;
using SHHS.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]
namespace SHHS.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            Control.Max = 100;
            Control.ProgressDrawable.SetColorFilter(Color.FromRgb(198, 182, 130).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
            //Control.ProgressTintListColor.FromRgb(182, 231, 233).ToAndroid();
            Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.FromRgb(255, 255, 255).ToAndroid());
            Control.ScaleY = 10;

        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}