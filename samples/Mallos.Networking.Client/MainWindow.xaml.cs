namespace Mallos.Networking.ClientSample
{
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.Logging.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        // DataBindings
        public ObservableCollectionDispatcher<ChatMessage> Messages { get; }

        // Client
        public ServiceProvider ServiceProvider { get; private set; }
        public NetClient NetClient { get; private set; }

        private DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            var serviceCollection = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false, reloadOnChange: false)
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(config);

            serviceCollection.AddLogging(logging =>
            {
                logging.AddConfiguration(config.GetSection("Logging"));
                logging.AddMemory();
            });

            this.ServiceProvider = serviceCollection.BuildServiceProvider();

            this.NetClient = new NetClient(ServiceProvider);

            // DataBindings
            this.Messages = new ObservableCollectionDispatcher<ChatMessage>(Dispatcher, NetClient.Chat.Messages);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var logger = ServiceProvider.GetServices<ILoggerProvider>().First(o => o is MemoryLoggerProvider);
            if (logger is MemoryLoggerProvider memoryLogger)
            {
                var builder = new StringBuilder();
                foreach (var message in memoryLogger.Messages)
                {
                    builder.Append(message);
                }
                this.LogTextBox.Text = builder.ToString();
            }
        }

        private void ChatTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(ChatTextBox.Text))
            {
                this.NetClient.Chat.SendMessage(ChatTextBox.Text);
                this.ChatTextBox.Text = "";
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var netParams = new NetConnectionParameters(
                LoginNameTextBox.Text, LoginPasswordBox.Password, LoginAddressTextBox.Text);

            this.NetClient.Start(netParams)
                .ContinueWith(task =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (task.Result)
                        {
                            this.Overlay.Visibility = Visibility.Hidden;
                        }
                    });
                });
        }
    }
}
