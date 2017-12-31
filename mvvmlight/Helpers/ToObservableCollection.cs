using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mvvmframework.Helpers
{
    public static class ToObservable
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> collection)
        {
            return new ObservableCollection<T>(collection as IEnumerable<T>);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
