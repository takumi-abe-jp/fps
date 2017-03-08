using UnityEngine;
using UnityEngine.Networking;

public class DisableRemotePlayerBehaviours : NetworkBehaviour {
	public Behaviour[] behaviours;
	// Use this for initialization
	void Start () {
		
		if (!isLocalPlayer) {
			foreach (var behabiour in behaviours) {
				behabiour.enabled = false;
			}
		}
	}
}
