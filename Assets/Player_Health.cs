using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Health : NetworkBehaviour {

	[SyncVar (hook = "OnHealthChanged")] private int health = 100;
	private Text healthText;

	private bool shouldDie = false;
	public bool isDead = false;

	public delegate void DieDelegate();
	public event DieDelegate EventDie;

	//********** 開始 **********//
	//Player再生成のためのevent
	public delegate void RespawnDelegate();
	public event RespawnDelegate EventRespawn;
	//********** 終了 **********//

	void Start () {
		healthText = GameObject.Find("Health Text").GetComponent<Text>();
		SetHealthText();
	}

	void Update () {
		CheckCondition();
	}

	void CheckCondition ()
	{
		if (health <= 0 && !shouldDie && !isDead) {
			shouldDie = true;
		}

		if (health <= 0 && shouldDie) {
			if (EventDie != null) {
				EventDie ();
			}
			shouldDie = false;
		}

		//********** 開始 **********//
		//HPが1以上あるのにisDead=trueの時
		if (health > 0 && isDead){
			//EventRespawnに何か登録されている時
			if (EventRespawn != null){
				//EventRespawn実行
				EventRespawn();
			}
			isDead = false;
		}
		//********** 終了 **********//
	}

	void SetHealthText ()
	{
		if (isLocalPlayer) {
			healthText.text = "HP " + health.ToString();
		}
	}

	public void DeductHealth(int dmg)
	{
		health -= dmg;
	}

	void OnHealthChanged (int hlth)
	{
		health = hlth;
		SetHealthText();
	}

	//********** 開始 **********//
	public void ResetHealth ()
	{
		//Player_RespawnスクリプトのCmdRespawnOnServerメソッドが[Command]のため、
		//SyncVarが機能する
		health = 100;
	}
	//********** 終了 **********//
}