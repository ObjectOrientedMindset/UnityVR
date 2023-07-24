using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootingData : MonoBehaviour
{
  [Serializable]
  public struct UserData
  {
    public int userId;
    public string name;
    public int score;
    public List<int> points;
    public int misses;
    public float accuracy;
    public float tenSuccessRate;
    public float nineSuccessRate;
    public float eightSuccessRate;
    public float sevenSuccessRate;
    public float sixSuccessRate;
    public float missSuccessRate;
    public float hitSuccessRate;
    public float totalSuccessRate;
  }

  [Serializable]
  public struct UserDataListWrapper
  {
    public List<UserData> dataList;
  }

  private UserData data;
  private UserDataListWrapper dataListWrapper;
  private Shooting[] shootingList;
  private string jsonData;
  private string path;
  private string readJson;
  private Login login;
  private List<int> totalPoint;
  private int totalShotsFired;

  private void Start()
  {
    // Initialize Variables
    shootingList = FindObjectsOfType<Shooting>();
    login = FindObjectOfType<Login>();
    data = new UserData();
    dataListWrapper = new UserDataListWrapper();
    data.points = new List<int>();
    dataListWrapper.dataList = new List<UserData>();
    path = Application.dataPath + "/Data/data.json";
    readFile();
    totalPoint = new List<int>();
    totalShotsFired = 0;
  }
  private void readFile()
  {
    // Read from data.json
    readJson = System.IO.File.ReadAllText(path);
    if (readJson != "")
    {
      UserDataListWrapper json = JsonUtility.FromJson<UserDataListWrapper>(readJson);
      dataListWrapper = json;
    }
  }
  private void saveFile()
  {
    // Write to data.json
    try
    {
      // Write the JSON string to the file
      jsonData = JsonUtility.ToJson(dataListWrapper);
      System.IO.File.WriteAllText(path, jsonData);
      Debug.Log("JSON file created: " + path);
    }
    catch (System.IO.IOException ex)
    {
      Debug.LogError("Failed to write JSON file: " + path + "\nError: " + ex.Message);
    }
  }

  public void SaveData()
  {

    int totalScore = 0;
    data.userId = dataListWrapper.dataList.Count;
    data.tenSuccessRate = 0;
    data.nineSuccessRate = 0;
    data.eightSuccessRate = 0;
    data.sevenSuccessRate = 0;
    data.sixSuccessRate = 0;

    foreach (Shooting shooting in shootingList)
    {
      totalScore += shooting.score;
      if (shooting.point != 0)
      {
        data.points.Add(shooting.point);
      }
    }
    data.score = totalScore;

    if (totalShotsFired != 0)
    {
      data.misses = totalShotsFired - data.points.Count;
      // throw exception if misses is negative

      if (data.misses < 0) throw new Exception(("Error!!! Misses is less than 0 (ShootingData.cs)"));
      data.accuracy = (float)totalScore / (float)totalShotsFired;
      data.missSuccessRate = (float)(totalShotsFired - data.misses) / (float)totalShotsFired * 100f;
      // Target Hit Frequency
      foreach (int point in data.points)
      {
        switch (point)
        {
          case 10:
            data.tenSuccessRate++;
            break;
          case 9:
            data.nineSuccessRate++;
            break;
          case 8:
            data.eightSuccessRate++;
            break;
          case 7:
            data.sevenSuccessRate++;
            break;
          case 6:
            data.sixSuccessRate++;
            break;
          default:
            break;
        }
      }
      // Get the hit frequency percentage
      data.tenSuccessRate = data.tenSuccessRate / (float)data.points.Count * 100f;
      data.nineSuccessRate = data.nineSuccessRate / (float)data.points.Count * 100f;
      data.eightSuccessRate = data.eightSuccessRate / (float)data.points.Count * 100f;
      data.sevenSuccessRate = data.sevenSuccessRate / (float)data.points.Count * 100f;
      data.sixSuccessRate = data.sixSuccessRate / (float)data.points.Count * 100f;

      data.hitSuccessRate = data.tenSuccessRate + (data.nineSuccessRate * 0.9f) +
      (data.eightSuccessRate * 0.8f) + (data.sevenSuccessRate * 0.7f) + (data.sixSuccessRate * 0.6f);
      // Total Success Rate is the average of hit success rate and miss success rate
      data.totalSuccessRate = (data.hitSuccessRate + data.missSuccessRate) / 2f;
    }
    else
    {
      data.accuracy = 0;
      data.missSuccessRate = 0;
      data.misses = 0;
    }
  }

  private void SaveJson()
  {
    dataListWrapper.dataList.Add(data);
  }

  private void sortPlayers()
  {
    //Sort players according to the accuracy
    dataListWrapper.dataList.Sort((x, y) => y.accuracy.CompareTo(x.accuracy));
    //Print Sorted list
    foreach (UserData data in dataListWrapper.dataList)
    {
      Debug.Log("Name: " + data.name + data.userId + " Score: " + data.score + " Accuracy: " + data.accuracy);
    }

  }

  private void Update()
  {
    // check the bullets shoot if there is any bullet left
    if (shootingList[0].weapons[shootingList[0].currentWeapon].BulletInChamber)
    {
      if (shootingList[0].weapons[shootingList[0].currentWeapon].readyToShoot && BNG.InputBridge.Instance.RightTrigger >= 0.75f)
      {
        float shotInterval = Time.timeScale < 1 ? shootingList[0].weapons[shootingList[0].currentWeapon].SlowMoRateOfFire : shootingList[0].weapons[shootingList[0].currentWeapon].FiringRate;
        if (Time.time - shootingList[0].weapons[shootingList[0].currentWeapon].lastShotTime < shotInterval)
        {
          return;
        }
        totalShotsFired++;
      }
    }
  }

  private void OnApplicationQuit()
  {
    SaveData();
    SaveJson();
    saveFile();
    // sortPlayers();
  }
}


