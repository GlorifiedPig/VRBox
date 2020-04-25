using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }

    public static Client[] clients;

    private static TcpListener tcpListener;

    public static void Start( int maxPlayers, int port ) {
        MaxPlayers = maxPlayers;
        Port = port;
        clients = new Client[MaxPlayers];

        Debug.Log( "Starting VRBox server..." );
        InitializeServerData();

        tcpListener = new TcpListener( IPAddress.Any, port );
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient( new AsyncCallback( TCPConnectCallback ), null );

        Debug.Log( $"VRBox server started on port {port}." );
    }

    private static void TCPConnectCallback( IAsyncResult result ) {
        TcpClient client = tcpListener.EndAcceptTcpClient( result );
        tcpListener.BeginAcceptTcpClient( new AsyncCallback( TCPConnectCallback ), null );
        Debug.Log( $"Incoming connection from {client.Client.RemoteEndPoint}..." );

        for( int i = 1; i <= MaxPlayers; i++ ) {
            if( clients[i].tcp.socket == null ) {
                clients[i].tcp.Connect( client );
                return;
            }
        }
        Debug.Log( $"{client.Client.RemoteEndPoint} failed to connect: Server full." );
    }

    private static void InitializeServerData() {
        for( int i = 1; i <= MaxPlayers; i++ ) {
            clients[i] = new Client( i );
        }
    }
}
