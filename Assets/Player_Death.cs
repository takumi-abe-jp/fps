using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Death : NetworkBehaviour {

	private Player_Health healthScript; //Player_Healthスクリプトの変数
	private Image crossHairImage; //照準のImage

	void Start () {
		//キャッシュしておく
		crossHairImage = GameObject.Find("Crosshair Image").GetComponent<Image>();
		healthScript = GetComponent<Player_Health>();
		//Eventを登録
		healthScript.EventDie += DisablePlayer;
	}

	//メモリーリークした時用の、安全のためのメソッド そんなに重要じゃないらしい
	//OnDisable: 消滅する時に呼ばれる
	void OnDisable()
	{
		//EventからDisablePlayerメソッドを削除
		healthScript.EventDie -= DisablePlayer;
	}

	//Eventで登録されるメソッド　CheckConditionメソッド内で使われる
	//各コンポーネントを非アクティブ状態にする
	void DisablePlayer ()
	{
		GetComponent<CharacterController> ().enabled = false;
		GetComponent<Player_Shooting> ().enabled = false;
		GetComponent<BoxCollider> ().enabled = false;

		//子オブジェクトのRendererを全て格納
		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		//格納したRenderer全てを比アクティブ化
		foreach (Renderer ren in renderers) {
			ren.enabled = false;
		}
		//isDeadをtrueにすることで、CheckConditionのif文内に入らないようにする
		healthScript.isDead = true;

		if(isLocalPlayer){
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = false;
			crossHairImage.enabled = false;
			//Respawn Button (次回以降でPlayerを復活させるためのボタンを生成する箇所)
			GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton.SetActive(true);

		}
	}
}