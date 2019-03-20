namespace Mallos.Networking.ClientSample
{
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        // Client
        public NetClient NetClient { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            var serviceProvider = Services.Create();

            this.NetClient = new NetClient(serviceProvider);
            this.NetClient.Start(new NetConnectionParameters("eric", "abc123", "localhost"));
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
    }
}
