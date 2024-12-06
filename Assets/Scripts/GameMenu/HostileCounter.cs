using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HostileCounter : MonoBehaviour
{
    private int startingEnemyCount;
    private int enemyCount;

    private GameObject[] levelTriggers;
    private GameObject[] exitArrows;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeReferences();
    }

    void Update()
    {
        UpdateEnemyCount();

        if (enemyCount <= 3)
        {
            SetTriggersAndArrows(true);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        levelTriggers = GameObject.FindGameObjectsWithTag("LevelTrigger");
        exitArrows = GameObject.FindGameObjectsWithTag("ExitArrow");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        startingEnemyCount = enemies.Length;
        enemyCount = startingEnemyCount;

        UpdateHostileCounterText();
        SetTriggersAndArrows(false);
    }

    private void UpdateEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        enemyCount = 0;

        foreach (GameObject enemy in enemies)
        {
            EnemyDeath enemyDeath = enemy.GetComponent<EnemyDeath>();
            if (!enemyDeath.dead)
            {
                enemyCount++;
            }
        }

        UpdateHostileCounterText();
    }

    private void UpdateHostileCounterText()
    {
        TextMeshProUGUI counterText = GetComponent<TextMeshProUGUI>();

        counterText.text = $"Hostiles: {enemyCount}";
    }

    private void SetTriggersAndArrows(bool isActive)
    {
        foreach (GameObject trigger in levelTriggers)
        {
            BoxCollider collider = trigger.GetComponent<BoxCollider>();
            MeshRenderer renderer = trigger.GetComponent<MeshRenderer>();

            collider.enabled = isActive;
            renderer.enabled = isActive;
        }

        foreach (GameObject arrow in exitArrows)
        {
            ExitArrow arrowComponent = arrow.GetComponent<ExitArrow>();
            MeshRenderer renderer = arrow.GetComponent<MeshRenderer>();

            arrowComponent.enabled = isActive;
            renderer.enabled = isActive;
        }
    }
}