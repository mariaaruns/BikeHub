using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    [QueryProperty(nameof(BrandId), "brandId")]
    public partial class AddEditBrandViewModel : ObservableObject
    {

        [ObservableProperty]
        private string brandId;
        public string PageTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(BrandId) && BrandId != "0")
                    return "Edit Brand";

                return "Add Brand";
            }
        }

        private void RaisePageTitleChanged() => OnPropertyChanged(nameof(PageTitle));


        partial void OnBrandIdChanged(string value) => RaisePageTitleChanged();


    }
}
