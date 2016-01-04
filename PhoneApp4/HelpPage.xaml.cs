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
    public partial class HelpPage : PhoneApplicationPage
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = "";
            IDictionary<string, string> parameters = NavigationContext.QueryString;
            if (parameters.ContainsKey("msg")) msg = parameters["msg"];
            string text = "";
            switch (msg)
            {
                case "Main Menu": text = "This is the Main Menu -page for DIY Platformer! Click on 'Create Level' -button to create a unique level, 'Select Level' to select a level to play on or 'Play Level' to start playing on selected level";
                    break;
                case "Create: Level": text = "Create: Level";
                    break;
                case "Select Level": text = "Select Level -page";
                    break;
                case "Create Level": text = "Create Level -page";
                    break;
                default: text = "How did you get here? :O";
                    break;
            }
            txtHelpInfo.Text = text;
        }
    }
}