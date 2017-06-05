using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rocket : NetworkBehaviour {

	public GameObject ID;
	public MasterManager Master;

	void OnTriggerEnter (Collider collider) {

		if (collider.gameObject != ID){

			if (isServer) {
				if (collider.tag == "Player") {
					ID.GetComponent<PlayerData>().PlayerScore++;
					collider.gameObject.GetComponent<CCC>().TakeHit();
				}

				Master.SwapGun(ID);
			}

			Destroy(transform.gameObject);
		}
	}
}
