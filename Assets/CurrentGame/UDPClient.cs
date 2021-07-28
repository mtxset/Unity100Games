using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Minigames.UDPClient {
	public class UDPClient : MonoBehaviour {

		public int Port = 9050;

		private Socket socket;
		private EndPoint remoteEndPoint;

		private void Start() {
			int recv;
      byte[] data = new byte[1024];
      var ipEndPoint = new IPEndPoint(IPAddress.Any, Port);

      socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			var ipAddr = IPAddress.Parse("127.0.0.1");
      var sender = new IPEndPoint(ipAddr, Port);
      remoteEndPoint = (EndPoint)(sender);

      // recv = socket.ReceiveFrom(data, ref remoteEndPoint);

      // Debug.Log($"Message received from {remoteEndPoint.ToString()}:");
      // Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

      // string welcome = "Welcome to my test server";
      // data = Encoding.ASCII.GetBytes(welcome);
      // socket.SendTo(data, data.Length, SocketFlags.None, remoteEndPoint);
		}

		public void sendUDPMessage() {
      string welcome = "Hello";
      var data = Encoding.ASCII.GetBytes(welcome);
      socket.SendTo(data, data.Length, SocketFlags.None, remoteEndPoint);
		}
	}
}
