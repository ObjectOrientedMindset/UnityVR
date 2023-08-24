using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.IO;

public class ShootingData : MonoBehaviour
{
    [Serializable]
    public struct UserData
    {
        public int placement;
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
        public float fiveSuccessRate;
        public float fourSuccessRate;
        public float threeSuccessRate;
        public float twoSuccessRate;
        public float oneSuccessRate;
        public float missSuccessRate;
        public float hitSuccessRate;
        public float totalSuccessRate;
        public float reflexTime;
        public float totalShotsFired;
        public string uniqueID;
    }

    [Serializable]
    public struct UserDataListWrapper
    {
        public List<UserData> dataList;
    }

    [Serializable]
    private class ResponseData
    {
        public string key;
    }

    [Tooltip("Check the box if the game is polygon")]
    public bool isPolygon = false;
    public UserData data;
    private UserDataListWrapper dataListWrapper;
    private string path;
    string relativePath;
    private string jsonData;
    private string readJson;
    public Shooting shooting;
    private WeaponWheelSelect isWeaponSelected;
    private List<float> reflexTimeList;
    private string postURL = "http://localhost:5000/data";
    private string API_KEY = "Pd64/3+<@%2*Z&+dsaW332Fc-21";
    private string registerURL = "http://localhost:5000/register";
    private bool isMakingPostRequest = false;
    private string gameKey;
    private Guid uniqueId;

