using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour {
	//Network variable to read/write values to see which arrows are being pressed.
	private NetworkVariable<ArrowKeys> arrows = new NetworkVariable<ArrowKeys>(
		new ArrowKeys {
			up = false,
			down = false,
			left = false,
			right = false,
		}, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

	//A struct to hold the different arrow values.
	public struct ArrowKeys : INetworkSerializable {
		public bool up;
		public bool down;
		public bool left;
		public bool right;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
			serializer.SerializeValue(ref up);
			serializer.SerializeValue(ref down);
			serializer.SerializeValue(ref left);
			serializer.SerializeValue(ref right);
		}
	}

	private Dictionary<GameObject, Action> arrowActions = new Dictionary<GameObject, Action>();

	//Basically on awake, on start
	public override void OnNetworkSpawn() {
		//upBtn.onClick.AddListener(GoUp);
	}

	private void Update() {
		//If the update is coming from the host
		if(IsHost) {
			if (arrows.Value.up) {
				Debug.Log("UP BUTTON PRESSED!");
			}
			else if (arrows.Value.down) {
				Debug.Log("DOWN BUTTON PRESSED!");
			}
			else if (arrows.Value.left) {
				Debug.Log("LEFT BUTTON PRESSED!");
			}
			else if (arrows.Value.right) {
				Debug.Log("RIGHT BUTTON PRESSED!");
			}
		} 
		
		//Update is coming from the client
		else {
			//Left click is being pressed
			if(Input.GetMouseButtonDown(0)) {
				///Check if mouse press is over the buttons
				if(EventSystem.current.IsPointerOverGameObject()) {
					GameObject clickedObject = EventSystem.current.currentSelectedGameObject;

					switch(clickedObject.name) {
						case "Up":
							GoUp();
							break;

						case "Down":
							GoDown();
							break;

						case "Left":
							GoLeft();
							break;

						case "Right":
							GoRight();
							break;
					}
				}
			}

			//Left mouse was let go, reset
			if(Input.GetMouseButtonUp(0)) {
				GoDefault();
			}
		}
	}
	
	//Helper methods to set directional values
	private void GoUp() {
		if (!IsOwner) return;

		arrows.Value = new ArrowKeys {
			up = true,
			down = false,
			left = false,
			right = false,
		};
	}
	private void GoDown() {
		if (!IsOwner) return;

		arrows.Value = new ArrowKeys {
			up = false,
			down = true,
			left = false,
			right = false,
		};
	}

	private void GoLeft() {
		if (!IsOwner) return;

		arrows.Value = new ArrowKeys {
			up = false,
			down = false,
			left = true,
			right = false,
		};
	}

	private void GoRight() {
		if (!IsOwner) return;

		arrows.Value = new ArrowKeys {
			up = false,
			down = false,
			left = false,
			right = true,
		};
	}

	private void GoDefault() {
		Debug.Log("Going default!");

		if (!IsOwner) return;

		arrows.Value = new ArrowKeys {
			up = false,
			down = false,
			left = false,
			right = false,
		};
	}
}
