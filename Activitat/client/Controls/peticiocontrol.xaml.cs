using JocVaixell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Lógica de interacción para peticiocontrol.xaml
    /// </summary>
    public partial class peticiocontrol : UserControl
    {
        private TableAlbertcontrol tableControl;
        public peticiocontrol()
        {
            InitializeComponent();
            this.tableControl = tableControl;

        }
        private void env_peticio(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(IP_servidor.Text) && !string.IsNullOrEmpty(Port.Text))
            {
                IPAddress ipAddress;

                if (!IPAddress.TryParse(IP_servidor.Text, out ipAddress))
                {
                    MessageBox.Show("Dirección IP no válida");
                    return;
                }
                int port;
                if (!int.TryParse(Port.Text, out port))
                {
                    MessageBox.Show("Puerto no válido");
                    return;
                }
                string row = Fila.Text;
                string column = Columna.Text;
                string message = $"{row},{column}";

                try
                {
                    using (TcpClient client = new TcpClient(IP_servidor.Text, port))
                    {
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] data = Encoding.ASCII.GetBytes(message);
                            stream.Write(data, 0, data.Length);

                            byte[] buffer = new byte[1024];
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
                            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            response = response.TrimEnd().ToLower();

                            if (!string.IsNullOrEmpty(response))
                            {
                                String resultat;
                                if (response == "agua")
                                {
                                    resultat = "has fallat";
                                    Indicador.Text = resultat;
                                }
                                else if (response == "vaixell")
                                {
                                    resultat = "has tocat";
                                    Indicador.Text = resultat;
                                }
                                tableControl.revealcell(int.Parse(row), int.Parse(column), response);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
                }
            }
            else
            { 
                MessageBox.Show("Por favor ingrese la dirección IP del servidor y el puerto.");
            }
        }
    }
}
