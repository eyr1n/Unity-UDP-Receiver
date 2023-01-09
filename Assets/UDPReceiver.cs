using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
  private UdpClient receiver;
  private Vector3 force = new(0, 0, 0);

  private Rigidbody rb;

  void Start()
  {
    rb = this.GetComponent<Rigidbody>();
    receiver = new(54321);

    Task.Run(() =>
    {
      while (true)
      {
        IPEndPoint ep = null;
        byte[] msg = receiver.Receive(ref ep);

        // MemoryMarshalでbyte[]をfloat[]に
        Span<float> converted = MemoryMarshal.Cast<byte, float>(msg);

        force.x = converted[0];
        force.z = converted[1];
      }
    });
  }

  void Update()
  {
    rb.AddForce(force);
  }
}
