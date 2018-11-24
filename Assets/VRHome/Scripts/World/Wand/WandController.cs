﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {

	int state = 0;

	Follower follower;
	public WandParticleController wandParticleController;

	public GameObject shadowPrefab;
	ShadowParticleController shadow;

	MovingDetector movingDetector;

	public delegate void RequestTransport();
	public RequestTransport requestTransport;

	SimpleGrabable wandGrabable;

	// Vector3 hostBase;
	// Vector3 guestBase;

	PhotonView photonView;

	public void Init(Transform player, Vector3 hostBase, Vector3 guestBase) {
		follower = GetComponent<Follower>();
		
		movingDetector = GetComponent<MovingDetector>();
		movingDetector.enabled = false;

		wandGrabable = GetComponent<SimpleGrabable>();
		wandGrabable.onGrab += OnGrabbed;
		

		var cameraTransform = player.GetChild(0).GetChild(2);
		follower.Init(cameraTransform);

		var shadowGO = PhotonNetwork.Instantiate(shadowPrefab.name, new Vector3(0,0,0), Quaternion.identity, 0) as GameObject;
		shadow = shadowGO.GetComponent<ShadowParticleController>();
		shadow.Init(transform, guestBase, hostBase);

		PlayNetworkedParticleEffect(0);
		state = 0;
		
	}

	void Awake() {
		photonView = GetComponent<PhotonView>();
	}


	void PlayNetworkedParticleEffect(int index) {
		photonView.RPC("PlayNetworkedParticleEffectRPC",PhotonTargets.All,index);
	}


	[PunRPC]
	public void PlayNetworkedParticleEffectRPC(int index){
		wandParticleController.PlayParticleEffect(index);
	}


	
	void OnGrabbed() {
		if(state != 0) return;
		
		follower.Disable();
		movingDetector.enabled = true;
		wandGrabable.onGrab -= OnGrabbed;
		//wandParticleController.StopAll();
		PlayNetworkedParticleEffect(-1);
		state = 1;
	}

	void OnMovementAchieved(){
		if(state != 1) return;
		
		StartCoroutine(TransportEffect());
		
		state = 2;
	}

	IEnumerator TransportEffect() {
		//wandParticleController.PlayParticleEffect(2);
		PlayNetworkedParticleEffect(2);
		shadow.PlayNetworkParticle(2);
		yield return new WaitForSeconds(2f);
		StartTransport();
		yield return null;
	}

	void StartTransport(){
		state = 3;
		//wandParticleController.StopAll();
		PlayNetworkedParticleEffect(-1);
		shadow.StopAllNetworkParticle();

		if(requestTransport!=null)
			requestTransport();
		
		CleanUpWand();
	}

	void CleanUpWand() {
		PhotonNetwork.Destroy(shadow.gameObject);
	}


	bool lastIsMoving = false;
	void Update() {
		if(state == 1) {

			bool curIsMoving = movingDetector.IsMoving();

			if(lastIsMoving == false && curIsMoving == true) {
				//wandParticleController.PlayParticleEffect(1);
				PlayNetworkedParticleEffect(1);
				shadow.PlayNetworkParticle(1);
			}

			if(lastIsMoving == true && curIsMoving == false) {
				//wandParticleController.StopAll();
				PlayNetworkedParticleEffect(-1);
				shadow.StopAllNetworkParticle();
			}

			lastIsMoving = curIsMoving;

			if(movingDetector.DoesMovingLastSeconds(3f)) {
				OnMovementAchieved();
			}
		}

	}


}