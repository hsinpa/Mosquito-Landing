using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TextEffect
{
	public class TextCharacters {
		public char[] characters;
		public Color color;
		public float time_periods;

		public TextCharacters(string p_raw_word) {
			characters = p_raw_word.ToCharArray();
			time_periods = 0.2f;
			color = Color.white;
		}
	}
}