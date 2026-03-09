using BikeHub.Mobile.Controls;
using BikeHub.Mobile.ViewModel;
using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
using System.Net.Mime;
using System.Collections.ObjectModel;
using BikeHub.Shared.Dto.Response;
namespace BikeHub.Mobile.Pages;

public class AddEditProductPage : ContentPage
{
    private readonly AddEditProductViewModel _vm;

    public AddEditProductPage(AddEditProductViewModel viewModel)
    {
        _vm = viewModel;
        BindingContext = _vm;

        this.Bind(TitleProperty, nameof(_vm.PageTitle));

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 15,
                Margin = new Thickness(10, 5),

                Children =
                    {
                            BuildProfileImage(),

                            CreateLabel("Product Name"),
                            CreateEntryFrame("Product Name", nameof(_vm.ProductName)),

                            CreateLabel("Price"),
                            CreateEntryFrame("Price", nameof(_vm.Price)),

                            CreateLabel("Quantity"),
                            CreateEntryFrame("Quantity", nameof(_vm.Quantity)),

                            CreateLabel("Model Year"),
                            CreateModelYearPickerFrame("ModelYear",nameof(_vm.SelectedModelYear),_vm.ModelyearList),

                            CreateLabel("Brand"),
                            CreatePickerFrame("Select Brand", nameof(_vm.SelectedBrand), _vm.BrandList),

                            CreateLabel("Category"),
                            CreatePickerFrame("Select Category", nameof(_vm.SelectedCategory), _vm.CategoryList),

                            new Button{
                            Text="SAVE",BackgroundColor=Colors.Orange,
                            Command=_vm.SaveProductCommand
                            }


                    }
            }
        };
            
    }


    protected  override  async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.LoadDropDownCommand is not null && 
            _vm.LoadDropDownCommand.CanExecute(null)) 
        { 
                await _vm.LoadDropDownCommand.ExecuteAsync(null);
        }
    }

    private View BuildProfileImage()
    {
        var glossyOrange = new LinearGradientBrush(
            new GradientStopCollection
            {
            new( Color.FromRgb(255, 200, 100), 0f ),
            new( Color.FromRgb(255, 140, 0),   0.5f ),
            new( Color.FromRgb(200, 80, 0),    1f )
            },
            new Point(0, 0),
            new Point(0, 1)
        );

        return new Frame
        {
            HeightRequest = 150,
            WidthRequest = 150,
            CornerRadius = 75,
            BorderColor = Colors.Green,
            HasShadow = false,
            Padding = 0,
            HorizontalOptions = LayoutOptions.Center,

            Content = new Grid
            {
                Children =
            {
                new Image
                {
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 150,
                    WidthRequest = 150
                }
              .Bind(Image.SourceProperty, nameof(_vm.ProductImage)),

                new ImageButton
                {
                    Source = "pencil.png",
                    Background = glossyOrange,
                    BorderWidth = 1,
                    BorderColor = Color.FromRgb(180, 60, 0),
                    WidthRequest = 150,
                    HeightRequest = 30,
                    VerticalOptions = LayoutOptions.End
                }
                .BindCommand(nameof(_vm.UploadPhotoCommand))
            }
            }
        };
    }
    private View CreateEntryFrame(string placeholder, string bindingProperty)
    {
        return new Frame
        {
            BorderColor = Colors.LightGray,
            CornerRadius = 10,
            Padding = new Thickness(10, 0),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.Fill,

            Content = new NoUnderlineEntry
            {
                Placeholder = placeholder
            }
            .Bind(Entry.TextProperty, bindingProperty)
        };
    }

    private View CreatePickerFrame(string title, string bindingProperty, ObservableCollection<DropdownDto> itemsSource)
    {
        return new Frame
        {
            BorderColor = Colors.LightGray,
            CornerRadius = 10,
            Padding = new Thickness(10, 0),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.Fill,

            Content = new Grid
            {
                ColumnDefinitions = Columns.Define(
                   Star,
                    Auto
                ),

                Children =
            {
                new NoUnderlinePicker
                {
                    Title = title,
                    ItemsSource = (itemsSource ?? new ObservableCollection<DropdownDto>()),
                    ItemDisplayBinding = new Binding("Text"),
                    
                }
                .Bind(Picker.SelectedItemProperty, bindingProperty)
                .Column(0),

                new Image
                {
                    Source = "angle_down.png",
                    HeightRequest = 24,
                    WidthRequest = 24,
                    VerticalOptions = LayoutOptions.Center
                }
                .Column(1)
            }
            }
        };
    }

    private View CreateModelYearPickerFrame(string title, string bindingProperty, ObservableCollection<int> itemsSource)
    {
        return new Frame
        {
            BorderColor = Colors.LightGray,
            CornerRadius = 10,
            Padding = new Thickness(10, 0),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.Fill,
            Content = new Grid
            {
                ColumnDefinitions = Columns.Define(
                   Star,
                    Auto
                ),
                Children =
            {
                new NoUnderlinePicker
                {
                    Title = title,
                    ItemsSource = (itemsSource ?? new ObservableCollection<int>()),
                    
                }
                .Bind(Picker.SelectedItemProperty, bindingProperty)
                .Column(0),
                new Image
                {
                    Source = "angle_down.png",
                    HeightRequest = 24,
                    WidthRequest = 24,
                    VerticalOptions = LayoutOptions.Center
                }
                .Column(1)
            }
            }
        };
    }
    private Label CreateLabel(string text, double fontSize = 18, bool bold = true)
    {
        return new Label
        {
            Text = text,
            FontSize = fontSize,
            FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None
        };
    }
/*
    private Frame CreateEntryFrame(string placeholder, string bindingProperty)
    {
        var entry = new NoUnderlineEntry
        {
            Placeholder = placeholder,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        entry.SetBinding(Entry.TextProperty, bindingProperty);

        return new Frame
        {
            Content = entry,
            BorderColor = Colors.LightGray,
            CornerRadius = 10,
            Padding = new Thickness(10,0),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
    }

    private Frame CreatePickerFrame(string title, string bindingProperty, IList<string> itemsSource)
    {
        var picker = new NoUnderlinePicker
        {
            Title = title,
            ItemsSource= (itemsSource ?? new List<string>()).ToList(),
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Center
        };
        picker.SetBinding(Picker.SelectedItemProperty, bindingProperty);

        var arrowImage = new Image
        {
            Source = "angle_down.png",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            HeightRequest = 24,
            WidthRequest = 24
        };

        var grid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Auto)
        }
        };
        grid.Children.Add(picker);
        Grid.SetColumn(picker, 0);
        grid.Children.Add(arrowImage);
        Grid.SetColumn(arrowImage, 1);

        return new Frame
        {
            Content = grid,
            BorderColor = Colors.LightGray,
            CornerRadius = 10,
            Padding = new Thickness(10,0),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
    }

    private Label CreateLabel(string text, string bindingProperty = null, double fontSize = 18, bool bold = true)
    {
        var label = new Label
        {
            FontSize = fontSize,
            FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None
        };
        if (bindingProperty != null)
            label.SetBinding(Label.TextProperty, bindingProperty);
        else
            label.Text = text;

        return label;
    }
*/
}