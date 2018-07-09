using UnityEngine;
using System.Collections;

public class EnemyBoss : EnemyAI {

    public enum ModeState {Wander,follow,stop};
    public ModeState Mods = ModeState.Wander;
  
	public AnimationCurve rotateCurve ;
    public AnimationCurve[] rotateCurves;
    int curveNumber = 0;

    public bool loop = true ;		// loop curve
    internal float elapse = 0f ;     // time for rotate curve

    [Tooltip("color of the path in the Editor")]
    public Color pathColor = Color.yellow;


    void Start(){
		//targetPoint = pl.DirToPoint(initialPosition) ;
        //StartCoroutine(ModeChange(10));
        
    }
	
	public virtual void Update(){
        switch (Mods)
        {
            case ModeState.Wander:
                wander();
                break;

            case ModeState.follow:
                follow();
                break;

            case ModeState.stop:
                stop();
                break;
        }
       
    }
	
    public void wander()
    {
        if (loop)
        {
            elapse = Mathf.Repeat(elapse + Time.deltaTime, rotateCurves[curveNumber].keys[rotateCurves[curveNumber].keys.Length - 1].time);
        }
        else
        {
            elapse += Time.deltaTime;
        }
        /*Vector3 point = transform.position + transform.forward - pl.transform.position; // move forward
        point += transform.right * rotateCurves[curveNumber].Evaluate(elapse) * 0.6f; // get curve value, rotate via curve
        targetPoint = pl.DirToPoint(point);*/
        transform.right += new Vector3(0,1* rotateCurves[curveNumber].Evaluate(elapse) * 0.6f,0);
        MoveTo();
    }

    public void follow()
    {
        targetPoint = pl.dragonHead.position;
        MoveTo();
    }

    public void stop()
    {
        
    }
    /*
    void DrawPath(Transform[] path) //drawing the path in the Editor
    {
        Vector3[] pathPositions = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            pathPositions[i] = path[i].position;
        }
        Vector3[] newPathPositions = CreatePoints(pathPositions);
        Vector3 previosPositions = Interpolate(newPathPositions, 0);
        Gizmos.color = pathColor;
        int SmoothAmount = path.Length * 20;
        for (int i = 1; i <= SmoothAmount; i++)
        {
            float t = (float)i / SmoothAmount;
            Vector3 currentPositions = Interpolate(newPathPositions, t);
            Gizmos.DrawLine(currentPositions, previosPositions);
            previosPositions = currentPositions;
        }
    }
    */
    IEnumerator ModeChange(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        switch (Mods)
        {
            case ModeState.Wander:
                Mods = ModeState.follow;
                break;

            case ModeState.follow:
                Mods = ModeState.Wander;
                curveNumber = Random.Range(0, rotateCurves.Length);
                break;
        }
        StartCoroutine(ModeChange(10));
    }

   
}
