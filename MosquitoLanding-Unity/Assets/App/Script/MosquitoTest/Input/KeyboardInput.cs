using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  MosquitoInput {
	public class KeyboardInput : BaseInput {

		// Use this for initialization
		public override int IsRightClick () {
			return (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) ? 1 : 0;
		}
		
		// Update is called once per frame
		public override int IsLeftClick () {
			return (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) ? 1 : 0;
		}

		public override int IsFrontClick(){
			return (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) ? 1 : 0;
		}
	}
}