using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using Utility;

public class GameController : Observer {
	MosquitoHandler _mosquitoHandler;
	CameraHandler _camera;

	GameView view;
	

    public override void OnNotify(string p_event, params object[] p_objects) {
        base.OnNotify(p_event, p_objects);

        switch (p_event) {
            case EventFlag.Game.SetUp : { 
                Debug.Log("Game Start");
                //PreparePhrase();
				view = MainApp.Instance.view;
				_mosquitoHandler = view.GetViewObject<MosquitoHandler>();
				_camera = Camera.main.transform.GetComponent<CameraHandler>();
	
				GameStart();
            }
            break;
        }
    }

	private void GameStart() {
		SpriteRenderer backgroundRenderer = view.GetViewObject("background").GetComponent<SpriteRenderer>();
		Debug.Log(_camera.name);
		
		_mosquitoHandler.SetUp(_camera);
		_camera.SetUp(backgroundRenderer);
	}

}
