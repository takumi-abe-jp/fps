using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Respawn : NetworkBehaviour {

	private Player_Health healthScript;
	private Image crossHairImage;
	private GameObject respawnButton;

	//********** 開始 **********//
	//メソッド名修正(Start → PreStartClient)
	public override void PreStartClient()
	{
		healthScript = GetComponent<Player_Health>();
		healthScript.EventRespawn += EnablePlayer;
	}

	//メソッド名修正(Start → OnStartLocalPlayer)
	public override void OnStartLocalPlayer () {

		crossHairImage = GameObject.Find("Crosshair Image").GetComponent<Image>();
		SetRespawnButton();
	}
	//********** 終了 **********//

	void SetRespawnButton ()
	{
		if (isLocalPlayer) {
			respawnButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton;
			respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn);
			respawnButton.SetActive(false);
		}
	}

	void Update () {

	}

	//********** 開始 **********//
	//メソッド名修正(OnDisable → OnNetworkDestroy)
	public override void OnNetworkDestroy()
	//********** 終了 **********//
	{
		healthScript.EventRespawn -= EnablePlayer;
	}

	void EnablePlayer()
	{
		GetComponent<CharacterController> ().enabled = true;
		GetComponent<Player_Shooting> ().enabled = true;
		GetComponent<BoxCollider> ().enabled = true;

		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer ren in renderers) {
			ren.enabled = true;
		}

		if(isLocalPlayer){
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			crossHairImage.enabled = true;
			respawnButton.SetActive(false);
		}
	}

	void CommenceRespawn()
	{
		CmdRespawnOnServer();
	}

	[Command]
	void CmdRespawnOnServer()
	{
		healthScript.ResetHealth();
	}

}