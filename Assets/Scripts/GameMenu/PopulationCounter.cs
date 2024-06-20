using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationCounter : MonoBehaviour
{
    private string Enemies = "Enemy";
    private string Casuals = "Casual";

    private TextMeshProUGUI PopulationCounterText;

    private int populationCount;

    void Update(){
        PopulationCounterText = GetComponent<TextMeshProUGUI>();

        UpdateCount();
    }

    void UpdateCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Enemies);
        GameObject[] casuals = GameObject.FindGameObjectsWithTag(Casuals);

        populationCount = enemies.Length + casuals.Length;

        PopulationCounterText.text = "Population: " + populationCount;

        foreach(GameObject enemy in enemies)
        {
            EnemyDeath component = enemy.GetComponent<EnemyDeath>();
            
            if(component.dead)
            {
                SubtractCount();
            }
        }

        foreach(GameObject casual in casuals)
        {
            EnemyDeath component = casual.GetComponent<EnemyDeath>();
            
            if(component.dead)
            {
                SubtractCount();
            }
        }
    }

    void SubtractCount()
    {
        populationCount--;
        
        PopulationCounterText.text = "Population: " + populationCount;
    }
}