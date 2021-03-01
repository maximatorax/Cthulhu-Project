using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillSystem
{
    void LearnSkill(Skill skillToLearn);
    void UnlearnSkill(Skill skillToUnlearn);

}
