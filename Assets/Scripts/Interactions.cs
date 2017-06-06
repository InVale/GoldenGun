using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Rewired;
using DG.Tweening;
using System;
using System.IO;

public class Interactions : NetworkBehaviour {

	public GameObject RocketPrefab;
	public Transform RocketSpawn;
	public GameObject Gun;

	public float TimeBeforeSwitch = 10;
	public float TravelSpeed = 10;
	public bool _canFireRocket = true;

	public float Timer = -1;

	Player _player;
	MasterManager _master;

	void Start()
	{
		if (isLocalPlayer) {
			_player = ReInput.players.GetPlayer(0);
			_master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();
		}
	}

	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {

			if (_canFireRocket) {

				if (_player.GetButtonDown("Fire")) {
					CmdFire(RocketSpawn.position, RocketSpawn.rotation);
					_canFireRocket = false;
					Gun.SetActive(false);
					Timer = -1;
				}
				else if (Timer > 0) {
					Timer -= Time.deltaTime;
					if (Timer <= 0) {
						Timer = -1;
						_canFireRocket = false;
						Gun.SetActive(false);
						CmdSwapGun();
					}
				}
			}
		}
	}

	[Command]
	void CmdSwapGun () {
		_master.SwapGun(gameObject);
	}

	public void Desactivate () {
		Timer = -1;
		_canFireRocket = false;
		Gun.SetActive(false);
		RpcDesactivate();
	}

	[ClientRpc]
	public void RpcDesactivate() {
		Timer = -1;
		_canFireRocket = false;
		Gun.SetActive(false);
	}

	public void Activate () {
		_canFireRocket = true;
		Gun.SetActive(true);
		RpcActivate();
	}

	[ClientRpc]
	public void RpcActivate() {
		_canFireRocket = true;
		Gun.SetActive(true);
		Timer = TimeBeforeSwitch;
	}

	[Command]
	void CmdFire(Vector3 myPosition, Quaternion myRotation)
	{
		var _rocket = (GameObject)Instantiate(RocketPrefab, myPosition, myRotation);

		_rocket.transform.GetComponent<Rigidbody>().velocity = _rocket.transform.forward * TravelSpeed;
		_rocket.transform.rotation *= Quaternion.Euler(90, 0, 0);

		Rocket rocketScript = _rocket.transform.GetComponent<Rocket> ();
		rocketScript.Master = _master;
		rocketScript.ID = gameObject;

		NetworkServer.Spawn(_rocket);
	}
}
