using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector]
    public static EnemyManager Instance { get; private set; }

    private List<FSMEnemies> enemies = new List<FSMEnemies>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterEnemy(FSMEnemies enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void HealAllEnemies()
    {
        foreach (FSMEnemies enemy in enemies)
        {
            enemy.Heal();
        }
    }

    public void ReviveAllEnemies()
    {
        foreach (FSMEnemies enemy in enemies)
        {
            enemy.transform.parent.gameObject.SetActive(true);
        }
    }
}
