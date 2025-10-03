using Bikehub.Client.Models;
using Bikehub.Client.PageModels;

namespace Bikehub.Client.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}