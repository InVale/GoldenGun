using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MasterManager : NetworkBehaviour {

	GameObject[] Players;
	Interactions[] PlayerScripts;

	void Start () {
		if (isServer) {
			GetPlayers();
		}
	}

	void GetPlayers() {
		Players = GameObject.FindGameObjectsWithTag("Player");
		PlayerScripts = new Interactions[Players.Length];
		for (int i = 0; i < Players.Length; i++) {
			PlayerScripts[i] = Players[i].GetComponent<Interactions>();
		}
	}

	public void SwapGun (GameObject PlayerID) {

		if (isServer) {

			if (Network.connections.Length + 1 != Players.Length) {
				GetPlayers();
			}

			foreach (Interactions g in PlayerScripts) {
				if (Players.Length == 1) {
					g.Activate();
				}
				else {
					if (PlayerID == g.gameObject) {
						g.Desactivate();
					}
					else {
						g.Activate();
					}
				}
			}
		}
	}
}
