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
        public List<int> point;
        public int misses;
        public int accuracy;
    }

    [Serializable]
    public struct UserDataListWrapper
    {
        public List<UserData> dataList;
    }

    private UserData data;
    private UserDataListWrapper dataListWrapper;
    private BNG.Shooting shooting;
    private string jsonData;
    private string path;
    private string readJson;
    private Login login;

    private void Start()
    {
        // Initialize Variables
        login = GetComponent<Login>();
        shooting = GetComponent<BNG.Shooting>();
        data = new UserData();
        dataListWrapper = new UserDataListWrapper();
        data.point = new List<int>();
        dataListWrapper.dataList = new List<UserData>();
        path = Application.dataPath + "/Data/data.json";
        readFile();
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
        data.userId = dataListWrapper.dataList.Count;
        if(login.userName.text != "" && login.userName.text != null)
        {
            data.name = login.userName.text;
        }
        else
        {
            data.name = "Player";
        }
        data.score = shooting.score;
        data.point.Add(shooting.point);
        data.misses = shooting.misses;
        if(shooting.shotsFired != 0)
        {
            data.accuracy = shooting.score / shooting.shotsFired;
        }
        else
        {
            data.accuracy = 0;
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

    private void OnApplicationQuit() {
        SaveData();
        SaveJson();
        saveFile();
        sortPlayers();
    }
}


