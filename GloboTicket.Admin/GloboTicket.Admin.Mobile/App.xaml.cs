﻿using GloboTicket.Admin.Mobile.Views;

namespace GloboTicket.Admin.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
