﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEMethod : MonoBehaviour {

	//public string methodName;
	public int from;
	public int to;
	public int forPlayer;

	//static entry exit manager ??

	public void SetUpBasicInfo(int from_room, int to_room, int for_player) {
		from = from_room;
		to = to_room;
		forPlayer = for_player;
		//eeManager = eem;
	}

	virtual public void InitMethod() {
		//different for different type
		//1.Method entity instantiate
		
	}

	virtual public void CleanUpMethod() {

	}

	virtual public void TeleportTriggered() {
		//Call EEManager to teleport player  to _

	}

	

}