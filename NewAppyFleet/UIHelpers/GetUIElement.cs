using Xamarin.Forms;

namespace NewAppyFleet
{
    public class GetUIElement
    {
        public static T GetFirstElement<T>(StackLayout layout) where T:View
        {
            if (layout.Children.Count == 0)
                return default(T);

            T returnElement = default(T);
            for (var n = 0; n < layout.Children.Count; ++n)
            {
                if (layout.Children[n] is T)
                {
                    returnElement = layout.Children[n] as T;
                    continue;
                }
            }

            return returnElement;
        }
    }
}
