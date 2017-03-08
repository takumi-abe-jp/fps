using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {
	[SerializeField]Camera FPSCharacterCam;
	[SerializeField]AudioListener audioListener;

	//********** 開始 **********//
	//Startメソッドから修正
	public override void OnStartLocalPlayer ()

	{
		//削除
		//if (isLocalPlayer) {
		//********** 終了 **********//
		GameObject.Find("Scene Camera").SetActive(false);
		GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
		FPSCharacterCam.enabled = true;
		audioListener.enabled = true;
		//		}
	}
}