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

	LineRenderer _line;
	Player _player;
	MasterManager _master;

	void Start()
	{
		_master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();
		_line = GetComponent<LineRenderer>();

		if (isLocalPlayer) {
			_player = ReInput.players.GetPlayer(0);
		}
	}

	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			if (_canFireRocket) {
				if (_player.GetButtonDown("Fire")) {
					CmdFire(RocketSpawn.position, RocketSpawn.rotation);
					Desactivate();
				}
				else if (Timer > 0) {
					Timer -= Time.deltaTime;
					if (Timer <= 0) {
						CmdSwapGun();
					}
				}
			}
		}

		if (_canFireRocket) {

			_line.enabled = true;

			RaycastHit hit;
			Physics.Raycast(RocketSpawn.position, RocketSpawn.forward, out hit);
			Vector3[] laser = new Vector3[2] {RocketSpawn.position, hit.point};
			_line.SetPositions(laser);
		}
		else {
			_line.enabled = false;
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
