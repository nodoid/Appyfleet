using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppyFleet.UWP.UserControls
{
    public sealed partial class CustomLabelControl : UserControl
    {
        public CustomLabelControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CustomLabelControl), new PropertyMetadata(0));

        public SolidColorBrush LabelBackground
        {
            get { return (SolidColorBrush)GetValue(LabelBackgroundProperty); }
            set { SetValue(LabelBackgroundProperty, value); }
        }


        public static readonly DependencyProperty LabelBackgroundProperty =
            DependencyProperty.Register("LabelBackground", typeof(SolidColorBrush), typeof(CustomLabelControl), new PropertyMetadata(0));

        public CornerRadius LabelRadius
        {
            get { return (CornerRadius)GetValue(LabelRadiusProperty); }
            set { SetValue(LabelRadiusProperty, value); }
        }

        public static readonly DependencyProperty LabelRadiusProperty =
            DependencyProperty.Register("LabelRadius", typeof(CornerRadius), typeof(CustomLabelControl), new PropertyMetadata(0));

        public SolidColorBrush LabelForeground
        {
            get { return (SolidColorBrush)GetValue(LabelForegroundProperty); }
            set { SetValue(LabelForegroundProperty, value); }
        }


        public static readonly DependencyProperty LabelForegroundProperty =
            DependencyProperty.Register("LabelForeground", typeof(SolidColorBrush), typeof(CustomLabelControl), new PropertyMetadata(0));
    }
}
