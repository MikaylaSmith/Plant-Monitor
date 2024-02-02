using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plant_Monitor.Models;
using Plant_Monitor.ViewModels;
using Plant_Monitor.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plant_Monitor.Views
{
    public partial class Home : ContentPage
    {
        ItemsViewModel _viewModel;

        public Home()
        {
			InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private async void OnButtonClick(object sender, EventArgs e)
        {

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Navigation.PushAsync(new NewItemPage());
        }
    }
}