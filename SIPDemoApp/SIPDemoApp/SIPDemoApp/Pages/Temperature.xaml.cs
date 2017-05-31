using SIPDemoApp.MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SIPDemoApp.Pages
{
    public partial class Temperature : ContentPage
    {
        public Temperature()
        {
            InitializeComponent();
            LabelTemperatura.Text = "20 °C";
            MessagingCenter.Subscribe<MqttHelperInitializer>(this, "TemperaturaChanged", c => { Device.BeginInvokeOnMainThread(() => TemperatureChanged()); });
        }

        private void TemperatureChanged()
        {
            //Alterar label como indicador de temperatura. Método é chamado quando existe um publish pra esse tópico
            LabelTemperatura.Text = string.Join(" ", new object[] { MqttHelperInitializer.Temperatura, "°C" });

            var temperatura = Convert.ToDouble(MqttHelperInitializer.Temperatura);
            
            if (temperatura < 10)
                LabelTemperatura.TextColor = Color.FromHex("#52aaff");
            else if (temperatura < 25)
                LabelTemperatura.TextColor = Color.FromHex("#228b22");
            else if (temperatura >= 30)
                LabelTemperatura.TextColor = Color.FromHex("#b30000");

        }
    }
}
