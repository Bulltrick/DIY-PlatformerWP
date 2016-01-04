using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp4.Resources;

namespace PhoneApp4
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            txtCreator.Text = "Start Button Pressed";
            //TODO: send Level ID? to load the correct level
            string msg = txtBoxLevel.Text;
            NavigationService.Navigate(new Uri("/PlayLevel.xaml?levelname=" + msg, UriKind.Relative));
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            txtCreator.Text = "Help Button";
            //string msg = this.Title;
            string msg = "Main Menu";
            NavigationService.Navigate(new Uri("/HelpPage.xaml?msg=" + msg, UriKind.Relative));
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            txtCreator.Text = "Create Button";
            NavigationService.Navigate(new Uri("/CreateLevel.xaml", UriKind.Relative));
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            txtCreator.Text = "Select Button";
            NavigationService.Navigate(new Uri("/SelectLevel.xaml", UriKind.Relative));
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