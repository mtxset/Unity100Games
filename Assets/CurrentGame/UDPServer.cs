using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Minigames.UDPServer
{
  public class UDPServer : MonoBehaviour
  {
    public int Port = 9050;

    private Socket socket;
    private EndPoint remoteEndPoint;

    private void Start()
    {
      int recv;
      byte[] data = new byte[1024];
      var ipEndPoint = new IPEndPoint(IPAddress.Any, Port);

      socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

      socket.Bind(ipEndPoint);

      var sender = new IPEndPoint(IPAddress.Any, 0);
      remoteEndPoint = (EndPoint)(sender);

      // recv = socket.ReceiveFrom(data, ref remoteEndPoint);

      // Debug.Log($"Message received from {remoteEndPoint.ToString()}:");
      // Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

      // string welcome = "Welcome to my test server";
      // data = Encoding.ASCII.GetBytes(welcome);
      // socket.SendTo(data, data.Length, SocketFlags.None, remoteEndPoint);

      var recvThread = new Thread(new ThreadStart(receiveUDP));
      recvThread.IsBackground = true;
      recvThread.Start();
    }

    private void receiveUDP()
    {
      while (true)
      {
        Debug.Log("Waiting for a client...");
        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref remoteEndPoint);

        Debug.Log($"Message received from {remoteEndPoint.ToString()}:");
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
        //socket.SendTo(data, recv, SocketFlags.None, remoteEndPoint);
      }
    }
  }
}
