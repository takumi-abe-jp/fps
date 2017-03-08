using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
//********** 開始 **********//
using System.Collections.Generic; //Listを使う時のライブラリ
//********** 終了 **********//

[NetworkSettings(channel=0, sendInterval=0.033f)]
public class Player_SyncPosition : NetworkBehaviour {

	//********** 開始 **********//
	//hook: SyncVar変数が変更された時、指定メソッドを実行するようサーバーから全クライアントへ命令を出す
	[SyncVar (hook = "SyncPositionValues")]
	//********** 終了 **********//
	private Vector3 syncPos;

	[SerializeField] Transform myTransform;

	//********** 開始 **********//
	float lerpRate;
	float normalLerpRate = 15;
	float fasterLerpRate = 25;
	//********** 終了 **********//

	private Vector3 lastPos;
	private float threshold = 0.5f;
	private NetworkClient nClient;
	private int latency;
	private Text latencyText;

	//********** 開始 **********//
	//Position同期用のList
	private List<Vector3> syncPosList = new List<Vector3>();

	//HistoricalLerpingメソッドを使う時はtrueにする
	//SerializeFieldなのでInspectorビューから変更可能
	[SerializeField] private bool useHistoricalLerping = false;

	//2点間の距離を判定する時に使う
	private float closeEnough = 0.1f;
	//********** 終了 **********//

	void Start ()
	{
		nClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
		//********** 開始 **********//
		lerpRate = normalLerpRate;
		//********** 終了 **********//
	}

	void Update ()
	{
		LerpPosition();
		ShowLatency();
	}

	void FixedUpdate ()
	{
		TransmitPosition(); 
	}

	void LerpPosition ()
	{
		if (!isLocalPlayer) {
			//********** 開始 **********//
			if (useHistoricalLerping) {
				//前時代の補間メソッド
				HistoricalLerping();
			}
			else
			{
				//通常の補間メソッド
				OrdinaryLerping();
			}
			//********** 終了 **********//
			//			Debug.Log(Time.deltaTime.ToString());
		}
	}

	[Command]
	void CmdProvidePositionToServer (Vector3 pos)
	{
		syncPos = pos;
		//		Debug.Log("Command");
	}

	[ClientCallback]
	void TransmitPosition ()
	{
		if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold) {
			CmdProvidePositionToServer(myTransform.position);
			lastPos = myTransform.position;
		}
	}

	//********** 開始 **********//
	//クライアントのみ有効
	[Client]
	//hookで指定されたメソッド　全クライアントが実行
	void SyncPositionValues (Vector3 latestPos)
	{
		syncPos = latestPos;
		//ListにPosition追加
		syncPosList.Add(syncPos);
	}
	//********** 終了 **********//

	void ShowLatency ()
	{
		if (isLocalPlayer) {
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}

	//********** 開始 **********//
	//通常使われる補間メソッド
	void OrdinaryLerping ()
	{
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

	//過去使用されていた補間メソッド
	void HistoricalLerping ()
	{
		//Listが1以上あったら
		if (syncPosList.Count > 0) {
			//現在位置とListの0番目の位置との中間値を補間
			myTransform.position = Vector3.Lerp (myTransform.position, syncPosList [0], Time.deltaTime * lerpRate);

			//2点間がcloseEnoughより小さくなった時
			if (Vector3.Distance (myTransform.position, syncPosList [0]) < closeEnough) {
				//Listの0番目を削除
				syncPosList.RemoveAt (0);
			}
			//syncPosList.Countが0に戻った時、同期が追いついたことを意味する
			Debug.Log(syncPosList.Count.ToString());
		}
	}
	//********** 終了 **********//
}