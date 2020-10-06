using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuickQuotes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            ((MainPage)MainPage).quoteCollection.SaveDataToFile();
        }

        protected override void OnResume()
        {
        }
    }
}
