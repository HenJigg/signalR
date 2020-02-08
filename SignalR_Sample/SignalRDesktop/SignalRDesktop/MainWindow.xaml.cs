using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalRDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44345/chatHub")
                .Build();

            connection.On<string>("online", (msg) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    txtInfo.Text += msg + "\r\n";
                });
            });

            connection.On<string, string>("ReceiveMessage", (user, msg) =>
             {
                 this.Dispatcher.Invoke(() =>
                 {
                     txtMsg.Text += $"{user}:{msg} \r\n";
                 });
             });

            connection.StartAsync();
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            string title = $"监工{new Random().Next(1, 99999)}号";
            Title = title;

            connection.InvokeAsync("Login", title);
            btnSend.IsEnabled = true;
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            connection.InvokeAsync("SignOut", Title);
            connection.StopAsync();
            this.Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtSend.Text == "") return;

            connection.InvokeAsync("SendMessage", Title, txtSend.Text);
        }
    }
}
