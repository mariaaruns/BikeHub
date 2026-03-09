using Android.App;
using Android.Runtime;
using BikeHub.Mobile.Handler;

namespace BikeHub.Mobile
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {

        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void OnCreate()
        {
            base.OnCreate();

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                GlobalExceptionHandler.Handle(e.Exception, "Android");
                e.Handled = true;
            };
        }
    }
}
