
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Android.Content.Res;
#endif

namespace BikeHub.Mobile.Controls
{
    public class NoUnderlineDatePicker : DatePicker
    {
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

            if (Handler is IDatePickerHandler datePickerHandler)
            {

                if (BackgroundColor == null)
                {

                    datePickerHandler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(Colors.Transparent.ToPlatform());

                }

                else
                {

                    datePickerHandler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(BackgroundColor.ToPlatform());

                }
            }


#endif

        }
    }
}
