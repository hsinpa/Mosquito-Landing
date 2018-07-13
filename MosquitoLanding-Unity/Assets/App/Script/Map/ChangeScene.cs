using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour {

    public float time;
    public string SceneName;
    public Image img;
    public float speed=0.3f;
    bool st;
    // Use this for initialization
    private void Update()
    {
        if (st)
        {
            img.color = img.color + new Color(0, 0, 0, speed * Time.deltaTime);
        }
    }

    public void go () {
        st = true;
        img.enabled = true;
        Invoke("cc", time);
	}
	
	public void cc()
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
}
