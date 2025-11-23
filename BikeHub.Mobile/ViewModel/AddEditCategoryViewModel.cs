using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{

    [QueryProperty(nameof(CategoryId), "categoryId")]
    public partial class AddEditCategoryViewModel:ObservableObject
    {
        [ObservableProperty]
        private string categoryId;
        public string PageTitle
        {
            get
            {

                if (!string.IsNullOrEmpty(CategoryId) && CategoryId != "0")
                    return "Edit Category";

                return "Add Category";
            }
        }

        private void RaisePageTitleChanged() => OnPropertyChanged(nameof(PageTitle));

        partial void OnCategoryIdChanged(string value) => RaisePageTitleChanged();

    }
}
