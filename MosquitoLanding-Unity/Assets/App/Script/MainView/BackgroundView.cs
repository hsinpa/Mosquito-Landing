using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundView : MonoBehaviour {

	public SpriteRenderer background_sprite;

	public void SetUp() {
		background_sprite = GetComponent<SpriteRenderer>();

		AutoAssignBorder();
	}

	public void AutoAssignBorder() {
		float offset = 0.25f;
		Vector2 bgSize = background_sprite.bounds.size;
		//
		BoxCollider2D topBorder = new GameObject("top_border").AddComponent<BoxCollider2D>();
        topBorder.gameObject.layer = 12;
		topBorder.transform.SetParent(this.transform);
		topBorder.transform.position = new Vector2(0, offset+ (bgSize.y * 0.5f));
		topBorder.size = new Vector2(bgSize.x, 0.5f);

		BoxCollider2D bottomBorder = new GameObject("bottom_border").AddComponent<BoxCollider2D>();
        bottomBorder.gameObject.layer = 12;
		bottomBorder.transform.SetParent(this.transform);
		bottomBorder.transform.position = new Vector2(0, -offset -(bgSize.y * 0.5f));
		bottomBorder.size = new Vector2(bgSize.x, 0.5f);

		BoxCollider2D leftBorder = new GameObject("left_border").AddComponent<BoxCollider2D>();
        leftBorder.gameObject.layer = 12;
		leftBorder.transform.SetParent(this.transform);
		leftBorder.transform.position = new Vector2(-offset-(bgSize.x * 0.5f), 0);
		leftBorder.size = new Vector2(0.5f, bgSize.y);

		BoxCollider2D rightBorder = new GameObject("right_border").AddComponent<BoxCollider2D>();
        rightBorder.gameObject.layer = 12;
		rightBorder.transform.SetParent(this.transform);
		rightBorder.transform.position = new Vector2(offset + (bgSize.x * 0.5f), 0);
		rightBorder.size = new Vector2(0.5f, bgSize.y);
	}

}
