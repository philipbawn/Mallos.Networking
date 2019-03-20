namespace Mallos.Networking.ServerSample
{
    using Mallos.Networking.Chat.Abstractions;
    using Mallos.Networking.Logging.Memory;
    using Mallos.Networking.User;
    using Mallos.Networking.User.Abstractions;
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
        public ObservableCollectionDispatcher<IdentityUser> ConnectedUsers { get; }
        public ObservableCollectionDispatcher<ChatMessage> Messages { get; }

        // Server
        public ServiceProvider ServiceProvider { get; private set; }
        public NetServer<IdentityUser> NetServer { get; private set; }

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

            var userStorage = new InMemoryUserStorage<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStorage);

            userManager.CreateAsync(new IdentityUser("Eric"), "abc123").Wait();

            this.NetServer = new NetServer<IdentityUser>(ServiceProvider, userManager);
            this.NetServer.Start();

            // DataBindings
            this.ConnectedUsers = new ObservableCollectionDispatcher<IdentityUser>(Dispatcher, NetServer.ConnectedUsers);
            this.Messages = new ObservableCollectionDispatcher<ChatMessage>(Dispatcher, NetServer.Chat.Messages);

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
                this.NetServer.Chat.SendMessage(ChatTextBox.Text);
                this.ChatTextBox.Text = "";
            }
        }
    }
}
