using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using BNG;

public class Shooting : MonoBehaviour
{
    // public RaycastWeapon rifle;
    public BNG.RaycastWeapon pistol; // 1 pistol 5 rifle 1 shotgun
    public BNG.RaycastWeapon pistol2;
    public BNG.RaycastWeapon rifle;
    public BNG.RaycastWeapon rifle2;
    public BNG.RaycastWeapon rifle3;
    public BNG.RaycastWeapon rifle4;
    public BNG.RaycastWeapon rifle5;
    public BNG.RaycastWeapon rifle6;
    public BNG.RaycastWeapon rifle7;
    public BNG.RaycastWeapon shotgun;
    public List<GameObject> targets;
    public Dictionary<string, BNG.RaycastWeapon> weapons;
    public float reflexTime;
    public string currentWeapon = "pistol";
    // Shooting Data
    public int score
    {
        get { return _score; }
        set { _score += value; }
    }
    public int point = 0;
    public int misses = 0;
    public int shotsFired = 0;
    private int _score = 0;

    private GameObject currentTarget;
    private WeaponWheelSelect isWeaponSelected;

    
    // Collision world position
    private Vector3 intersectionPoint;
    private Vector2 rayHitPos;

    // Define collider position
    private Vector2 colliderCenter;
    

    private int hitID;
    private GameObject hitObj;
    private EmeraldAI.EmeraldAISystem zombie;
    
    // Start is called before the first frame update
    void Start()
    {
        // Add weapons
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
        weapons.Add("rifle7", rifle7);
        // Unit test for weapons
        if (weapons.Count == 0)
        {
            throw new Exception("No weapon found!(Add Weapons) Shooting.cs(76)");
        }
        isWeaponSelected = FindObjectOfType<WeaponWheelSelect>();
    }

