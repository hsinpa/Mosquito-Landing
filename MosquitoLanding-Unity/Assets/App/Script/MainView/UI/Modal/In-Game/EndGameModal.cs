using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameModal : Modal {
	
	public override void SetUp(object[] p_objects) {
		float bloodImbidePercentage = (float)p_objects[0];
		int brokenPartCount = (int)p_objects[1];

		SetInfo(bloodImbidePercentage, brokenPartCount);
	}

	private void SetInfo(float p_bloodAmount, int p_brokenPartCount) {
		Image bloodBall = transform.Find("blood_ball").GetComponent<Image>();
		bloodBall.fillAmount = p_bloodAmount;

		Text infoText = transform.Find("menu/panel/basic_info").GetComponent<Text>();
		infoText.text = (p_bloodAmount >= GameModel.winCondition) ? "Success" : "Explode";
		infoText.text += "Score : " + Mathf.FloorToInt(p_bloodAmount * 100) + "%";
		infoText.text += "Broken Parts : " + p_brokenPartCount;
	}

	public void Restart() {
		MainApp.Instance.subject.notify(EventFlag.Game.Restart);
	}
   
}
