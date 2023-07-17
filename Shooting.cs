using UnityEngine;
using System;
using System.Collections.Generic;

namespace BNG
{
    public class Shooting : MonoBehaviour
    {
        // Define circle collider 2d object
        public GameObject target;
        // define sphere collider
        public SphereCollider sphereCollider;
        // public RaycastWeapon rifle;
        public RaycastWeapon pistol; // 1 pistol 5 rifle 1 shotgun
        public RaycastWeapon pistol2;
        public RaycastWeapon rifle;
        public RaycastWeapon rifle2;
        public RaycastWeapon rifle3;
        public RaycastWeapon rifle4;
        public RaycastWeapon rifle5;
        public RaycastWeapon rifle6;
        public RaycastWeapon shotgun;

        private ShootingData data;

        // Collision world position
        private Vector3 intersectionPoint;
        private Vector2 rayHitPos;
        // Define collider position
        private Vector2 colliderCenter;
        private Dictionary<string, RaycastWeapon> weapons;
        private string currentWeapon = "pistol";



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
                Debug.Log("Target dont have a tag!(Add a Tag to the object).");
                throw new Exception();
            }
            // Add weapons
            data = GetComponent<ShootingData>();
            weapons = new Dictionary<string, RaycastWeapon>();
            weapons.Add("pistol", pistol);
            weapons.Add("pistol2", pistol2);
            weapons.Add("rifle", rifle);
            weapons.Add("rifle2", rifle2);
            weapons.Add("rifle3", rifle3);
            weapons.Add("rifle4", rifle4);
            weapons.Add("rifle5", rifle5);
            weapons.Add("rifle6", rifle6);
            weapons.Add("shotgun", shotgun);
        }
        private string rayCast()
        {
            Ray ray = new Ray(weapons[currentWeapon].MuzzlePointTransform.position, weapons[currentWeapon].MuzzlePointTransform.forward);
            // Get ray position
            // Raycast from rifle muzzle
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // check if raycast hit the sphere collider
                SphereCollider collider = hit.collider.GetComponent<SphereCollider>();

                if (sphereCollider.tag == collider.tag)
                {
                    // Find hit point at the center of the sphere
                    Vector3 hitPoint = hit.collider.transform.position;
                    // Calculate the intersection point with the sphere's z position
                    float t = (hitPoint.z - ray.origin.z) / ray.direction.z;
                    intersectionPoint = ray.GetPoint(t);
                    return collider.tag;
                }
            }
            else
            {
                return "No Hit";
            }
            return "No Hit";
        }

        private float circleBoundaryPos(float x, float r, float offset)
        {
            float pos = Mathf.Sqrt((r * r) - (x * x)) + offset;
            return pos;
        }


        private void Score()
        {
            // Calculate magnitude of rayHitPos vector
            Vector2 absHitPos = new Vector2(Mathf.Abs(rayHitPos.x), Mathf.Abs(rayHitPos.y));
            // Score Logic
            point = 0;
            if (sphereCollider.tag == "HedefFX")
            {
                switch (absHitPos.x)
                {
                    case float n when (n >= 0 && n <= 0.048f):
                        if (absHitPos.y >= 0 && absHitPos.y <= 0.08f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.048f, 0.032f))
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
                        else if (absHitPos.y > 0.08f && absHitPos.y <= 0.16f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.096f, 0.064f))
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
                        else if (absHitPos.y > 0.16f && absHitPos.y <= 0.24f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.144f, 0.096f))
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
                        else if (absHitPos.y > 0.24f && absHitPos.y <= 0.32f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.192f, 0.128f))
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
                    case float n when (n > 0.048f && n <= 0.096f):
                        if (absHitPos.y >= 0 && absHitPos.y <= 0.16f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.096f, 0.064f))
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
                        else if (absHitPos.y > 0.16f && absHitPos.y <= 0.24f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.144f, 0.096f))
                            {
                                misses--;
                                point = 9;
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
                        else if (absHitPos.y > 0.24f && absHitPos.y <= 0.32f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.192f, 0.128f))
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
                    case float n when (n > 0.096f && n <= 0.144f): //14.4
                        if (absHitPos.y >= 0 && absHitPos.y <= 0.24f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.144f, 0.096f))
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
                        else if (absHitPos.y > 0.24f && absHitPos.y <= 0.32f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.192f, 0.128f))
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
                    case float n when (n > 0.144f && n <= 0.192f):
                        if (absHitPos.y >= 0 && absHitPos.y <= 0.32f)
                        {
                            if (absHitPos.y > circleBoundaryPos(absHitPos.x, 0.192f, 0.128f))
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
            else if (sphereCollider.tag == "Military target")
            {
                Debug.Log("Military target Collision!");
            }
        }
        // Update is called once per frame
        void Update()
        {
            float lastShotPistol = Time.time - weapons["pistol"].lastShotTime;
            float lastShotPistol2 = Time.time - weapons["pistol2"].lastShotTime;
            float lastShotRifle = Time.time - weapons["rifle"].lastShotTime; 
            float lastShotRifle2 = Time.time - weapons["rifle2"].lastShotTime;
            float lastShotRifle3 = Time.time - weapons["rifle3"].lastShotTime;
            float lastShotRifle4 = Time.time - weapons["rifle4"].lastShotTime;
            float lastShotRifle5 = Time.time - weapons["rifle5"].lastShotTime;
            float lastShotRifle6 = Time.time - weapons["rifle6"].lastShotTime;
            float lastShotShotgun = Time.time - weapons["shotgun"].lastShotTime;
            float[] lastshots = new float[] 
            { lastShotPistol, lastShotPistol2, lastShotRifle, lastShotRifle2, lastShotRifle3, lastShotRifle4, lastShotRifle5, lastShotRifle6, lastShotShotgun };
            
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
                default:
                    break;
            }

            // check the bullets shoot if there is any bullet left
            if (weapons[currentWeapon].BulletInChamber)
            {
                if (weapons[currentWeapon].readyToShoot && InputBridge.Instance.RightTrigger >= 0.75f)
                {
                    shotsFired++;                 
                    misses++;
                    float shotInterval = Time.timeScale < 1 ? weapons[currentWeapon].SlowMoRateOfFire : weapons[currentWeapon].FiringRate;
                    if (Time.time - weapons[currentWeapon].lastShotTime < shotInterval)
                    {
                        return;
                    }
                    string hit = rayCast();
                    if (hit == sphereCollider.tag)
                    {
                        rayHitPos = new Vector2(intersectionPoint.x - colliderCenter.x,
                        intersectionPoint.y - colliderCenter.y);

                        Score();
                        data.SaveData();                        
                    }
                }
            }

        }
    }
}
