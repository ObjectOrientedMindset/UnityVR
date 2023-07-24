using UnityEngine;
using System;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    // Define circle collider 2d object
    public GameObject target;

    // define sphere collider
    public SphereCollider sphereCollider;

    // public RaycastWeapon rifle;
    public BNG.RaycastWeapon pistol; // 1 pistol 5 rifle 1 shotgun
    public BNG.RaycastWeapon pistol2;
    public BNG.RaycastWeapon rifle;
    public BNG.RaycastWeapon rifle2;
    public BNG.RaycastWeapon rifle3;
    public BNG.RaycastWeapon rifle4;
    public BNG.RaycastWeapon rifle5;
    public BNG.RaycastWeapon rifle6;
    public BNG.RaycastWeapon shotgun;
    public Camera cam;
    // Collision world position
    private Vector3 intersectionPoint;
    private Vector2 rayHitPos;
    private ShootingData data;

    // Define collider position
    private Vector2 colliderCenter;
    public Dictionary<string, BNG.RaycastWeapon> weapons;
    public string currentWeapon = "pistol";
    private int hitID;
    private int targetID;
    private GameObject hitObj;

    // Shooting Data
    public int _score = 0;

    public int score
    {
        get { return _score; }
        set { _score += value; }
    }
    public int point = 0;
    public int misses = 0;
    public int shotsFired = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (target.tag != "Untagged")
        {
            // get collider center
            colliderCenter = sphereCollider.bounds.center;
        }
        else
        {
            throw new Exception("Target dont have a tag!(Add a Tag to the object) Shooting.cs(59)");
        }
        // Add weapons
        data = FindObjectOfType<ShootingData>();
        weapons = new Dictionary<string, BNG.RaycastWeapon>();
        weapons.Add("pistol", pistol);
        weapons.Add("pistol2", pistol2);
        weapons.Add("rifle", rifle);
        weapons.Add("rifle2", rifle2);
        weapons.Add("rifle3", rifle3);
        weapons.Add("rifle4", rifle4);
        weapons.Add("rifle5", rifle5);
        weapons.Add("rifle6", rifle6);
        weapons.Add("shotgun", shotgun);
        // Unit test for weapons
        if (weapons.Count == 0)
        {
            throw new Exception("No weapon found!(Add Weapons) Shooting.cs(76)");
        }
    }

    private void rayCast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        // Raycast from rifle muzzle
        //Ray ray = new Ray(weapons[currentWeapon].MuzzlePointTransform.position, weapons[currentWeapon].MuzzlePointTransform.forward);
        // Get ray position
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // check if raycast hit the target
            hitObj = hit.collider.gameObject;
            hitID = hitObj.GetInstanceID();
            targetID = target.GetInstanceID();
            if (hitID == targetID)
            {
                // Find hit point at the center of the sphere
                Vector3 hitPoint = hit.collider.transform.position;
                // Calculate the intersection point with the sphere's z position
                float t = (hitPoint.z - ray.origin.z) / ray.direction.z;
                intersectionPoint = ray.GetPoint(t);
            }
        }
        // Unit test raycast
        if (hitObj == null)
        {
            Debug.LogError("No object found! Shooting.cs(103)");
        }
        
    }

    // Calculate circle boundary position
    private float circleBoundaryPos(float x, float r, float offset)
    {
        // Unit test for circleBoundaryPos
        if (x > r)
        {
            Debug.LogError("x is greater than r! Shooting.cs(114)");
        }
        float pos = Mathf.Sqrt((r * r) - (x * x)) + offset;
        return pos;
    }

    private void Score()
    {
        float scale_x = target.transform.localScale.z;
        float scale_y = target.transform.localScale.y;
        // Calculate magnitude of rayHitPos vector
        Vector2 absHitPos = new Vector2(Mathf.Abs(rayHitPos.x), Mathf.Abs(rayHitPos.y));
        // Debug.Log("Hit Position: " + absHitPos);
        misses++; 
        // Score Logic        
        if (sphereCollider.tag == "HedefFX")
        {
            switch (absHitPos.x)
            {
                case float n when (n >= 0 && n <= (0.048f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= 0.08f * scale_y)
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.048f * scale_y), (0.032f * scale_y)))
                        {
                            misses--;
                            point = 9;
                            score = 9;
                            Debug.Log("Point: 9");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 10;
                            score = 10;
                            Debug.Log("Point: 10");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.08f * scale_y) && absHitPos.y <= (0.16f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.096f * scale_y), (0.064f * scale_y)))
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 9;
                            score = 9;
                            Debug.Log("Point: 9");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.16f * scale_y) && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.144f * scale_y), (0.096f * scale_y)))
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.24f * scale_y) && absHitPos.y <= (0.32f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                case float n when (n > (0.048f * scale_x) && n <= (0.096f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.16f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.096f * scale_y), (0.064f* scale_y)))
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 9;
                            score = 9;
                            Debug.Log("Point: 9");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.16f * scale_y) && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.144f * scale_y), (0.096f * scale_y)))
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.24f * scale_y) && absHitPos.y <= (0.32f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                case float n when (n > (0.096f * scale_x) && n <= (0.144f * scale_x)): //14.4
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.144f * scale_y), (0.096f * scale_y)))
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.24f * scale_y) && absHitPos.y <= (0.32f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                case float n when (n > (0.144f * scale_x) && n <= (0.192f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.32f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f* scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                default:
                    Debug.Log("Collision out of range!");
                    break;
            }
        }
        else if (sphereCollider.tag == "move_01")
        {
            switch (absHitPos.x)
            {
                case float n when (n >= 0 && n <= (0.04f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.06f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.04f * scale_y), (0.02f * scale_y)))
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            if (n >= 0 && n <= (0.02f * scale_x))
                            {
                                if (absHitPos.y >= 0 && absHitPos.y <= (0.03f * scale_y))
                                {
                                    if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.02f * scale_y), (0.01f * scale_y)))
                                    {
                                        misses--;
                                        point = 9;
                                        score = 9;
                                        Debug.Log("Point: 9");
                                        Debug.Log("Score: " + score);
                                        break;
                                    }
                                    else
                                    {
                                        misses--;
                                        point = 10;
                                        score = 10;
                                        Debug.Log("Point: 10");
                                        Debug.Log("Score: " + score);
                                        break;
                                    }
                                }
                                else
                                {
                                    misses--;
                                    point = 9;
                                    score = 9;
                                    Debug.Log("Point: 9");
                                    Debug.Log("Score: " + score);
                                    break;
                                }
                            }
                            else
                            {
                                misses--;
                                point = 9;
                                score = 9;
                                Debug.Log("Point: 9");
                                Debug.Log("Score: " + score);
                                break;
                            }
                        }
                    }
                    else if (absHitPos.y > (0.06f * scale_y) && absHitPos.y <= (0.12f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.08f * scale_y), (0.04f * scale_y)))
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.12f * scale_y) && absHitPos.y <= (0.18f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.12f * scale_y), (0.06f* scale_y)))
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.18f * scale_y) && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                case float n when (n > (0.04f * scale_x) && n <= (0.08f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.12f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.08f * scale_y), (0.04f * scale_y)))
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 8;
                            score = 8;
                            Debug.Log("Point: 8");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > 0.12f && absHitPos.y <= (0.18f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.12f * scale_y), (0.06f * scale_y)))
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.18f * scale_y) && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                case float n when (n > (0.08f * scale_x) && n <= (0.12f * scale_x)): //14.4
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.18f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.12f * scale_y), (0.06f * scale_y)))
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 7;
                            score = 7;
                            Debug.Log("Point: 7");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    else if (absHitPos.y > (0.18f * scale_y) && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                case float n when (n > (0.12f * scale_x) && n <= (0.16f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.24f * scale_y))
                    {
                        if (absHitPos.y > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y)))
                        {
                            Debug.Log("Collision out of range!");
                            break;
                        }
                        else
                        {
                            misses--;
                            point = 6;
                            score = 6;
                            Debug.Log("Point: 6");
                            Debug.Log("Score: " + score);
                            break;
                        }
                    }
                    break;
                default:
                    Debug.Log("Collision out of range!");
                    break;
            }
            if (absHitPos.y > (0.30f * scale_y))
            {
                misses--;
                point = 10;
                score = 10;
                Debug.Log("Point: 10");
                Debug.Log("Score: " + score);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        colliderCenter = sphereCollider.bounds.center;
        point = 0;
        // get mouse button down
        if(Input.GetMouseButtonDown(0))
        {
            rayCast();
            if (hitID == targetID)
            {  
                rayHitPos = new Vector2(intersectionPoint.x - colliderCenter.x,
                intersectionPoint.y - colliderCenter.y);
                Score();
                data.SaveData();
            }
        }      
        // float lastShotPistol = Time.time - weapons["pistol"].lastShotTime;
        // float lastShotPistol2 = Time.time - weapons["pistol2"].lastShotTime;
        // float lastShotRifle = Time.time - weapons["rifle"].lastShotTime;
        // float lastShotRifle2 = Time.time - weapons["rifle2"].lastShotTime;
        // float lastShotRifle3 = Time.time - weapons["rifle3"].lastShotTime;
        // float lastShotRifle4 = Time.time - weapons["rifle4"].lastShotTime;
        // float lastShotRifle5 = Time.time - weapons["rifle5"].lastShotTime;
        // float lastShotRifle6 = Time.time - weapons["rifle6"].lastShotTime;
        // float lastShotShotgun = Time.time - weapons["shotgun"].lastShotTime;
        // float[] lastshots = new float[]
        // { lastShotPistol, lastShotPistol2, lastShotRifle, lastShotRifle2, lastShotRifle3, lastShotRifle4, lastShotRifle5, lastShotRifle6, lastShotShotgun };
        // // Unit test for lastshots
        // foreach (float lastshot in lastshots)
        // {
        //     if(lastshot < 0)
        //     {
        //         Debug.LogError("Warning!!! Lastshot Time is less than 0 Shooting.cs(618)");
        //     }
        // }
        // // sort lastShot times
        // Array.Sort(lastshots);
        // switch (lastshots[0])
        // {
        //     case float n when (n == lastShotPistol):
        //         currentWeapon = "pistol";
        //         break;
        //     case float n when (n == lastShotPistol2):
        //         currentWeapon = "pistol2";
        //         break;
        //     case float n when (n == lastShotRifle):
        //         currentWeapon = "rifle";
        //         break;
        //     case float n when (n == lastShotRifle2):
        //         currentWeapon = "rifle2";
        //         break;
        //     case float n when (n == lastShotRifle3):
        //         currentWeapon = "rifle3";
        //         break;
        //     case float n when (n == lastShotRifle4):
        //         currentWeapon = "rifle4";
        //         break;
        //     case float n when (n == lastShotRifle5):
        //         currentWeapon = "rifle5";
        //         break;
        //     case float n when (n == lastShotRifle6):
        //         currentWeapon = "rifle6";
        //         break;
        //     case float n when (n == lastShotShotgun):
        //         currentWeapon = "shotgun";
        //         break;
        //     default:
        //         break;
        // }

        // // check the bullets shoot if there is any bullet left
        // if (weapons[currentWeapon].BulletInChamber)
        // {
        //     if (weapons[currentWeapon].readyToShoot && BNG.InputBridge.Instance.RightTrigger >= 0.75f)
        //     {
        //         float shotInterval = Time.timeScale < 1 ? weapons[currentWeapon].SlowMoRateOfFire : weapons[currentWeapon].FiringRate;
        //         if (Time.time - weapons[currentWeapon].lastShotTime < shotInterval)
        //         {
        //             return;
        //         }
        //         rayCast();
        //         if (hitID == targetID)
        //         {
        //             rayHitPos = new Vector2(intersectionPoint.x - colliderCenter.x,
        //             intersectionPoint.y - colliderCenter.y);
        //             Score();
        //         }

        //     }
        // }
    }
}
