using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.Models
{
    public partial class OrderItemsCartDto : ObservableObject
    {

        [ObservableProperty]
        private int _quantity;


        [ObservableProperty]
        private int _stock;

        [ObservableProperty]
        private decimal _price;


        [ObservableProperty]
        private decimal _discount;

        [ObservableProperty]
        private int _productId;

        [ObservableProperty]
        private string _productName = string.Empty;


        partial void OnQuantityChanged(int oldValue, int newValue)
        {
            // delta is positive when quantity increased, negative when decreased
            var delta = newValue - oldValue;

            if (newValue < 1)
            {
                RequestRemove?.Invoke(this);
                return;
            }

            if (delta > 0)
            {
                // Need to reserve 'delta' items from available stock.
                // If not enough available stock, revert change without re-entering this method.
                if (delta > _stock)
                {
                    // revert directly to backing field to avoid recursion into this partial method
                    _quantity = oldValue;
                    OnPropertyChanged(nameof(Quantity));
                    return;
                }

                _stock -= delta;
                OnPropertyChanged(nameof(Stock));
            }
            else if (delta < 0)
            {
                // Released (-delta) items back to available stock
                _stock += -delta;
                OnPropertyChanged(nameof(Stock));
            }

            NotifyQuantityChanged();
        }


        public event Action? QuantityChanged;

        public void NotifyQuantityChanged() 
        {
            QuantityChanged?.Invoke();
        }

        public event Action<OrderItemsCartDto>? RequestRemove;

        [RelayCommand]
        void IncreaseQty() => Quantity++;

        [RelayCommand]
        void DecreaseQty() => Quantity--;

    }
}
