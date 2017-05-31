using SIPDemoApp.MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SIPDemoApp.Pages
{
    public partial class Control : ContentPage
    {
        public Control()
        {
            InitializeComponent();
            LampImage.IsVisible = false; //Inicializa Falso

            var gesture = new TapGestureRecognizer(async (c) => await LampImage_Clicked());

            LampImage.GestureRecognizers.Add(gesture);
            LampImage.IsVisible = true;

            LampImage.Source = ImageSource.FromFile("imglampapagada.png");


            MessagingCenter.Subscribe<MqttHelperInitializer>(this, "Subscribed", c => { Device.BeginInvokeOnMainThread(() => ChangeVisibility()); });
            MessagingCenter.Subscribe<MqttHelperInitializer>(this, "ControleChanged", c => { Device.BeginInvokeOnMainThread(() => LampChanged()); });
        }

        private async Task LampImage_Clicked()
        {
            //Recupera estado atual da lampada e envia o estado contrário para o broker. Se está 'liga', então envia 'desliga'
            var dictionary = MqttHelperInitializer.ControleLampada
                .Where(c => c.Value != MqttHelperInitializer.Controle).FirstOrDefault();

            MqttHelperInitializer.Publish(MqttHelperInitializer.LampadaTopic, dictionary.Key);

            //Desativa o botão até o comando tiver resposta
            LampImage.IsEnabled = false;
            activityIndicator.IsRunning = true;

            await Task.FromResult(true);
        }

        private void LampChanged()
        {
            //Ativa novamente o botão
            LampImage.IsEnabled = true;
            activityIndicator.IsRunning = false;

            var estadoAtual = MqttHelperInitializer.Controle; //Estado atual da lampada pós publicação no broker

            var dictionary = MqttHelperInitializer.ControleLampada
            .Where(c => c.Value == MqttHelperInitializer.Controle).FirstOrDefault();

            LampImage.Source = dictionary.Value ? ImageSource.FromFile("imglampacesa.png") : ImageSource.FromFile("imglampapagada.png");
        }

        void ChangeVisibility()
        {
            LampImage.IsVisible = MqttHelperInitializer.ControleAvaliable;
        }
    }
}
