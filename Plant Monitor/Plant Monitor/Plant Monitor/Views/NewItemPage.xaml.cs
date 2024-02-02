using Plant_Monitor.Models;
using Plant_Monitor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.Collections.ObjectModel;

namespace Plant_Monitor.Views
{
	public partial class NewItemPage : ContentPage
	{
		public Plant Plant { get; set; }
		public NewItemPage()
		{
			//Initializes the page
			InitializeComponent();
			//Binds the context to a new item view model
			BindingContext = new NewItemViewModel();
		}
	}

}
