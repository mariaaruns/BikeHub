using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.Handler
{
    public static class GlobalExceptionHandler
    {
        public static void Handle(Exception? exception, string source)
        {
            if (exception == null)
                return;

            try
            {
                
                Debug.WriteLine($"[{source}] {exception}");

                
                var logPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "crash.log"
                );

                File.AppendAllText(
                    logPath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{source}]\n{exception}\n\n"
                );

                
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    // Avoid crash loop when Shell is not ready
                    if (Shell.Current != null)
                    {
                        await Shell.Current.DisplayAlert(
                            "Unexpected Error",
                            "Something went wrong. Please try again.",
                            "OK"
                        );
                    }
                });
            }
            catch
            {
              
            }
        }
    }
}
