using Pokedex.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pokedex.Controls
{
    public class Paging : Grid
    {
        public static readonly BindableProperty PageSizeProperty = BindableProperty.Create(nameof(PageSize), typeof(int), typeof(Paging), 20, BindingMode.OneWay);
        public static readonly BindableProperty CurrentPageProperty = BindableProperty.Create(nameof(CurrentPage), typeof(int), typeof(Paging), 1, BindingMode.TwoWay, propertyChanged: CurrentPageChanged);
        public static readonly BindableProperty TotalItemsProperty = BindableProperty.Create(nameof(TotalItems), typeof(int), typeof(Paging), 0, BindingMode.TwoWay, propertyChanged: TotalPagesChanged);
        public static readonly BindableProperty PageChangedCommandProperty = BindableProperty.Create(nameof(PageChangedCommand), typeof(ICommand), typeof(Paging), null, BindingMode.TwoWay);
        

        public static readonly BindableProperty PageBackgroundColorProperty = BindableProperty.Create(nameof(PageBackgroundColor), typeof(Color), typeof(Paging), Color.White, BindingMode.OneWay);
        public static readonly BindableProperty SelectedPageBackgroundColorProperty = BindableProperty.Create(nameof(SelectedPageBackgroundColor), typeof(Color), typeof(Paging), Color.LightGray, BindingMode.OneWay);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Paging), Color.Black, BindingMode.OneWay);


        private readonly StackLayout _paging = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(0, 10), HorizontalOptions = LayoutOptions.CenterAndExpand };



        public int CurrentPage
        {
            get { return (int)this.GetValue(CurrentPageProperty); }
            set { this.SetValue(CurrentPageProperty, value); }
        }

        public int PageSize
        {
            get { return (int)this.GetValue(PageSizeProperty); }
            set { this.SetValue(PageSizeProperty, value); }
        }

        public int TotalItems
        {
            get { return (int)this.GetValue(TotalItemsProperty); }
            set { this.SetValue(TotalItemsProperty, value); }
        }



        public Color PageBackgroundColor
        {
            get { return (Color)this.GetValue(PageBackgroundColorProperty); }
            set { this.SetValue(PageBackgroundColorProperty, value); }
        }

        public Color SelectedPageBackgroundColor
        {
            get { return (Color)this.GetValue(SelectedPageBackgroundColorProperty); }
            set { this.SetValue(SelectedPageBackgroundColorProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }


        public ICommand PageChangedCommand
        {
            get { return (ICommand)this.GetValue(PageChangedCommandProperty); }
            set { this.SetValue(PageChangedCommandProperty, value); }
        }


        public Paging()
        {
            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            this.ColumnDefinitions.Add(new ColumnDefinition() {  });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            Grid.SetColumn(_paging, 1);

            Children.Add(_paging);

        }

        private void Clear()
        {
            _paging.Children.Clear();
        }

        public void Init()
        {

            if (TotalItems > 0 && PageSize > 0)
            {
                var totalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
                var pagesToShow = (int)Math.Min(5, totalPages);

                var lastPage = pagesToShow;

                if (CurrentPage > 4)
                {
                    lastPage = (int)Math.Min(totalPages, CurrentPage + 2);
                }

                _paging.Children.Add(BuildPageElement("<<", 1, CurrentPage > 1));
                //_paging.Children.Add(BuildPageElement("<", CurrentPage - 1, CurrentPage > 1));

                for (int page = lastPage - pagesToShow + 1; page <= lastPage; page++)
                {
                    _paging.Children.Add(BuildPageElement(page.ToString(), page, true, page == CurrentPage));
                }

                //_paging.Children.Add(BuildPageElement(">", CurrentPage + 1, CurrentPage < totalPages));
                _paging.Children.Add(BuildPageElement(">>", totalPages, CurrentPage < totalPages));
                
                //_paging.Children.Add
            }
        }

        private Frame BuildPageElement(string text, int page, bool enabled, bool selected = false)
        {
            var button = new Frame()
            {
                IsEnabled = enabled,
                BorderColor = selected ? SelectedPageBackgroundColor : PageBackgroundColor,
                BackgroundColor = selected ? SelectedPageBackgroundColor : PageBackgroundColor,
                CornerRadius = 6,
                Padding = new Thickness(12, 4),
                Content = new Label() { TextColor = TextColor, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), Text = text },
            };
            
            button.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command<int>(ChangePage), CommandParameter = page });

            return button;
        }

        private void ChangePage(int page)
        {
            if (PageChangedCommand != null && page != CurrentPage && PageChangedCommand.CanExecute(page))
                PageChangedCommand.Execute(page);
        }

        private static void CurrentPageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var paging = bindable as Paging;
            if (paging != null)
            {
                paging.Clear();
                paging.Init();
            }
        }

        private static void TotalPagesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var paging = bindable as Paging;

            if (paging != null)
            {
                paging.Clear();
                paging.Init();
            }

        }

    }

}
