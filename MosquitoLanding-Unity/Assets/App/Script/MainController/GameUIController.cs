using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

public class GameUIController : Observer {
    public ModalView _modalView {
        get {
            return MainApp.Instance.view.GetViewObject<ModalView>();
        }
    }



	public override void OnNotify(string p_event, params object[] p_objects) {
        base.OnNotify(p_event, p_objects);

        switch (p_event) {
            case EventFlag.Modal.Open : {
                Modal _modal = (Modal)p_objects[0];
                OpenModal(_modal, p_objects);
            }
            break;

            case EventFlag.Modal.Close : { 
                _modalView.Close();
            }
            break;
        }
    }

    public void OpenModal(Modal p_modal, object[] p_objects) {
        p_modal.SetUp(p_objects);
		_modalView.Open(p_modal);
    }

}
