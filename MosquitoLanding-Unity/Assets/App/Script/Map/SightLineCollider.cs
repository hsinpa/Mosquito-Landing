using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightLineCollider : MonoBehaviour {

    HandMove handMove;
    public float sighttime;
    float tt;
    public SpriteRenderer EyeSprite;
    bool see;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (see)
        {
            if (tt>sighttime)
            {
                handMove.alert = true;
            }
            tt += Time.deltaTime;
            EyeSprite.color = new Color(EyeSprite.color.r, EyeSprite.color.g, EyeSprite.color.b, Mathf.Lerp(EyeSprite.color.a, 1, 0.1f));
            if (EyeSprite.color.b > 0)
            {
                EyeSprite.color = new Color(EyeSprite.color.r, EyeSprite.color.g - ((1 / sighttime) * Time.deltaTime), EyeSprite.color.b - ((1 / sighttime) * Time.deltaTime), EyeSprite.color.a);
            }
        }
        else
        {
            if (handMove.alert==false) {
                if (tt > 0)
                    tt -= Time.deltaTime;
                EyeSprite.color = Color.Lerp(EyeSprite.color, new Color(1, 1, 1, 0), 0.05f);
            }

        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
       if(col.tag == "Player")
        {
            handMove = GetComponentInParent<HandMove>();
            see = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            see = false;
        }
    }

}
