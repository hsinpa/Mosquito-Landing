using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  MosquitoInput {
	public abstract class BaseInput {

		abstract public int IsRightClick();
		abstract public int IsLeftClick();

		public virtual int IsFrontClick(){
			return 0;
		}

	}
}