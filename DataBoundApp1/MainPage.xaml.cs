using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DataBoundApp1.Resources;
using DataBoundApp1.ViewModels;
using System.Collections.ObjectModel;

namespace DataBoundApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                await App.ViewModel.LoadData();
            }
        }

        // Handle selection changed on LongListSelector
        private async void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            //NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));
            var uri = new Uri((MainLongListSelector.SelectedItem as ItemViewModel).Url);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        private async void ApplicationBarRefreshButton_Click(object sender, EventArgs e)
        {
            await App.ViewModel.LoadData();
        }

        private void ApplicationBarMenuDeleteOlderThanWeek_Click(object sender, EventArgs e)
        {
            List<ItemViewModel> itemList = App.ViewModel.Items.Where(i => i.Date < (DateTime.Now.AddDays(-7))).ToList();
            ObservableCollection<ItemViewModel> newItems = new ObservableCollection<ItemViewModel>(itemList);
        }

        private void ApplicationBarMenuDeleteAll_Click(object sender, EventArgs e)
        {
            App.ViewModel.Items.Clear();
        }




        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}