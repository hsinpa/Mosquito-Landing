using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Utility;
/// <summary>
/// Class that implements the functionalities of modal windows.
/// </summary>
public class Modal : MonoBehaviour
{
    /// <summary>
    /// Flag that indicates the type of modal.
    /// </summary>
    // public ModalType type;

    /// <summary>
    /// Flag that indicates the modal should hide the hud when it shows up.
    /// </summary>
    public bool hideHud = false;


    public bool fadeAnimation = true;

    /// <summary>
    /// Reference to the canvas group.
    /// </summary>
    public CanvasGroup group { get { return GetComponent<CanvasGroup>(); } }

    public virtual void SetUp(params object[] p_objects) {

    }

    /// <summary>
    /// Method called when the modal is requested to open.
    /// </summary>
    public virtual void Open() 
    {
        float d = OnModalOpen();
        // Notify(d, N.Modal.Open);
        group.interactable = true;
        group.blocksRaycasts = true;
        Cull(false);
        transform.SetAsLastSibling();
		// transform.parent.Find("background").GetComponent<Image>().enabled = useBackground;

        //Stop the character voice
        // if (app.view.audio != null) app.view.audio.PlayDialogueVoice(null);

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetTrigger("Open");
    }

    /// <summary>
    /// Method called when the modal is requested to close.
    /// </summary>
    public virtual void Close() 
    {
        OnModalClose();
        // Notify(d, N.Modal.Close);
        group.interactable = false;
        group.blocksRaycasts = false;
		// transform.parent.Find("background").GetComponent<Image>().enabled = false;
    }

    /// <summary>
    /// Method to be extended and implement the open behaviour, returning the duration of the transition.
    /// </summary>
    /// <returns></returns>
    virtual protected float OnModalOpen() {
//		RectTransform rec = GetComponent<RectTransform>();
//		Tween.Add<Vector2>(rec, "anchoredPosition", Vector2.zero, exitTime, Cubic.Out);	
		group.DOKill();
		group.DOFade(1, 0.4f);

        return 0.3f;
    }

    /// <summary>
    /// Method to be extended and implement the close behaviour, returning the duration of the transition.
    /// </summary>
    /// <returns></returns>
    virtual protected float OnModalClose() {
		RectTransform rec = GetComponent<RectTransform>();
		// Tween.Add<Vector2>(rec, "anchoredPosition", new Vector2(1500f, 0f) , exitTime, Cubic.Out);
		group.DOFade(0, 0.2f).OnComplete( ()=> {
            rec.anchoredPosition =  new Vector2(10000f, 0f);
        });
		return 0.2f;
    }

    /// <summary>
    /// Move the ui away.
    /// </summary>
    /// <param name="p_flag"></param>
    public void Cull(bool p_flag)
    {
		//group.alpha = p_flag ? 0 : 1;
		RectTransform rec = GetComponent<RectTransform>();        
        if (fadeAnimation) {
            rec.DOLocalMove(p_flag ? new Vector2(10000f, 0f) : Vector2.zero, 1);
        } else {
		    rec.anchoredPosition = p_flag ? new Vector2(10000f, 0f) : Vector2.zero;
        }
    }
}
