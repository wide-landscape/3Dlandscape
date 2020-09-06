using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Urho;
using Urho.Forms;
using System.Diagnostics;

namespace _3Dlandscape
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
			Debug.WriteLine("***** App.App()");

			InitializeComponent();

			//MainPage = new MainPage();     //  This would use the MainPage.xaml, not needed here
			MainPage = new UrhoPage();

		}

        protected override void OnStart()
        {
			
		}

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }


	public class UrhoPage : ContentPage
	{
        readonly UrhoSurface urhoSurface;

		public UrhoPage()
		{

			Debug.WriteLine("***** UrhoPage.UrhoPage()");


			urhoSurface = new UrhoSurface();
			urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
			Title = " UrhoSharp + Xamarin.Forms";
			Content = new StackLayout
			{
				Padding = new Thickness(0, 0, 0, 0),
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = { urhoSurface }
			};
		}
		protected override void OnAppearing()
		{
			StartUrhoApp();
		}
		async void StartUrhoApp()
		{
			Debug.WriteLine("***** UrhoPage.StartUrhoApp()");
			await urhoSurface.Show<WorldView>(new ApplicationOptions(assetsFolder: "Data") { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });
		}

	}
}

