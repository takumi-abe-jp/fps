using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Player_ID : NetworkBehaviour {

	//SyncVar: [Command]で変更後、全クライアントへ変更結果を送信
	[SyncVar] private string playerUniqueIdentity;
	//???
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	//NetworkManagerによってPlayerが生成された時に実行される
	public override void OnStartLocalPlayer()
	{
		GetNetIdentity();
		SetIdentity();
	}

	void Awake () {
		//自分の名前を取得する時に使う
		myTransform = transform;
	}

	// Update is called once per frame
	void Update ()
	{
		//例外が発生した時にSetIdentityメソッド実行
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
			SetIdentity();
		}
	}

	[Client]
	void GetNetIdentity()
	{
		//NetworkIdentityのNetID取得
		playerNetID = GetComponent<NetworkIdentity>().netId;
		//名前を付けるメソッド実行
		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}

	void SetIdentity ()
	{
		//自分以外のPlayerオブジェクトの場合
		if (!isLocalPlayer) {
			//今付いている名前のまま
			myTransform.name = playerUniqueIdentity;
		} else {
			//自分自信の場合、MakeUniqueIdentityメソッドで名前を取得
			myTransform.name = MakeUniqueIdentity();
		}
	}

	string MakeUniqueIdentity ()
	{
		//Player + NetIDで名前を付ける
		string uniqueName = "Player " + playerNetID.ToString();
		return uniqueName;
	}

	//Command: SyncVar変数を変更し、変更結果を全クライアントへ送る
	[Command]
	void CmdTellServerMyIdentity (string name)
	{
		playerUniqueIdentity = name;
	}
}