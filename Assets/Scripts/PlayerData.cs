using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour {

	HUDHandler _myHUD;
	Interactions _myInteractions;
	MasterManager _master;

	[SyncVar]
	public int PlayerScore = 0;
	[SyncVar]
	public int OpponentScore = 0;

	void Start () {
		_myHUD = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<HUDHandler>();
		_master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();
		_myInteractions = gameObject.GetComponent<Interactions>();
	}

	void Update () {
		if (isLocalPlayer) {
			_myHUD.ScoreData(PlayerScore, OpponentScore);
			_myHUD.Timer(_master.Timer);
		}
	}

	public void HurtAnim () {
		_myHUD.Hurt();
	}

	public void SwapAnim() {
		_myHUD.Swap ();
	}
}
