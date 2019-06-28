using Android.Text.Util;
using Android.Util;
using Android.Widget;
using MainCRMV2;
using MainCRMV2.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.App.ActionBar;

[assembly: ExportRenderer(typeof(AwesomeHyperLinkLabel), typeof(AwesomeHyperLinkLabelRenderer))]
namespace MainCRMV2.Droid
{
    public class AwesomeHyperLinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (AwesomeHyperLinkLabel)Element;
            if (view == null) return;

#pragma warning disable CS0618 // Type or member is obsolete
            TextView textView = new TextView(Forms.Context);
#pragma warning restore CS0618 // Type or member is obsolete
            textView.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            textView.SetTextColor(view.TextColor.ToAndroid());

            // Setting the auto link mask to capture all types of link-able data
            textView.AutoLinkMask = MatchOptions.All;
            // Make sure to set text after setting the mask
            textView.Text = view.Text;
            textView.SetTextSize(ComplexUnitType.Dip, (float)view.FontSize);

            // overriding Xamarin Forms Label and replace with our native control
            SetNativeControl(textView);
        }
    }
}