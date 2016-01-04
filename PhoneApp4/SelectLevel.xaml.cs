using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneApp4
{
    public partial class SelectLevel : PhoneApplicationPage
    {
        private List<string> lstLocalLevels;
        public SelectLevel()
        {

            InitializeComponent();
            //TODO: Fill list from local levels!
            lstLocalLevels = new List<string>();
            lstLocalLevels.Add("Level yksi");
            lstLocalLevels.Add("hieno kenttä");
            acBoxLocalLevels.ItemsSource = lstLocalLevels;
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Select";
            NavigationService.Navigate(new Uri("/HelpPage.xaml?msg=" + msg, UriKind.Relative));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void acBoxLocalLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (acBoxLocalLevels.SelectedItem != null)
            {
                acBoxLocalLevels.Text = acBoxLocalLevels.SelectedItem.ToString();
                // TODO: Load a picture of the level above
            }
        }

        private void acBoxSearchCreators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void acBoxSearchLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_GotFocus(object sender, RoutedEventArgs e)
        {
            //TODO: Show more info about the clicked level
        }

    }
}