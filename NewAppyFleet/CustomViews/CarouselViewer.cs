using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace NewAppyFleet.CustomViews
{
    public class SliderView : ContentView
    {
        View currentView;
        StackLayout dotLayout;
        double height, width;
        int dotCount = 1;

        public SliderView(View rootview, double h, double w, bool includeDots = true)
        {
            height = h;
            width = w;

            currentView = rootview;

            Children = new ObservableCollection<View>();
            Children.Insert(0, currentView);

            ViewScreen = new AbsoluteLayout
            {
                HeightRequest = height,
                WidthRequest = width,
            };

            dotLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
            };

            var whiteDot = new Button
            {
                BorderRadius = 5,
                HeightRequest = 10,
                WidthRequest = 10,
                StyleId = 0.ToString(),
                BackgroundColor = Color.White
            };

            if (includeDots)
            {
                dotLayout.Children.Add(whiteDot);

                Children.CollectionChanged += Children_CollectionChanged;
            }
                var dotRect = new Rectangle(
                                  x: width / 2 - (15) / 2,
                                  y: height - 15,
                                  width: 15,
                                  height: 10
                              );
            
            ViewScreen.Children.Add(currentView, new Rectangle(0, 0, width, height));
            if (includeDots)
                ViewScreen.Children.Add(dotLayout, dotRect);

            Content = ViewScreen;
        }

        void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateDots();
        }

        public int MinimumSwipeDistance { get; set; }

        public AbsoluteLayout ViewScreen { get; set; }

        public ObservableCollection<View> Children { get; set; }

        public View CurrentView { get; set; }

        public StackLayout DotStack => dotLayout;

        public uint TransitionLength { get; set; }

        public void UpdateDots()
        {
            var dotsToAdd = Children.Count - dotCount;

            if (dotsToAdd == 0)
                return;

            switch (dotsToAdd.ToString())
            {
                case "1": // Add a dot 
                    Button whiteDot = new Button
                    {
                        BorderRadius = 5,
                        HeightRequest = 10,
                        WidthRequest = 10,
                        StyleId = (dotCount).ToString(),
                        BackgroundColor = Color.White,
                        Opacity = 0.5,
                    };
                    dotLayout.Children.Add(whiteDot);
                    dotCount++;
                    break;

                case "-1": // Remove a dot
                    dotLayout.Children.RemoveAt(dotLayout.Children.Count - 1);
                    dotCount--;
                    break;
            }

            var dotRect = new Rectangle(
                              x: width / 2 - (dotCount * 15) / 2,
                              y: height - 15,
                              width: dotCount * 15,
                              height: 10
                          );
            ViewScreen.Children.Remove(dotLayout);
            ViewScreen.Children.Add(dotLayout, dotRect);
        }
    }
}