    private void rayCast()
    {
        // Raycast from rifle muzzle
        Ray ray = new Ray(
            weapons[currentWeapon].MuzzlePointTransform.position,
            weapons[currentWeapon].MuzzlePointTransform.forward
        );
        // Get ray position
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // check if raycast hit the target
            hitObj = hit.collider.gameObject;
            hitID = hitObj.GetInstanceID();
            foreach (GameObject ele in targets)
            {
                if (ele.tag == "parcalanan")
                {
                    // get objects child gameobjects
                    GameObject[] childs = new GameObject[ele.transform.childCount];
                    for (int i = 0; i < ele.transform.childCount; i++)
                    {
                        childs[i] = ele.transform.GetChild(i).gameObject;
                    }
                    foreach (GameObject child in childs)
                    {
                        if (hitID == child.GetInstanceID())
                        {
                            currentTarget = child;
                            Debug.Log(currentTarget.tag);
                            break;
                        }
                    }
                }
                else if(ele.tag == "zombie")
                {
                    if(hitID == ele.GetInstanceID())
                    {
                        currentTarget = ele;
                        zombie = currentTarget.GetComponent<EmeraldAI.EmeraldAISystem>();
                        intersectionPoint = hit.point;
                        break;
                    }
                }
                else if (hitID == ele.GetInstanceID())
                {
                    currentTarget = ele;
                    // Find hit point at the center of the sphere
                    Vector3 hitPoint = hit.collider.transform.position;
                    // Calculate the intersection point with the sphere's z position
                    float t = (hitPoint.z - ray.origin.z) / ray.direction.z;
                    intersectionPoint = ray.GetPoint(t);
                    break;
                }
            }
        }
        else
        {
            hitID = 0;
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
        float scale_x = currentTarget.transform.localScale.z;
        float scale_y = currentTarget.transform.localScale.y;
        Vector2 absHitPos = new Vector2(Mathf.Abs(rayHitPos.x), Mathf.Abs(rayHitPos.y));
        Debug.Log("Hit Position: " + absHitPos);
        misses++;
        // Score Logic
        if (currentTarget.tag == "HedefFX")
        {
            switch (absHitPos.x)
            {
                case float n when (n >= 0 && n <= (0.048f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= 0.08f * scale_y)
                    {
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.048f * scale_y), (0.032f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.096f * scale_y), (0.064f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.144f * scale_y), (0.096f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.096f * scale_y), (0.064f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.144f * scale_y), (0.096f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.144f * scale_y), (0.096f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.192f * scale_y), (0.128f * scale_y))
                        )
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
        else if (currentTarget.tag == "move_01")
        {
            switch (absHitPos.x)
            {
                case float n when (n >= 0 && n <= (0.04f * scale_x)):
                    if (absHitPos.y >= 0 && absHitPos.y <= (0.06f * scale_y))
                    {
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.04f * scale_y), (0.02f * scale_y))
                        )
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
                                    if (
                                        absHitPos.y
                                        > circleBoundaryPos(
                                            absHitPos.x,
                                            (0.02f * scale_y),
                                            (0.01f * scale_y)
                                        )
                                    )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.08f * scale_y), (0.04f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.12f * scale_y), (0.06f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.08f * scale_y), (0.04f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.12f * scale_y), (0.06f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.12f * scale_y), (0.06f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y))
                        )
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
                        if (
                            absHitPos.y
                            > circleBoundaryPos(absHitPos.x, (0.16f * scale_y), (0.08f * scale_y))
                        )
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
        else if (currentTarget.tag == "top")
        {
            misses--;
            point = 10;
            score = 10;
            Debug.Log("Point: 10");
            Debug.Log("Score: " + score);
        }
        else if (currentTarget.tag == "Respawn")
        {
            misses--;
            point = 2;
            score = 2;
            Debug.Log("Point: 2");
            Debug.Log("Score: " + score);
        }
        else if (currentTarget.tag == "Head")
        {
            misses--;
            point = 10;
            score = 10;
            Debug.Log("Point: 10");
            Debug.Log("Score: " + score);
        }
        else if (currentTarget.tag == "Chest")
        {
            misses--;
            point = 8;
            score = 8;
            Debug.Log("Point: 8");
            Debug.Log("Score: " + score);
        }
        else if (currentTarget.tag == "Arm")
        {
            misses--;
            point = 4;
            score = 4;
            Debug.Log("Point: 4");
            Debug.Log("Score: " + score);
        }
        else if (currentTarget.tag == "Leg")
        {
            misses--;
            point = 6;
            score = 6;
            Debug.Log("Point: 6");
            Debug.Log("Score: " + score);
        }
        else if (currentTarget.tag == "soldier")
        {
            if (rayHitPos.y > 0.39f)
            {
                misses--;
                point = 10;
                score = 10;
                Debug.Log("Point: 10");
                Debug.Log("Score: " + score);
            }
            else if (rayHitPos.y > 0.10f)
            {
                if (absHitPos.x > 0f && absHitPos.x < 0.10f)
                {
                    misses--;
                    point = 10;
                    score = 10;
                    Debug.Log("Point: 10");
                    Debug.Log("Score: " + score);
                }
                else if (absHitPos.x > 0.10f && absHitPos.x < 0.18f)
                {
                    misses--;
                    point = 8;
                    score = 8;
                    Debug.Log("Point: 8");
                    Debug.Log("Score: " + score);
                }
                else if (absHitPos.x > 0.18f && absHitPos.x < 0.22f)
                {
                    misses--;
                    point = 6;
                    score = 6;
                    Debug.Log("Point: 6");
                    Debug.Log("Score: " + score);
                }
                else if (absHitPos.x > 0.22f && absHitPos.x < 0.39f)
                {
                    misses--;
                    point = 4;
                    score = 4;
                    Debug.Log("Point: 4");
                    Debug.Log("Score: " + score);
                }
            }
            else
            {
                if (absHitPos.x > 0f && absHitPos.x < 0.10f)
                {
                    misses--;
                    point = 8;
                    score = 8;
                    Debug.Log("Point: 8");
                    Debug.Log("Score: " + score);
                }
                else if (absHitPos.x > 0.10f && absHitPos.x < 0.18f)
                {
                    misses--;
                    point = 6;
                    score = 6;
                    Debug.Log("Point: 6");
                    Debug.Log("Score: " + score);
                }
                else if (absHitPos.x > 0.18f && absHitPos.x < 0.22f)
                {
                    misses--;
                    point = 4;
                    score = 4;
                    Debug.Log("Point: 4");
                    Debug.Log("Score: " + score);
                }
            }
        }
        else if(currentTarget.tag == "move_01Miss")
        {
            misses--;
            point = -10;
            score = -10;
            Debug.Log("Point: -10");
            Debug.Log("Score: " + score);
        }
        else if(currentTarget.tag == "zombie")
        {
            Debug.Log("Zombie Health: " + zombie.CurrentHealth);
            misses--;
            if(zombie.CurrentHealth <= 25)
            {
                point = 20;
                score = 20;
                Debug.Log("Point: 20");
                Debug.Log("Score: " + score);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        targets.Clear();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("HedefFX");
        GameObject[] taggedObjects1 = GameObject.FindGameObjectsWithTag("move_01");
        GameObject[] taggedObjects2 = GameObject.FindGameObjectsWithTag("top");
        GameObject[] taggedObjects3 = GameObject.FindGameObjectsWithTag("Respawn");
        GameObject[] taggedObjects4 = GameObject.FindGameObjectsWithTag("parcalanan");
        GameObject[] taggedObjects5 = GameObject.FindGameObjectsWithTag("soldier");
        GameObject[] taggedObjects6 = GameObject.FindGameObjectsWithTag("zombie");

        targets.AddRange(taggedObjects);
        targets.AddRange(taggedObjects1);
        targets.AddRange(taggedObjects2);
        targets.AddRange(taggedObjects3);
        targets.AddRange(taggedObjects4);
        targets.AddRange(taggedObjects5);
        targets.AddRange(taggedObjects6);
        point = 0;

        float lastShotPistol = Time.time - weapons["pistol"].lastShotTime;
        float lastShotPistol2 = Time.time - weapons["pistol2"].lastShotTime;
        float lastShotRifle = Time.time - weapons["rifle"].lastShotTime;
        float lastShotRifle2 = Time.time - weapons["rifle2"].lastShotTime;
        float lastShotRifle3 = Time.time - weapons["rifle3"].lastShotTime;
        float lastShotRifle4 = Time.time - weapons["rifle4"].lastShotTime;
        float lastShotRifle5 = Time.time - weapons["rifle5"].lastShotTime;
        float lastShotRifle6 = Time.time - weapons["rifle6"].lastShotTime;
        float lastShotRifle7 = Time.time - weapons["rifle7"].lastShotTime;
        float lastShotShotgun = Time.time - weapons["shotgun"].lastShotTime;
        float[] lastshots = new float[]
        {
            lastShotPistol,
            lastShotPistol2,
            lastShotRifle,
            lastShotRifle2,
            lastShotRifle3,
            lastShotRifle4,
            lastShotRifle5,
            lastShotRifle6,
            lastShotShotgun,
            lastShotRifle7
        };
        // Unit test for lastshots
        foreach (float lastshot in lastshots)
        {
            if (lastshot < 0)
            {
                Debug.LogError("Warning!!! Lastshot Time is less than 0 Shooting.cs(618)");
            }
        }
        // sort lastShot times
        Array.Sort(lastshots);
        switch (lastshots[0])
        {
            case float n when (n == lastShotPistol):
                currentWeapon = "pistol";
                break;
            case float n when (n == lastShotPistol2):
                currentWeapon = "pistol2";
                break;
            case float n when (n == lastShotRifle):
                currentWeapon = "rifle";
                break;
            case float n when (n == lastShotRifle2):
                currentWeapon = "rifle2";
                break;
            case float n when (n == lastShotRifle3):
                currentWeapon = "rifle3";
                break;
            case float n when (n == lastShotRifle4):
                currentWeapon = "rifle4";
                break;
            case float n when (n == lastShotRifle5):
                currentWeapon = "rifle5";
                break;
            case float n when (n == lastShotRifle6):
                currentWeapon = "rifle6";
                break;
            case float n when (n == lastShotShotgun):
                currentWeapon = "shotgun";
                break;
            case float n when (n == lastShotRifle7):
                currentWeapon = "rifle7";
                break;
            default:
                break;
        }

        // // check the bullets shoot if there is any bullet left and if holding a weapon
        if (isWeaponSelected.handGrabber.HeldGrabbable != null)
        {
            if (weapons[currentWeapon].BulletInChamber)
            {
                if (
                    weapons[currentWeapon].readyToShoot
                    && BNG.InputBridge.Instance.RightTrigger >= 0.75f
                )
                {
                    float shotInterval =
                        Time.timeScale < 1
                            ? weapons[currentWeapon].SlowMoRateOfFire
                            : weapons[currentWeapon].FiringRate;
                    if (Time.time - weapons[currentWeapon].lastShotTime < shotInterval)
                    {
                        return;
                    }
                    rayCast();
                    if (hitID == currentTarget.GetInstanceID())
                    {
                        if (
                            currentTarget.tag == "Head"
                            || currentTarget.tag == "Arm"
                            || currentTarget.tag == "Leg"
                            || currentTarget.tag == "Chest"
                        )
                        {
                            Score();
                        }
                        else if(currentTarget.tag == "zombie")
                        {
                            Debug.Log("Zombie Health: " + zombie.CurrentHealth);
                            BoxCollider collider = currentTarget.GetComponent<BoxCollider>();
                            colliderCenter = collider.bounds.center;
                            rayHitPos = new Vector2(
                                intersectionPoint.x - colliderCenter.x,
                                intersectionPoint.y - colliderCenter.y
                            );
                            Score();
                        }
                        else
                        {
                            SphereCollider collider = currentTarget.GetComponent<SphereCollider>();
                            colliderCenter = collider.bounds.center;
                            rayHitPos = new Vector2(
                                intersectionPoint.x - colliderCenter.x,
                                intersectionPoint.y - colliderCenter.y
                            );
                            Score();
                        }
                    }
                }
            }
        }
    }
}