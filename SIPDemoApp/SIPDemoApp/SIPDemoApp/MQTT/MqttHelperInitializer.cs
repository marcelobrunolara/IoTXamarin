
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SIPDemoApp.MQTT
{
    public class MqttHelperInitializer
    {
        public MqttHelperInitializer()
        {

        }

        #region Private
        private static IMqtt m2mqttClient;
        #endregion

        #region Properties
        public static bool TemperaturaAvaliable { get; set; }
        public static string Temperatura { get; set; }

        public static bool UmidadeAvaliable { get; set; }
        public static string Umidade { get; set; }

        public static bool ControleAvaliable { get; set; }
        public static bool Controle { get; set; }

        //READONLY TOPICS
        public static string TemperaturaTopic { get; } = "dht22/temperatura";
        public static string UmidadeTopic { get; } = "dht22/umidade";
        public static string LampadaTopic { get; } = "esp8266/pincmd";

        //READONLY COMMANDS
        public static Dictionary<string, bool> ControleLampada { get; } = new Dictionary<string, bool>() { { "liga", true }, { "desliga", false } };
        #endregion

        #region Methods

        public void ConnectAndSubscribeToAllTopics()
        {

            //Conecta no server
            m2mqttClient = DependencyService.Get<IMqtt>();
            m2mqttClient.Connect("m12.cloudmqtt.com", "16315", "xjcvmsjt", "H7_gY8PR5840");

            //Subscreve aos topicos
            m2mqttClient.SubscribeToTopic(LampadaTopic);
            m2mqttClient.SubscribeToTopic(TemperaturaTopic);
            m2mqttClient.SubscribeToTopic(UmidadeTopic);

            m2mqttClient.PublishedSuccessCallBack = PublishedSuccess;
            m2mqttClient.SubscribedSuccessCallBack = SubscribeSuccess;
            m2mqttClient.PublishReceivedCallBack = PublishReceived;

        }

        public static void Publish(string topic, string command)
        {
            m2mqttClient.PublishInTopic(topic, command);
        }

        private static void PublishedSuccess(bool isPublished)
        {

        }

        private void SubscribeSuccess(bool isSubcribedSuccess)
        {
            if (isSubcribedSuccess) //Se subscreveu aos topicos, então ativa os controles da tela
            {

                ControleAvaliable = true;
                MessagingCenter.Send<MqttHelperInitializer>(this, "Subscribed");
            }
        }

        private void PublishReceived(string topic, string message)
        {
            try
            {
                if (topic == UmidadeTopic)
                {
                    UmidadeAvaliable = true;
                    Umidade = message;
                    MessagingCenter.Send<MqttHelperInitializer>(this, "HumidityChanged");
                    return;
                }
                if (topic == TemperaturaTopic)
                {
                    TemperaturaAvaliable = true;
                    Temperatura = message;
                    MessagingCenter.Send<MqttHelperInitializer>(this, "TemperaturaChanged");
                    return;
                }
                if (topic == LampadaTopic)
                {
                    Controle = ControleLampada[message];
                    MessagingCenter.Send<MqttHelperInitializer>(this, "ControleChanged");
                    return;
                }
            }
            catch (System.Exception e)
            {
                var a = e;
                throw;
            }
        }
        #endregion

    }
}
