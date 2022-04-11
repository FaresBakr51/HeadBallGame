using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
public class AutoHost : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] private Text _iptxt;
    public  string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
    void Start()
    {
        _iptxt.text = LocalIPAddress();
        //if (!Application.isBatchMode)
        //{ //Headless build
        //    Debug.Log($"=== Client Build ===");
        //    networkManager.StartClient();
        //}
        //else
        //{
        //    Debug.Log($"=== Server Build ===");
        //}
    }
    public void JoinLocal()
    {
        networkManager.networkAddress = LocalIPAddress();
        networkManager.StartClient();
    }


}
