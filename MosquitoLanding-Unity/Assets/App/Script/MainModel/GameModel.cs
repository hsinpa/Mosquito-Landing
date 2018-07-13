using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour {
	public static float winCondition;

	public void SetUp(float p_winCondition) {
		winCondition = p_winCondition;
	}
}
