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

		public const string Restart = "game.start@event";

		public const string GameEnd = "game.end@event";
	}

	public class Modal {
		public const string Open = "modal.open@event";
		public const string Close = "modal.close@event";
	}

	public class Death {
		public const string Squash = "death_style.end@squash";
		public const string OverImbeded = "death_style.end@over_imbeded";
		public const string HitWall = "death_style.end@hit_wall";
	}
}

public enum InputMode {
		FirstPerson=0,
		Examination,
}

