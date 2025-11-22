
#if ANDROID
using Android.Content.Res;
#endif
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.Controls
{
    internal class NoUnderlineEntry:Entry
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

            if (Handler is IEntryHandler entryHandler)
            {

                if (BackgroundColor == null)
                {

                    entryHandler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(Colors.Transparent.ToPlatform());

                }

                else
                {

                    entryHandler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(BackgroundColor.ToPlatform());

                }
            }


#endif

        }
    }
}
