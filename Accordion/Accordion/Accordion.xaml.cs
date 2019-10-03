using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Accordion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Accordion : ContentView
    {
        public static readonly BindableProperty IndicatorViewProperty = BindableProperty.Create(nameof(IndicatorView), typeof(View), typeof(Accordion), default(View));
        public View IndicatorView
        {
            get => (View)GetValue(IndicatorViewProperty);
            set => SetValue(IndicatorViewProperty, value);
        }

        public static readonly BindableProperty ContentViewProperty = BindableProperty.Create(nameof(AccordionContentView), typeof(View), typeof(Accordion), default(View));
        public View AccordionContentView
        {
            get => (View)GetValue(ContentViewProperty);
            set => SetValue(ContentViewProperty, value);
        }

        public static readonly BindableProperty TitleBindableProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Accordion), default(string));
        public string Title
        {
            get => (string)GetValue(TitleBindableProperty);
            set => SetValue(TitleBindableProperty, value);
        }

        public static readonly BindableProperty IsOpenBindablePropertyProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(Accordion), false, propertyChanged: IsOpenChanged);
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenBindablePropertyProperty); }
            set { SetValue(IsOpenBindablePropertyProperty, value); }
        }

        public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(Accordion), Color.Transparent);
        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        private static void IsOpenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            bool isOpen;

            if (bindable != null && newValue != null)
            {
                var control = (Accordion)bindable;
                isOpen = (bool)newValue;

                if (control.IsOpen == false)
                {
                    control.Close();
                }
                else
                {
                    control.Open();
                }
            }
        }

        public uint AnimationDuration { get; set; }

        public Accordion()
        {
            InitializeComponent();
            Close();
            AnimationDuration = 250;
            IsOpen = false;
        }

        async void Close()
        {
            await Task.WhenAll(
                _accContent.TranslateTo(0, -10, AnimationDuration),
                _indicatorContainer.RotateTo(-180, AnimationDuration),
                _accContent.FadeTo(0, 50)
                );
            _accContent.IsVisible = false;
        }

        async void Open()
        {
            _accContent.IsVisible = true;
            await Task.WhenAll(
                _accContent.TranslateTo(0, 10, AnimationDuration),
                _indicatorContainer.RotateTo(0, AnimationDuration),
                _accContent.FadeTo(30, 50, Easing.SinIn)
            );
        }

        private void TitleTapped(object sender, EventArgs e)
        {
            IsOpen = !IsOpen;
        }
    }
}