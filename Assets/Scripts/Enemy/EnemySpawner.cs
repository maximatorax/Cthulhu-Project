using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> SpawnersList;

    public List<GameObject> EnemiesList;

    public GameObject Player;

    public int nbOfEnemyWanted;
    private int nbOfEnemy = 0;

    void Update()
    {
        if(nbOfEnemy >= nbOfEnemyWanted) return;
        foreach (Transform spawner in SpawnersList)
        {
            if (spawner == null) continue;
            if (Vector3.Distance(Player.transform.position, spawner.position) < 5)
            {
                Instantiate(EnemiesList[0], spawner.position, Quaternion.identity);
                Destroy(spawner.gameObject);
                nbOfEnemy++;
            }
        }
    }
}
