﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour {
	public Text msgtxt;
	public int forPlayer;
	//public Portal portal;
	//public GameObject portalBtn;
	
	//TODO:TV audio

	AudioSource audio;
	public AudioClip buttonSound;
	public AudioClip msgSound;

	public GameObject sendBackButton;
	public GameObject inviteButton;

	void Start () {
		if(Settings.instance.id != forPlayer) {
			gameObject.SetActive(false);
		}

		audio = gameObject.AddComponent<AudioSource>();

		EntryExitManager.instance.notifyGuestArrive += OnGuestArrive;
		EntryExitManager.instance.notifyGuestLeft += OnGuestLeft;

		inviteButton.SetActive(true);
		if(sendBackButton!=null) {
			sendBackButton.SetActive(false);
		} 
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.I)) {
			OnInviteClicked();
		}
		

		if(Input.GetKeyDown(KeyCode.P)) {
			OnPortalClicked();
		}

	}

	//Buttons:
	public void OnInviteClicked() {
		//Debug.Log("Invite Clicked");

		inviteButton.SetActive(false);

		SetMsg("Invitation sent. Awaiting for guest response...");
		MessageSystem.instance.SendInvitation(1,"Hi");
		audio.PlayOneShot(buttonSound);
	}

	public void OnQuestionClicked() {
		//Debug.Log("Question Clicked");
		
	}

	public void OnPortalClicked() {
		Debug.Log("Portal Clicked");

		audio.PlayOneShot(buttonSound);
		switch(Settings.instance.method) {
			case 0:
				SetMsg("Guest's portal to home is outside the door.");
				break;
			case 1:
				SetMsg("Guest's magic wand is here to take him back home");
				break;
			
		}
		
		EntryExitManager.instance.OnSetUpMethod(Settings.instance.id,1,1);

		if(sendBackButton!=null) {
			sendBackButton.SetActive(false);
		}
	}


	public void SetMsg(string msg) {
		audio.PlayOneShot(msgSound);
		msgtxt.text = msg;
	}


	//Callback
	public void OnGuestAccept() {
		SetMsg("Invitation accepted. Your guest is on the way!");
	}

	public void OnGuestArrive() {
		SetMsg("Your Guest is Arrived!");
		if(sendBackButton!=null) {
			sendBackButton.SetActive(true);
		}
	}

	public void OnGuestLeft() {
		SetMsg("Your Guest is already Left!");
		

		if(inviteButton!=null) {
			inviteButton.SetActive(true);
		}
	}

	IEnumerator CleanMsg(float timer) {
		yield return new WaitForSeconds(timer);
		SetMsg("");
		yield return null;
	}


}
