using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPDemoApp.MQTT
{
    public interface IMqtt
    {

        Action<string,string> PublishReceivedCallBack { get; set; }
        Action<bool> SubscribedSuccessCallBack { get; set; }
        Action<bool> PublishedSuccessCallBack { get; set; }
        Action<bool> UnsubscribedSuccessCallBack { get; set; }

        bool Connect(string ipAdress, string port, string username, string password);
        bool SubscribeToTopic(string topic);
        bool UnsubscribeToTopic(string topic);
        bool PublishInTopic(string topic, string command);
    }
}
