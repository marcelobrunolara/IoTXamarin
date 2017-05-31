using SIPDemoApp.MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SIPDemoApp
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            Title = "SIP 2017 - IoT & Xamarin";
            //var tab1 = new NavigationPage(new Pages.Control());
            //tab1.Icon = "icon.png";
            //tab1.Title = "Controle";

            //var tab2 = new NavigationPage(new Pages.Temperature());
            //tab2.Icon = "icon.png";
            //tab2.Title = "Temperatura";

            //var tab3 = new NavigationPage(new Pages.Humidity());
            //tab3.Icon = "icon.png";
            //tab3.Title = "Umidade";


            //Children.Add(tab1);
            //Children.Add(tab2);
            //Children.Add(tab3);

            InitializeComponent();
            MQTTConnectAndSubscribe();
        }

        private void MQTTConnectAndSubscribe()
        {
            MqttHelperInitializer obj = new MqttHelperInitializer();
            obj.ConnectAndSubscribeToAllTopics();
        }

    }
}
