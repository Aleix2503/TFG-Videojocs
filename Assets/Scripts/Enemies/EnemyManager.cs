using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector]
    public static EnemyManager Instance { get; private set; }

    private List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void HealAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<FSMEnemies>().Heal();
        }
    }

    public void ReviveAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeSelf)
            {
                enemy.SetActive(true);
                enemy.GetComponent<FSMEnemies>().Revive();
                enemy.GetComponentInParent<Animator>().Play(enemy.GetComponent<FSMEnemies>().state.ToString(), - 1, 0f);
                ResetAllTriggers(enemy.GetComponentInParent<Animator>());
            }
        }
    }

    void ResetAllTriggers(Animator animator)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
    }
}
