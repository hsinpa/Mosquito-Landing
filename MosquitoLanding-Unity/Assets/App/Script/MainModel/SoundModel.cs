using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundModel : MonoBehaviour {

	public AudioClip[] audioClips;
	
	private Dictionary<string, AudioClip> audioClipDics = new Dictionary<string, AudioClip>();
	
	public void SetUp() {
		foreach(AudioClip audio in audioClips) {
			if (audioClipDics.ContainsKey(audio.name)) continue;
			audioClipDics.Add(audio.name, audio);
		}
	}

	public AudioClip GetClip(string p_audio_key) {
		Debug.Log(p_audio_key);
		if (audioClipDics.ContainsKey(p_audio_key)) {
			return audioClipDics[p_audio_key];
		}
		return null;
	}

}
