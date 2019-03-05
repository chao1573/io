using UnityEngine;
using IO.Net;
using Common.Api;

public class Test : MonoBehaviour
{
    IClient _client;
    // Use this for initialization
    void Start()
    {
        _client = new Client();
        _client.Connect(complete =>
        {
            Debug.Log("Connect " + complete);
        });
        _client.DisconnectEvent += () =>
        {
            Debug.Log("disconnect");
        };
    }

    public void SendMessage()
    {
        var envelope = new Envelope { Groups = new TGroupsList { } };
        envelope.Groups.Count = 100;
        envelope.Groups.PageLimit = 10;
        _client.SendCollation(envelope, message =>
        {

        }, error => { });
    }
}
