using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] level1;
    public GameObject[] level2;
    public int currentLevel = 1;
    private int maxLevels = 2;
    public float levelDurationTime = 2f;

    private float timer = 0f;

    private void Start()
    {
        SwitchToLevel(currentLevel);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= levelDurationTime)
        {   
            
            timer = 0f;
            SwitchToNextLevel();
        }
    }

    public void SwitchToNextLevel()
    {
        int nextLevel = currentLevel + 1;
        
        if (nextLevel > maxLevels)
        {
            nextLevel = 1;
        }
        SwitchToLevel(nextLevel);
    }

    public void SwitchToLevel(int level)
    {
        // Disable all game objects
        
        foreach (GameObject obj in level1)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in level2)
        {
            obj.SetActive(false);
        }
        
        switch (level)
        {
            case 1:
                foreach (GameObject obj in level1)
                {
                    obj.SetActive(true);
                }
            break;
            case 2:
                foreach (GameObject obj in level2)
                {
                    obj.SetActive(true);
                }
            break;
            default:
            break;
        }
        
        currentLevel = level;
    }
}
