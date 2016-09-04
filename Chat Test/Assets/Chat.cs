using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour
{
    // メッセージを管理するリスト
    private List<string> messages = new List<string>();
    // Chat用のテキスト
    private string currentMessage = string.Empty;

    private void OnGUI()
    {
        // 接続が確率されていた場合の処理
        if (NetWorkMgr.Connected)
        {
            GUILayout.Space(150);

            GUILayout.BeginHorizontal(GUILayout.Width(250));

            // 入力情報の取得
            currentMessage = GUILayout.TextField(currentMessage);

            // Sendボタンの処理
            if (GUILayout.Button("Send"))
            {
                // 入力情報が空ではない場合処理します。
                if (!string.IsNullOrEmpty(currentMessage.Trim()))
                {
                    // GUIDを先頭に付加したメッセージを作成する。
                    // （本来ならユーザを特定する名称を使った方がよいかもです。）
                    string msg = Network.player.guid + ": " + currentMessage;

                    // ここがポイント本来なら以下のようにメソッドを記述し実行します。
                    // this.chatMessage(msg);
                    // メッセージを共有するため接続されているピアのRPC関数を呼び出す事ができる。
                    // つまりここが接続している端末に送信している部分となる。
                    // （※RPCMode.Allは、接続している全ての端末に送信する事になる。）
                    GetComponent<NetworkView>().RPC("chatMessage", RPCMode.All, new object[] { msg });

                    // 送信後は、入力値を空にしておく。
                    currentMessage = string.Empty;
                }
            }

            GUILayout.EndHorizontal();

            // 入力されたメッセージを逆順に表示していく。
            for (int i = messages.Count - 1; i >= 0; i--)
            {
                GUILayout.Label(messages[i]);
            }
        }
    }

    // 接続されているピアで認識されるRPC関数として指定するために[RPC]を記述する。
    [RPC]
    public void chatMessage(string msg)
    {
        // 引数のメッセージをローカルの配列にセットする。
        messages.Add(msg);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
