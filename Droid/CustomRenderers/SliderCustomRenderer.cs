using System;
using System.Threading.Tasks;
using Android.Views;
using NewAppyFleet.CustomViews;
using NewAppyFleet.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AndroidView = Android.Views.View;

[assembly: ExportRenderer(typeof(SliderView), typeof(SliderViewRenderer))]
namespace NewAppyFleet.Droid.CustomRenderers
{
    public class GesutreListener : GestureDetector.SimpleOnGestureListener
    {
        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return base.OnFling(e1, e2, velocityX, velocityY);
        }
    }

    public class SliderViewRenderer : ViewRenderer<SliderView, AndroidView>
    {
        private readonly GesutreListener listener;
        private readonly GestureDetector detector;

        private float x1, x2;

        SliderView sliderView;

        int currentViewIndex = 0;

        public SliderViewRenderer()
        {
            listener = new GesutreListener();
            detector = new GestureDetector(listener);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SliderView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Touch += HandleGenericMotion;
                sliderView = (SliderView)e.NewElement;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (sliderView.CurrentView is Layout)
            {
                var view = (ViewGroup)this.ViewGroup.GetChildAt(0);
                Android.Views.View currentLayout = (ViewGroup)view.GetChildAt(0);
                currentLayout.Touch -= HandleGenericMotion;
            }
            Touch -= HandleGenericMotion;
            GC.Collect();
            base.Dispose(disposing);
        }

        async void HandleGenericMotion(object sender, TouchEventArgs e)
        {
            detector.OnTouchEvent(e.Event);

            switch (e.Event.Action)
            {

                case MotionEventActions.Down:
                    x1 = e.Event.GetX();
                    break;
                case MotionEventActions.Up:
                    x2 = e.Event.GetX();
                    float delta = x2 - x1;
                    if (Math.Abs(delta) > sliderView.MinimumSwipeDistance)
                    {
                        if (delta > 0)
                        {
                            if (currentViewIndex != 0)
                            {
                                currentViewIndex--;
                                sliderView.CurrentView = sliderView.Children[currentViewIndex];
                                bool loading = await TranslateToCurrentViewAsync("Right");

                                if (sliderView.Children[currentViewIndex + 1] is Layout)
                                {
                                    var view = (ViewGroup)this.ViewGroup.GetChildAt(0);
                                    Android.Views.View currentLayout = (ViewGroup)view.GetChildAt(0);
                                    currentLayout.Touch -= HandleGenericMotion;
                                }
                                sliderView.ViewScreen.Children.Remove(sliderView.Children[currentViewIndex + 1]);
                            }
                        }
                        else if (delta < 0)
                        {
                            if (sliderView.Children.Count > currentViewIndex + 1)
                            {
                                currentViewIndex++;
                                sliderView.CurrentView = sliderView.Children[currentViewIndex];
                                bool loading = await TranslateToCurrentViewAsync("Left");

                                if (sliderView.Children[currentViewIndex - 1] is Layout)
                                {
                                    var view = (ViewGroup)this.ViewGroup.GetChildAt(0);
                                    Android.Views.View currentLayout = (ViewGroup)view.GetChildAt(0);
                                    currentLayout.Touch -= HandleGenericMotion;
                                }
                                sliderView.ViewScreen.Children.Remove(sliderView.Children[currentViewIndex - 1]);
                            }
                        }
                    }
                    break;
            }
        }

        public async Task<bool> TranslateToCurrentViewAsync(string direction)
        {
            var initialLayoutRect = new Rectangle(
                                        0,
                                        0,
                                        sliderView.Width,
                                        sliderView.Height
                                    );
            var error = false;

            var dotRect = new Rectangle(
                              x: sliderView.ViewScreen.Width / 2 - (sliderView.DotStack.Children.Count * 15) / 2,
                              y: sliderView.ViewScreen.Height - 15,
                              width: sliderView.DotStack.Children.Count * 15,
                              height: 10
                          );

            sliderView.ViewScreen.Children.Remove(sliderView.DotStack);
            sliderView.UpdateDots();

            foreach (var dot in sliderView.DotStack.Children)
                dot.Opacity = dot.StyleId == currentViewIndex.ToString() ? 1 : 0.5;

            switch (direction)
            {
                case "Right":
                    initialLayoutRect.X = -sliderView.ParentView.Width;
                    sliderView.ViewScreen.Children.Add(sliderView.CurrentView, initialLayoutRect);
                    sliderView.ViewScreen.Children.Add(sliderView.DotStack, dotRect);

                    error = await sliderView.CurrentView.TranslateTo(sliderView.ParentView.Width, 0, sliderView.TransitionLength);
                    break;
                case "Left":
                    initialLayoutRect.X = sliderView.ParentView.Width;

                    sliderView.ViewScreen.Children.Add(sliderView.CurrentView, initialLayoutRect);
                    sliderView.ViewScreen.Children.Add(sliderView.DotStack, dotRect);

                    error = await sliderView.CurrentView.TranslateTo(-sliderView.ParentView.Width, 0, sliderView.TransitionLength);
                    break;
            }

            if (sliderView.CurrentView is Layout)
            {
                var view = (ViewGroup)this.ViewGroup.GetChildAt(0);
                Android.Views.View currentLayout = (ViewGroup)view.GetChildAt(1);
                currentLayout.Touch += HandleGenericMotion;
            }

            return error;
        }
    }
}
