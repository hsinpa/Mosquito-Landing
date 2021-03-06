﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using Utility;

public class GameController : Observer {
	MosquitoHandler _mosquitoHandler;
	CameraHandler _camera;
	BackgroundView _background;
	GameView view;

	GameModel _gameModel;
	SoundModel _soundModel;
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
				_gameModel = MainApp.Instance.model.FindModel<GameModel>();
				_soundModel = MainApp.Instance.model.FindModel<SoundModel>();
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
		SpawnPoint spawnPoint = view.GetViewObject<SpawnPoint>();
		_camera.SetUp(backgroundRenderer);
		_soundModel.SetUp();
		_gameModel.SetUp(0.6f);
		_mosquitoHandler.SetUp(_camera, spawnPoint, _soundModel);
		_background.SetUp();
	}

	private void CleanUp() {
		_gameUIController._modalView.CloseAll();
	}
}
