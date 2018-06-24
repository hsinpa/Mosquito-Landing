using UnityEngine;
using System.Collections;

public class EventFlag : MonoBehaviour {
	public static int normalLayer = 0;
	public static int examineLayer = 9;

	public static int normalRaycastLayer = 1 << 0;
	public static int BodySkinRaycastLayer = 1 << 9;


	public class Game {
		public const string SetUp = "game.setup@event";
		public const string EnterGame = "game.enter@event";

		public const string GameStart = "game.start@event";

		public const string GameEnd = "game.end@event";
	}	
}

public enum InputMode {
		FirstPerson=0,
		Examination,
}