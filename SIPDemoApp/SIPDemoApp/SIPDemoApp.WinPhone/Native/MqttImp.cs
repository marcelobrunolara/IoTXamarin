using SIPDemoApp.MQTT;
using SIPDemoApp.WinPhone.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Xamarin.Forms;

[assembly: Dependency(typeof(MqttImp))]
namespace SIPDemoApp.WinPhone.Native
{
    public class MqttImp : IMqtt
    {
        #region Fields

        MqttClient client;
        private ushort packet = 0;

        #endregion

        #region Properties
        public Action<string, string> PublishReceivedCallBack { get; set; }
        public Action<bool> SubscribedSuccessCallBack { get; set; }
        public Action<bool> PublishedSuccessCallBack { get; set; }
        public Action<bool> UnsubscribedSuccessCallBack { get; set; }
        #endregion

        #region Methods
        public bool Connect(string ipAdress, string port, string username, string password)
        {
            client = new MqttClient(ipAdress, 16315, false, MqttSslProtocols.None);
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;

            client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            client.MqttMsgPublished += Client_MqttMsgPublished;
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgUnsubscribed += Client_MqttMsgUnsubscribed;

            byte code = client.Connect(Guid.NewGuid().ToString(), username, password);

            return code == 0; //if 0, connection accepted
        }
        public bool SubscribeToTopic(string topic)
        {
            //if is connected on broker
            if (client.IsConnected)
            {
                var code = client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                return true;
            }
            else return false;
        }
        public bool PublishInTopic(string topic, string command)
        {
            if (client.IsConnected)
            {
                client.Publish(topic, Encoding.UTF8.GetBytes(command), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                return true;
            }
            return false;
        }

        #endregion

        #region Events

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            PublishReceivedCallBack(e.Topic, Encoding.UTF8.GetString(e.Message,0,e.Message.Count()));
        }

        private void Client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            PublishedSuccessCallBack(e.IsPublished);
        }

        private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            SubscribedSuccessCallBack(e.GrantedQoSLevels[0] == MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE);
        }

        private void Client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            UnsubscribedSuccessCallBack(e.MessageId == packet);
            packet = 0;
        }

        public bool UnsubscribeToTopic(string topic)
        {
            if (client.IsConnected)
            {
                packet = client.Unsubscribe(new string[] { topic });
                return true;
            }
            else return false;
        }
        #endregion

        #region Constructor

        public MqttImp()
        {

        }

        #endregion
    }
}
