using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    public Sprite skillIcon;
    public bool skillActive;
    public int skillLevel;
    public int skillId;
    public string skillDescription;
}
