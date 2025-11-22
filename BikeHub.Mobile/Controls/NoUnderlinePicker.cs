using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections; // <- Add this

#if ANDROID
using Android.Content.Res;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#endif
namespace BikeHub.Mobile.Controls
{
    public class NoUnderlinePicker : Picker
    { // MVVM: SelectedItem bindable property
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(NoUnderlinePicker),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedItemChanged);

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (NoUnderlinePicker)bindable;
            if (newValue != null && picker.ItemsSource != null)
            {
                var list = picker.ItemsSource as IList;
                if (list != null)
                {
                    int index = list.IndexOf(newValue);
                    if (index >= 0)
                        picker.SelectedIndex = index;
                }
            }
        }

        // MVVM: SelectionChangedCommand bindable property
        public static readonly BindableProperty SelectionChangedCommandProperty =
            BindableProperty.Create(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(NoUnderlinePicker));

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }

        public NoUnderlinePicker()
        {
            SelectedIndexChanged += NoUnderlinePicker_SelectedIndexChanged;
        }

        private void NoUnderlinePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (SelectedIndex >= 0 && ItemsSource != null)
            {
                var list = ItemsSource as IList; // Non-generic IList
                if (list != null && SelectedIndex < list.Count)
                {
                    SelectedItem = list[SelectedIndex];
                    SelectionChangedCommand?.Execute(SelectedItem);
                }
            }
        }

        // --- Existing underline removal code ---
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            RemoveUnderline();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(BackgroundColor))
            {
                RemoveUnderline();
            }
        }

        private void RemoveUnderline()
        {
#if ANDROID
            if (Handler is IPickerHandler pickerHandler)
            {
                if (BackgroundColor == null)
                    pickerHandler.PlatformView.BackgroundTintList =
                        ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
                else
                    pickerHandler.PlatformView.BackgroundTintList =
                        ColorStateList.ValueOf(BackgroundColor.ToPlatform());
            }
#endif
        }
    }
}

