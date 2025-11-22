using BikeHub.Mobile.Controls;
using BikeHub.Mobile.ViewModel;
using System.Net.Mime;

namespace BikeHub.Mobile.Pages;

public class AddEditProductPage : ContentPage
{
    public AddEditProductPage(ProductsViewModel viewModel)
    {
        
        this.BindingContext = viewModel;
        this.SetBinding(TitleProperty, new Binding(nameof(viewModel.PageTitle)));
        var glossyOrange = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops = new GradientStopCollection
    {
        new GradientStop(Color.FromRgb(255, 200, 100), 0.0f), // bright top
        new GradientStop(Color.FromRgb(255, 140, 0), 0.5f),   // mid tone
        new GradientStop(Color.FromRgb(200, 80, 0), 1.0f)     // darker bottom
    }
        };

        var imageGrid = new Grid
        {
            HeightRequest = 150,
            WidthRequest = 150
        };

        // Product Image
        var productImage = new Image
        {
            Aspect = Aspect.AspectFill,
            HeightRequest = 150,
            WidthRequest = 150
        };
        productImage.SetBinding(Image.SourceProperty, nameof(viewModel.ProductImage));

        // Upload Icon Button (overlay)
        var uploadIconBtn = new ImageButton
        {
            Source = "pencil.png", // your camera/upload icon image
            //BackgroundColor = Colors.LightGray,
            Background = glossyOrange,
            BorderWidth =1,
            BorderColor = Color.FromRgb(180, 60, 0),
            CornerRadius = 0,
            WidthRequest = 150,
            HeightRequest = 10,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
        };
        uploadIconBtn.SetBinding(ImageButton.CommandProperty, nameof(viewModel.UploadPhotoCommand));

        // Add both to grid (image first, then button overlay)
        imageGrid.Children.Add(productImage);
        imageGrid.Children.Add(uploadIconBtn);

        // Frame for circular border
        var profileFrame = new Frame
        {
            HeightRequest = 150,
            WidthRequest = 150,
            CornerRadius = 75,
            BorderColor = Colors.Green,
            HasShadow = false,
            Padding = 0,
            HorizontalOptions = LayoutOptions.Center,
            Content = imageGrid
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 15,
                Margin = new Thickness(10, 5),
                Children =
            {
                profileFrame,
                

                CreateLabel("Product Name"),
                CreateEntryFrame("Product Name", nameof(viewModel.ProductName)),

                CreateLabel("Price"),
                CreateEntryFrame("Price", nameof(viewModel.Price)),

                CreateLabel("Quantity"),
                CreateEntryFrame("Quantity", nameof(viewModel.Quantity)),


                
                CreateLabel("Brand"),
                CreatePickerFrame("Select Brand", nameof(viewModel.SelectedBrand), viewModel.BrandList),
                
                CreateLabel("Category"),
                CreatePickerFrame("Select Category", nameof(viewModel.SelectedCategory), viewModel.CategoryList)
            }
            }
        };




    }

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

}