using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TextEffect
{
	public class TypeWriter : MonoBehaviour{
		private Text _textObject;
		private List<TextCharacters> textArray = new List<TextCharacters>();
		private int _index;
		private bool isActivate;


		public void AddMessage(Text p_textObject, string p_sentence) {
			_textObject = p_textObject;

			SetActive(true);

			string[] words = p_sentence.Split( new string[] {" "}, System.StringSplitOptions.None);
			foreach(string word in words) {
				TextCharacters text_character = new TextCharacters(word);
				textArray.Add(text_character);
				textArray.Add(new TextCharacters(" "));

			}

			StartCoroutine(ShowText());
		}

		private IEnumerator ShowText() {
			for (int i = 0; i < textArray.Count; i++) {
				TextCharacters _text = textArray[i];

				for (int k = 0; k < _text.characters.Length; k++) {
					_textObject.text = _textObject.text + _text.characters[k].ToString();
					yield return new WaitForSeconds(_text.time_periods);
				}
			}
			
			yield return new WaitForSeconds(1.5f);
			_textObject.DOFade(0, 0.5f);
		}

		public void SetActive(bool p_bool) {
			isActivate = p_bool;
			textArray.Clear();
			_textObject.text = "";
			_index = 0;
		}
		
	}
}
