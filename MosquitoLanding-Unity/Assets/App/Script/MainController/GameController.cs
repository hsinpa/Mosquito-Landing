using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using Utility;

public class GameController : Observer {
	MosquitoHandler _mosquitoHandler;
	CameraHandler _camera;
	BackgroundView _background;
	GameView view;

	GameUIController _gameUIController;
	
    public override void OnNotify(string p_event, params object[] p_objects) {
        base.OnNotify(p_event, p_objects);

        switch (p_event) {
            case EventFlag.Game.SetUp : {
                Debug.Log("Game Start");
                //PreparePhrase();
				view = MainApp.Instance.view;
				_mosquitoHandler = view.GetViewObject<MosquitoHandler>();
				_background = view.GetViewObject<BackgroundView>();

				_camera = Camera.main.transform.GetComponent<CameraHandler>();

				_gameUIController = GetComponentInChildren<GameUIController>();
				_gameUIController._modalView.CloseAll();
				GameStart();
            }
            break;

			case EventFlag.Game.GameEnd: {
				Debug.Log("Game end");
				//Call end game modal
				EndGameModal endGameModal = _gameUIController._modalView.GetModal<EndGameModal>();
				_gameUIController.OpenModal(endGameModal, p_objects);
			}
			break;

			case EventFlag.Game.Restart: {
				CleanUp();
				_mosquitoHandler.Init();
			}break;

        }
    }

	private void GameStart() {
		CleanUp();
		SpriteRenderer backgroundRenderer = view.GetViewObject("background").GetComponent<SpriteRenderer>();
		Debug.Log(_camera.name);
		
		_mosquitoHandler.SetUp(_camera);
		_camera.SetUp(backgroundRenderer);
		_background.SetUp();
	}

	private void CleanUp() {
		_gameUIController._modalView.CloseAll();
	}
}
