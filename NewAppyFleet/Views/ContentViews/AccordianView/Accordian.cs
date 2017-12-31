using System.Collections;
using Xamarin.Forms;

namespace NewAppyFleet.Views.ContentViews.AccordianView
{
    /*public class DefaultTemplate : AbsoluteLayout
    {
        public DefaultTemplate()
        {
            Padding = 5;
            HeightRequest = 50;
            var title = new Label { HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand };
            var price = new Label { HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End };
            Children.Add(title, new Rectangle(0, 0.5, 0.5, 1), AbsoluteLayoutFlags.All);
            Children.Add(price, new Rectangle(1, 0.5, 0.5, 1), AbsoluteLayoutFlags.All);
            title.SetBinding(Label.TextProperty, "Date", stringFormat: "{0:dd MMM yyyy}");
            price.SetBinding(Label.TextProperty, "Amount", stringFormat: "{0:C2}");
        }
    }*/

    public class Accordian : ScrollView
    {
        StackLayout layout = new StackLayout { Spacing = 1 };

        public DataTemplate Template { get; set; }
        public DataTemplate SubTemplate { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                propertyName: "ItemsSource",
                returnType: typeof(IList),
                declaringType: typeof(AccordionSectionView),
                defaultValue: default(IList),
                propertyChanged: Accordian.PopulateList);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Accordian(DataTemplate itemTemplate)
        {
            SubTemplate = itemTemplate;
            Template = new DataTemplate(() => (object)(new AccordionSectionView(itemTemplate, this)));
            Content = layout;
        }

        void PopulateList()
        {
            layout.Children.Clear();

            foreach (object item in ItemsSource)
            {
                var template = (View)Template.CreateContent();
                template.BindingContext = item;
                layout.Children.Add(template);
            }
        }

        static void PopulateList(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((Accordian)bindable).PopulateList();
        }
    }

    public class AccordionSectionView : StackLayout
    {
        bool isExpanded = false;
        StackLayout content = new StackLayout { HeightRequest = 0 };
        Color headerColor = Color.FromHex("0067B7");
        ImageSource arrowRight = ImageSource.FromFile("ic_keyboard_arrow_right_white_24dp.png".CorrectedImageSource());
        ImageSource arrowDown = ImageSource.FromFile("ic_keyboard_arrow_down_white_24dp.png".CorrectedImageSource());
        AbsoluteLayout header = new AbsoluteLayout();
        Image headerIcon = new Image { VerticalOptions = LayoutOptions.Center };
        Label headerTitle = new Label { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 50 };
        DataTemplate template;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                propertyName: "ItemsSource",
                returnType: typeof(IList),
                declaringType: typeof(AccordionSectionView),
                defaultValue: default(IList),
                propertyChanged: AccordionSectionView.PopulateList);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                propertyName: "Title",
                returnType: typeof(string),
                declaringType: typeof(AccordionSectionView),
                propertyChanged: AccordionSectionView.ChangeTitle);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public AccordionSectionView(DataTemplate itemTemplate, ScrollView parent)
        {
            template = itemTemplate;
            headerTitle.BackgroundColor = headerColor;
            headerIcon.Source = arrowRight;
            header.BackgroundColor = headerColor;

            header.Children.Add(headerIcon, new Rectangle(0, 1, .1, 1), AbsoluteLayoutFlags.All);
            header.Children.Add(headerTitle, new Rectangle(1, 1, .9, 1), AbsoluteLayoutFlags.All);

            Spacing = 0;
            Children.Add(header);
            Children.Add(content);

            header.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        if (isExpanded)
                        {
                            headerIcon.Source = arrowRight;
                            content.HeightRequest = 0;
                            content.IsVisible = false;
                            isExpanded = false;
                        }
                        else
                        {
                            headerIcon.Source = arrowDown;
                            content.HeightRequest = content.Children.Count * 50;
                            content.IsVisible = true;
                            isExpanded = true;

                            // Scroll top by the current Y position of the section
                            if (parent.Parent is VisualElement)
                            {
                                await parent.ScrollToAsync(0, this.Y, true);
                            }
                        }
                    })
                }
            );
        }

        void ChangeTitle()
        {
            headerTitle.Text = this.Title;
        }

        void PopulateList()
        {
            content.Children.Clear();

            foreach (object item in this.ItemsSource)
            {
                var temp = (View)template.CreateContent();
                temp.BindingContext = item;
                content.Children.Add(temp);
            }
        }

        static void ChangeTitle(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionSectionView)bindable).ChangeTitle();
        }

        static void PopulateList(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionSectionView)bindable).PopulateList();
        }
    }
}
