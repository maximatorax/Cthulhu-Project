using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatsSystem
{
    void AddStatPoints(int nbStatPoints);
    void SubStatPoints(int nbStatPoints);
    void AddExp(int expToAdd);
    void LevelUp();
    void IncreaseStrength();
    void DecreaseStrength();
    void IncreaseAgility();
    void DecreaseAgility();
    void IncreaseConstitution();
    void DecreaseConstitution();
    void IncreaseWisdom();
    void DecreaseWisdom();
    void IncreaseIntelligence();
    void DecreaseIntelligence();
    IEnumerator Leveling();
    void Confirm();
}
