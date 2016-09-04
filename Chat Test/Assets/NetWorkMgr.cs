using UnityEngine;
using System.Collections;

public class NetWorkMgr : MonoBehaviour
{
    public string connectionIP = "127.0.0.1";
    public int portNumber = 8632;
    public static bool Connected { get { return connected; } }

    private static bool connected = false;

    private void OnGUI()
    {
        // 接続済み
        if (connected)
        {
            GUILayout.Label("Connections: " + Network.connections.Length.ToString());
            // 接続を切断する場合
            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
            }
        }
        else
        {
            // 画面の入力情報を取得する。
            connectionIP = GUILayout.TextField(connectionIP);
            int.TryParse(GUILayout.TextField(portNumber.ToString()), out portNumber);

            // Clientになる場合
            if (GUILayout.Button("Connect"))
            {
                Network.Connect(connectionIP, portNumber);
            }

            // Serverになる場合
            if (GUILayout.Button("Server"))
            {
                Network.InitializeServer(4, portNumber);
            }
        }
    }

    //サーバーが初期化されたとき、サーバー側で呼び出されます。
    void OnServerInitialized()
    {
        Debug.Log("Server initialized and ready");
        connected = true;
    }

    //サーバーに接続したとき、クライアント側で呼び出されます。
    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
        connected = true;
    }

    // プレイヤーが接続されたとき、サーバー側で呼び出されます。
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Connected from " + player.ipAddress + ":" + player.port);
        connected = true;
    }

    //プレイヤーが切断されたとき、サーバー側で呼び出されます。
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    //サーバーから切断したとき、クライアント側で呼び出されます。
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        connected = false;
        if (Network.isServer)
        {
            Debug.Log("Local server connection disconnected");
        }
        else if (info == NetworkDisconnection.LostConnection)
        {
            Debug.Log("Lost connection to the server");
        }
        else
        {
            Debug.Log("Successfully diconnected from the server");
        }
    }

    //サーバーの接続に失敗したとき、クライアント側で呼び出されます。
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Could not connect to server: " + error);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
