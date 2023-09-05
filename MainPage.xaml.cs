using IdentityModel.OidcClient;
using MauiAuth0App.Auth0;
using MauiAuth0App.Pages;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

namespace MauiAuth0App;

public partial class MainPage : ContentPage
{
/*	int count = 0;*/
    private readonly Auth0Client auth0Client;
    public MainPage(Auth0Client client)
    {

		InitializeComponent();
        auth0Client = client;
        //OnLoadData();
        /*OnLogoutClicked();*/
    }

	/*private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}*/

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginResult = await auth0Client.LoginAsync();

        if (!loginResult.IsError)
        {
            //UsernameLbl.Text = loginResult.User.Identity.Name;
            await SecureStorage.Default.SetAsync("name", loginResult.User.Identity.Name);
            await SecureStorage.Default.SetAsync("img", loginResult.User
              .Claims.FirstOrDefault(c => c.Type == "picture")?.Value);

            //UserPictureImg.Source = loginResult.User
            //  .Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            LoginView.IsVisible = false;
            HomeView.IsVisible = true;
            Shell.SetTabBarIsVisible(this,true);
        }
        else
        {
            await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var logoutResult = await auth0Client.LogoutAsync();

        if (!logoutResult.IsError)
        {
            string Home = await SecureStorage.Default.GetAsync("HomeView");
            string Login = await SecureStorage.Default.GetAsync("LoginView");
            if (!string.IsNullOrEmpty(Home) && !string.IsNullOrEmpty(Login)) {
                var Homeview = JsonSerializer.Deserialize<bool>(Home);
                var Loginview = JsonSerializer.Deserialize<bool>(Login);
                HomeView.IsVisible = Homeview;
                LoginView.IsVisible = Loginview;
            }
            
            Shell.SetTabBarIsVisible(this, false);

        }
        else
        {
            await DisplayAlert("Error", logoutResult.ErrorDescription, "OK");
        }
    }

    //public async void OnLoadData()
    //{

    //    string Home = await SecureStorage.Default.GetAsync("HomeView");
    //    string Login = await SecureStorage.Default.GetAsync("LoginView");


    //    if (!string.IsNullOrEmpty(Home) && !string.IsNullOrEmpty(Login))
    //    {
    //        var Homeview = JsonSerializer.Deserialize<bool>(Home);
    //        var Loginview = JsonSerializer.Deserialize<bool>(Login);
    //        HomeView.IsVisible = Homeview;
    //        LoginView.IsVisible = Loginview;
    //        Shell.SetTabBarIsVisible(this, false);
    //        if (Homeview == false && Loginview == true)
    //        {
    //            await auth0Client.LogoutAsync();
    //        }
    //    }



    //}

    private async void GotoMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SingleProduct());
    }

    private async void OnOurdesignPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WebPage(ourdesignBtn.Text));
    }
    private async void OnPortfolioPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WebPage(portfolioBtn.Text));
    }
    private async void OnblogPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WebPage(blogBtn.Text));
    }

    private async void OnWebPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WebPage(""));
    }
    //private async void blog(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new blog());
    //}
    //private async void portfolio(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new portfolio());
    //}

    //private async void ourdesign(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new ourdesign());
    //}



}

