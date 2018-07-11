using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  MosquitoInput
{

	public class MosquitoInputManager {

		BaseInput _input;

		public MosquitoInputManager() {
			#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
				_input = new KeyboardInput();

			#elif UNITY_IOS || UNITY_ANDROID

			#endif
		}

		public int IsRightClick() {
			return _input.IsRightClick();
		}

		public int IsLeftClick() {
			return _input.IsLeftClick();
		}

		public int IsFrontClick() {
			return _input.IsFrontClick();
		}

	}

}

