using mvvmframework;
using System;
using Xamarin.Forms;

namespace NewAppyFleet.CustomViews
{
    public class BorderlessEntry : Entry
    {
        public new event EventHandler Completed;

        public const string ReturnKeyPropertyName = "ReturnKeyType";

        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
            "ReturnKeyPropertyName",
            typeof(ReturnKeyTypes),
            typeof(BorderlessEntry),
            ReturnKeyTypes.Done,
            BindingMode.OneWay
        );

        public ReturnKeyTypes ReturnType
        {
            get { return (ReturnKeyTypes)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public void InvokeCompleted()
        {
            if (this.Completed != null)
                this.Completed.Invoke(this, null);
        }

    }
}