    private void Start()
    {
        // Initialize Variables
        data = new UserData();
        dataListWrapper = new UserDataListWrapper();
        relativePath = "Data/data.json";
        path = Path.Combine(Application.dataPath, relativePath);
        data.points = new List<int>();
        data.totalShotsFired = 0;
        reflexTimeList = new List<float>();
        isWeaponSelected = FindObjectOfType<WeaponWheelSelect>();
        if (isPolygon)
        {
            postURL = "http://localhost:5000/polygon";
        }
        data.name = PlayerPrefs.GetString("PlayerName", "DefaultName"); // Oyuncu adını al, eğer yoksa "DefaultName" kullan
        uniqueId = Guid.NewGuid();
        data.uniqueID = uniqueId.ToString();
        data.placement = 0;
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
        else
        {
            dataListWrapper.dataList = new List<UserData>();
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

    private IEnumerator MakePostRequest()
    {
        // Create a sample JSON data to send in the POST request
        string jsonData = JsonUtility.ToJson(data);

        // Create a byte array from the JSON data
        byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        yield return StartCoroutine(RegisterGame()); // Wait for RegisterGame coroutine to complete

        if (gameKey != null)
        {
            // Set the content type header to application/json
            var requestHeader = new Dictionary<string, string>();
            requestHeader.Add("Content-Type", "application/json");
            requestHeader.Add("x-game-key", gameKey); // Use the key obtained from registration

            using (UnityWebRequest webRequest = UnityWebRequest.Post(postURL, jsonData))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bodyData);
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                // Set the request headers
                foreach (var entry in requestHeader)
                {
                    webRequest.SetRequestHeader(entry.Key, entry.Value);
                }

                // Send the request and wait for a response
                yield return webRequest.SendWebRequest();

                // Check for errors
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error while making request: " + webRequest.error);
                    yield break;
                }
                else
                {
                    if (dataListWrapper.dataList.Count != 0)
                    {
                        // If the request is successful, send all elements in dataList
                        foreach (var element in dataListWrapper.dataList)
                        {
                            string elementJsonData = JsonUtility.ToJson(element);
                            byte[] elementBodyData = System.Text.Encoding.UTF8.GetBytes(
                                elementJsonData
                            );
                            yield return StartCoroutine(RegisterGame()); // Wait for RegisterGame coroutine to complete
                            // Set the content type header to application/json
                            var Header = new Dictionary<string, string>();
                            Header.Add("Content-Type", "application/json");
                            Header.Add("x-game-key", gameKey); // Use the key obtained from registration
                            using (
                                UnityWebRequest elementWebRequest = UnityWebRequest.Post(
                                    postURL,
                                    elementJsonData
                                )
                            )
                            {
                                elementWebRequest.uploadHandler = new UploadHandlerRaw(
                                    elementBodyData
                                );
                                elementWebRequest.downloadHandler = new DownloadHandlerBuffer();

                                // Set the request headers
                                foreach (var entry in Header)
                                {
                                    elementWebRequest.SetRequestHeader(entry.Key, entry.Value);
                                }

                                // Send the request
                                yield return elementWebRequest.SendWebRequest();

                                if (elementWebRequest.result != UnityWebRequest.Result.Success)
                                {
                                    Debug.LogError(
                                        "Error while making request: " + elementWebRequest.error
                                    );
                                    yield break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No data to send");
                    }

                    // Clear dataList after all elements are sent
                    dataListWrapper.dataList.Clear();
                    saveFile();
                }

                // Process the response
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
        }
        else
        {
            dataListWrapper.dataList.Add(data);
            saveFile();
        }
    }

    private IEnumerator RegisterGame()
    {
        var requestHeader = new Dictionary<string, string>();
        requestHeader.Add("Content-Type", "application/x-www-form-urlencoded");
        requestHeader.Add("Authorization", API_KEY); // Use the key obtained from registration
        using (UnityWebRequest webRequest = UnityWebRequest.Get(registerURL))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // Set the request headers
            foreach (var entry in requestHeader)
            {
                webRequest.SetRequestHeader(entry.Key, entry.Value);
            }

            Debug.Log("Registering game...");
            yield return webRequest.SendWebRequest();
            try
            {
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error while registering game: " + webRequest.error);
                    // Handle the error here, e.g., return an error state or message
                    yield break;
                }

                // Parse the response JSON to get the key
                string response = webRequest.downloadHandler.text;
                gameKey = response;

                Debug.Log("Registered successfully! Game key: " + gameKey);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while processing response: " + e.Message);
                // Handle the exception here, e.g., return an error state or message
            }
        }
    }

    public void SaveData()
    {
        data.tenSuccessRate = 0;
        data.nineSuccessRate = 0;
        data.eightSuccessRate = 0;
        data.sevenSuccessRate = 0;
        data.sixSuccessRate = 0;
        data.fiveSuccessRate = 0;
        data.fourSuccessRate = 0;
        data.threeSuccessRate = 0;
        data.twoSuccessRate = 0;
        data.oneSuccessRate = 0;

        if (shooting.point != 0)
        {
            data.points.Add(shooting.point);
        }
        data.score = shooting.score;

        if (shooting.reflexTime != 0)
        {
            // find the average of reflex time List
            reflexTimeList.Add(shooting.reflexTime);
            data.reflexTime = reflexTimeList.Average();
        }

        if (data.totalShotsFired != 0)
        {
            data.misses = (int)(data.totalShotsFired - data.points.Count);
            // throw exception if misses is negative

            if (data.misses < 0)
                throw new Exception(("Error!!! Misses is less than 0 (ShootingData.cs)"));
            data.accuracy = (float)data.score / (float)data.totalShotsFired * 100f;
            data.missSuccessRate =
                (float)(data.totalShotsFired - data.misses) / (float)data.totalShotsFired * 100f;
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
                    case 5:
                        data.fiveSuccessRate++;
                        break;
                    case 4:
                        data.fourSuccessRate++;
                        break;
                    case 3:
                        data.threeSuccessRate++;
                        break;
                    case 2:
                        data.twoSuccessRate++;
                        break;
                    case 1:
                        data.oneSuccessRate++;
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
            data.fiveSuccessRate = data.fiveSuccessRate / (float)data.points.Count * 100f;
            data.fourSuccessRate = data.fourSuccessRate / (float)data.points.Count * 100f;
            data.threeSuccessRate = data.threeSuccessRate / (float)data.points.Count * 100f;
            data.twoSuccessRate = data.twoSuccessRate / (float)data.points.Count * 100f;
            data.oneSuccessRate = data.oneSuccessRate / (float)data.points.Count * 100f;

            data.hitSuccessRate =
                data.tenSuccessRate
                + (data.nineSuccessRate * 0.9f)
                + (data.eightSuccessRate * 0.8f)
                + (data.sevenSuccessRate * 0.7f)
                + (data.sixSuccessRate * 0.6f)
                + (data.fiveSuccessRate * 0.5f)
                + (data.fourSuccessRate * 0.4f)
                + (data.threeSuccessRate * 0.3f)
                + (data.twoSuccessRate * 0.2f)
                + (data.oneSuccessRate * 0.1f);
            // Total Success Rate is the average of hit success rate and miss success rate
            data.totalSuccessRate = (data.hitSuccessRate * data.missSuccessRate) / 100f;
        }
        else
        {
            data.accuracy = 0;
            data.missSuccessRate = 0;
            data.misses = 0;
        }
    }

    private void Update()
    {
        // check the bullets shoot if there is any bullet left
        if (isWeaponSelected.handGrabber.HeldGrabbable != null)
        {
            if (shooting.weapons[shooting.currentWeapon].BulletInChamber)
            {
                if (
                    shooting.weapons[shooting.currentWeapon].readyToShoot
                    && BNG.InputBridge.Instance.RightTrigger >= 0.75f
                )
                {
                    float shotInterval =
                        Time.timeScale < 1
                            ? shooting.weapons[shooting.currentWeapon].SlowMoRateOfFire
                            : shooting.weapons[shooting.currentWeapon].FiringRate;
                    if (
                        Time.time - shooting.weapons[shooting.currentWeapon].lastShotTime
                        < shotInterval
                    )
                    {
                        return;
                    }
                    data.totalShotsFired++;
                    SaveData();
                }
            }
        }
    }

    public IEnumerator sceneDatLoad()
    {
        SaveData();
        if (!isMakingPostRequest)
        {
            isMakingPostRequest = true;
            yield return StartCoroutine(MakePostRequest());
        }
    }
}
