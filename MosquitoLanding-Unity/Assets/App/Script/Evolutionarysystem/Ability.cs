using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour,IHealth {

    public List<IFeature> features;
    public List<ISkill> skills;
    List<ISkill> ActivateSkills;
    public List<string> Vs = new List<string>() {"a","b","c"};
    public int hp;

    
	// Use this for initialization
	void Start () {
        foreach (IFeature ft in features)
        {
            ft.AbilityUP();
        }

    }
	
	// Update is called once per frame
	void Update () {
        foreach(ISkill skill in ActivateSkills)
        {
            skill.Skill();
        }
    }

    

    public void Damage()
    {
        
    }
}
