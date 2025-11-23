using BikeHub.Mobile.Controls;
using BikeHub.Mobile.ViewModel;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Views;

namespace BikeHub.Mobile.Pages;

public partial class ProductsPage : ContentPage
{
	private ProductsViewModel _vm;
    private bool _ProductsLoaded;
    public ProductsPage(ProductsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _vm=viewModel;
    }
   

	protected async override void OnAppearing()
	{
		base.OnAppearing();
        if (_ProductsLoaded) return;
        _ProductsLoaded = true;
        if (_vm?.LoadProductCommand is not null && _vm.LoadProductCommand.CanExecute(null))
        {
            await _vm.LoadProductCommand.ExecuteAsync(null);
        }

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
       
        if (_vm?.LoadProductCommand is not null && _vm.LoadProductCommand.IsRunning)
        {
            _vm.LoadProductCommand.Cancel();
        }
    }


}