using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HostileCounter : MonoBehaviour
{
    private GameObject Player;

    private string Enemies = "Enemy";

    private TextMeshProUGUI HostileCounterText;

    private int startingEnemyCount;
    private int enemyCount;

    void Start(){
        Player = GameObject.Find("PlayerModel");

        HostileCounterText = GetComponent<TextMeshProUGUI>();
    }

    void Update(){
        if(Player.GetComponent<HealthStatus>().death){
            PlayerSubtractCount();
        }
        else{
            UpdateStartingCount();
            UpdateCount();
        }
    }

    void UpdateStartingCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Enemies);

        startingEnemyCount = enemies.Length;
    }

    void UpdateCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Enemies);

        GameObject[] levelTriggers = GameObject.FindGameObjectsWithTag("LevelTrigger");
        GameObject[] exitArrows = GameObject.FindGameObjectsWithTag("ExitArrow");

        enemyCount = enemies.Length;

        HostileCounterText.text = "Hostiles: " + enemyCount;

        foreach(GameObject enemy in enemies)
        {
            EnemyDeath component = enemy.GetComponent<EnemyDeath>();
            
            if(component.dead)
            {
                SubtractCount(enemy);
            }
        }

        if (enemyCount <= startingEnemyCount * 0.3f)
        {
            foreach (GameObject levelTrigger in levelTriggers)
            {
                levelTrigger.GetComponent<BoxCollider>().enabled = true;
                levelTrigger.GetComponent<MeshRenderer>().enabled = true;
            }

            foreach(GameObject exitArrow in exitArrows){
                exitArrow.GetComponent<ExitArrow>().enabled = true;
                exitArrow.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void SubtractCount(GameObject enemy)
    {
        enemyCount--;
        
        HostileCounterText.text = "Hostiles: " + enemyCount;
    }

    void PlayerSubtractCount()
    {
        HostileCounterText.text = "Hostiles: 0";
    }
}