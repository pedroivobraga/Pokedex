using MvvmHelpers;

namespace Pokedex.ViewModels
{
    public class SelectableItem<T> : ObservableObject
    {
        public T Item { get; set; }


        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public SelectableItem(T item) : this(item, false)
        {
        }

        public SelectableItem(T item, bool selected)
        {
            Item = item;
            Selected = selected;

        }
    }
}
