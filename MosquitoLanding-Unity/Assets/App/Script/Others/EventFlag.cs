using UnityEngine;
using System.Collections;

public class EventFlag : MonoBehaviour {
	public const int normalLayer = 0;
	public const int exitLayer = 1;
	public const int humanBodyLayer = 9;
	public const int mosquitoLayer = 10;
	public const int harmfulLayer = 12;

	public const int normalRaycastLayer = 1 << 0;
	public const int BodySkinRaycastLayer = 1 << 9;

	public class Game {
		public const string SetUp = "game.setup@event";
		public const string EnterGame = "game.enter@event";

		public const string Restart = "game.start@event";

		public const string GameEnd = "game.end@event";
	}

	public class Audio {
		public const string Crash1 = "mosquito.crash1";
		public const string Crash2 = "mosquito.crash2";
		public const string MosquitoFlying = "mosquito.flying";
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

