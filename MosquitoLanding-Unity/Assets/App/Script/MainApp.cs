﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

public class MainApp : Singleton<MainApp> {
	protected MainApp () {} // guarantee this will be always a singleton only - can't use the constructor!

	public Subject subject;

	public GameView view {
		get {
			Transform t_view = transform.Find("view");
			if (t_view) return t_view.GetComponent<GameView>();
			return null;
		}
	}

	public Model model {
		get {
			Transform t_view = transform.Find("model");
			if (t_view) return t_view.GetComponent<Model>();
			return null;
		}
	}
	
	private Observer[] observers = new Observer[0];

	void Awake() {
		//Set up event notificaiton
		subject = new Subject();

		RegisterAllController(subject);
		subject.notify(EventFlag.Game.SetUp);
	}

	public Observer GetObserver<T>() where T : Observer {
		
		foreach (Observer observer in observers) {
			if (observer.GetType() == typeof(T)) return (T)observer;
		}

		return default(T);
	}

	void RegisterAllController(Subject p_subject) {
		Transform ctrlHolder = transform.Find("controller");
		if (ctrlHolder == null) return;
		observers = ctrlHolder.GetComponentsInChildren<Observer>();
		
		foreach (Observer observer in observers) {
			subject.addObserver( observer );
		}
	}

}
