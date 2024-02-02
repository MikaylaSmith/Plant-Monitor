/*	
 *	Author(s): Mikayla Smith
 *	Filename: HomeViewModel.cs
 *	Date Created: 15 Nov 2022
 *	Modifications:
 *	- Created from Template in Visual Studio (15 Nov 2022)
 *	- Added formatting form ItemDetailViewModel (18 Nov 2022)
*/
using Plant_Monitor.Models;
using Plant_Monitor.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Diagnostics;

namespace Plant_Monitor.ViewModels
{
	public class HomeViewModel : BaseViewModel
	{
		public ObservableCollection<Plant> Plants { get; }
		public Command LoadPlantsCommand { get; }
		public Command AddPlantCommand { get; }
		public Command<Plant> PlantTapped { get; }

		public HomeViewModel()
		{
			Title = "Plant Monitor";
			Plants = new ObservableCollection<Plant>();

			LoadPlantsCommand = new Command(async () => await ExecuteLoadPlantsCommand());

			//PlantTapped = new Command<Plant>(OnPlantSelected);

			//AddPlantCommand = new Command(OnAddPlant);
		}

		async Task ExecuteLoadPlantsCommand()
		{
			IsBusy = true;

			try
			{
				//Plants.Clear();
				var items = await DataStore.GetItemsAsync(true);
				foreach (var item in items)
				{
					//Plants.Add(item);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		//async void OnPlantSelected(Plant plant)
		//{
		//	if (plant == null)
		//		return;

		//	// This will push the ItemDetailPage onto the navigation stack
		//	await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={plant.Plant_ID}");
		//}
	}
}