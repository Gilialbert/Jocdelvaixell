using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JocVaixell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            table.BorderThickness = new Thickness(1);
            table.BorderBrush = Brushes.Black;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(row.Text) && !string.IsNullOrEmpty(Column.Text))
            {
                int rows = int.Parse(this.row.Text);
                int columns = int.Parse(this.Column.Text);

                table.Rows = rows;
                table.Colums = columns;
                table.DrawGrid();

                this.row.Text = "";
                this.Column.Text = "";
            }
            else
            {
                MessageBox.Show("Introdueix un numero de filas i columnas");
            }
        }

        private void Start_Server_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Port.Text))
            {
                int port = int.Parse(this.Port.Text);
                Thread serverThread = new Thread(() => ServerLogic(port));
                serverThread.IsBackground = true;
                serverThread.Start();
                this.StartServer.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Introdueix un port");
            }
        }
        private void ServerLogic(int port)
        {
            if (port >= 0)
            {
                try
                {
                    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    IPAddress ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings["IpServidor"]);
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
                    serverSocket.Bind(localEndPoint);
                    serverSocket.Listen(10);

                    while (true)
                    {
                        Socket clientSocket = serverSocket.Accept();

                        Thread clientThread = new Thread(() => HandleClient(clientSocket));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al iniciar el servidor: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Port no vàlid");
            }
        }
        private void HandleClient(Socket clientSocket)
        {
            if (clientSocket != null)
            {

                byte[] buffer = new byte[1024];
                StringBuilder receivedMessage = new StringBuilder();

                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        Log.Items.Add("Cliente conectado.");
                    });
                    int bytesRead = clientSocket.Receive(buffer);
                    receivedMessage.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

                    Dispatcher.Invoke(() =>
                    {
                        Log.Items.Add(receivedMessage.ToString());
                    });

                    string[] messageParts = receivedMessage.ToString().Split(',');
                    int row = int.Parse(messageParts[0]);
                    int column = int.Parse(messageParts[1]);

                    string result = buscarSeparat(row, column);
                    if(result == "Vaixell") {
                        table.shiphitted(row, column);
                    }
                    byte[] responseData = Encoding.ASCII.GetBytes(result);
                    clientSocket.Send(responseData);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al recibir o enviar mensaje: {ex.Message}");
                }
            }
        }
        private string buscarSeparat(int r, int c)
        {

            Func<string> buscarDelegate = () =>
            {
                var valor = table.buscar(r, c);
                Log.Items.Add($"{valor}");
                return valor;

            };
            
            return Dispatcher.Invoke(buscarDelegate);
        }
    }
}