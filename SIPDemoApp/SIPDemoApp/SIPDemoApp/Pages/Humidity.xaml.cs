using SIPDemoApp.MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SIPDemoApp.Pages
{
    public partial class Humidity : ContentPage
    {
        public Humidity()
        {
            InitializeComponent();
            LabelUmidade.Text = "40 %";
            MessagingCenter.Subscribe<MqttHelperInitializer>(this, "HumidityChanged", c => { Device.BeginInvokeOnMainThread(() => HumidityChanged()); });
        }

        private void HumidityChanged()
        {
            //Alterar label como indicador de humidade. Método é chamado quando existe um publish pra esse tópico
            LabelUmidade.Text = string.Join(" ", new object[] { MqttHelperInitializer.Umidade, "%" });
            LabelUmidade.TextColor = Convert.ToDouble(MqttHelperInitializer.Umidade) > 40.0 ? Color.FromHex("#1e90ff") : Color.FromHex("#606970");
        }
    }
}
