using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditOrders : ContentPage
{
	private readonly AddOrderViewModel _vm;
	private  bool _isInitialized;
    public AddEditOrders(AddOrderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext=_vm=viewModel;
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_isInitialized) return;

		if (_vm.LoadBrandsAndCategoriesCommand.CanExecute(null) && _vm.LoadBrandsAndCategoriesCommand is not null) 
		{

			await _vm.LoadBrandsAndCategoriesCommand.ExecuteAsync(null);
            
			_isInitialized = true;
		}
    }
}