using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class which controlls player aiming and shooting
/// </summary>
public class ShootingController : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The projectile to be fired.")]
    public GameObject projectilePrefab = null;
    [Tooltip("The transform in the heirarchy which holds projectiles if any")]
    public Transform projectileHolder = null;

    [Header("Input")]
    [Tooltip("Whether this shooting controller is controled by the player")]
    public bool isPlayerControlled = false;

    [Header("Firing Settings")]
    [Tooltip("The minimum time between projectiles being fired.")]
    public float fireRate = 0.05f;

    [Tooltip("The maximum diference between the direction the" +
        " shooting controller is facing and the direction projectiles are launched.")]
    public float projectileSpread = 1.0f;

    // The last time this component was fired
    private float lastFired = Mathf.NegativeInfinity;

    [Header("Effects")]
    [Tooltip("The effect to create when this fires")]
    public GameObject fireEffect;
	private int level =  1;

    //The input manager which manages player input
    private InputManager inputManager = null;
	public void setLevel(int lvl){
			if (lvl >3){
					lvl = 3;
			}
			level = lvl;
	}
	public void setFireRate(float rate){
			fireRate = rate;
	}
	public int getLevel(){ 
			return level;
	}
	public float getFireRate(){
			return this.fireRate;
	}


    /// <summary>
    /// Description:
    /// Standard unity function that runs every frame
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void Update()
    {
        ProcessInput();
    }

    /// <summary>
    /// Description:
    /// Standard unity function that runs when the script starts
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void Start()
    {
        SetupInput();
    }

    /// <summary>
    /// Description:
    /// Attempts to set up input if this script is player controlled and input is not already correctly set up 
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void SetupInput()
    {
        if (isPlayerControlled)
        {
            if (inputManager == null)
            {
                inputManager = InputManager.instance;
            }
            if (inputManager == null)
            {
                Debug.LogError("Player Shooting Controller can not find an InputManager in the scene, there needs to be one in the " +
                    "scene for it to run");
            }
        }
    }

    /// <summary>
    /// Description:
    /// Reads input from the input manager
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void ProcessInput()
    {
        if (isPlayerControlled)
        {
            if (inputManager.firePressed || inputManager.fireHeld)
            {
                Fire();
            }
        }   
    }

    /// <summary>
    /// Description:
    /// Fires a projectile if possible
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public void Fire()
    {
        // If the cooldown is over fire a projectile
        if ((Time.timeSinceLevelLoad - lastFired) > fireRate)
        {
            // Launches a projectile
            SpawnProjectile();

            if (fireEffect != null)
            {
                Instantiate(fireEffect, transform.position, transform.rotation, null);
            }

            // Restart the cooldown
            lastFired = Time.timeSinceLevelLoad;
        }
    }

    /// <summary>
    /// Description:
    /// Spawns a projectile and sets it up
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
	public GameObject CreateTrans(GameObject projec,float angle){
		Vector3 rotationEulerAngles = projec.transform.rotation.eulerAngles;
		rotationEulerAngles.z += angle;                                                       			
		projec.transform.rotation = Quaternion.Euler(rotationEulerAngles);
		return projec;
	}
    public void SpawnProjectile()
    {
        // Check that the prefab is valid
        if (projectilePrefab != null)
        {
            // Create the projectile
            GameObject projectileGameObject1 = Instantiate(projectilePrefab, transform.position, transform.rotation, null);
            GameObject projectileGameObject2 = Instantiate(projectilePrefab, transform.position, transform.rotation, null);
            GameObject projectileGameObject3 = Instantiate(projectilePrefab, transform.position, transform.rotation, null);
            GameObject projectileGameObject4 = Instantiate(projectilePrefab, transform.position, transform.rotation, null);
            GameObject projectileGameObject5 = Instantiate(projectilePrefab, transform.position, transform.rotation, null);
			if(level == 1){
					projectileGameObject1 = CreateTrans(projectileGameObject1,0);
			} else if (level == 2){
					projectileGameObject1 = CreateTrans(projectileGameObject1,10);
					projectileGameObject3 = CreateTrans(projectileGameObject3,0);
					projectileGameObject2 = CreateTrans(projectileGameObject2,-10);
			} else if (level == 3) {
					projectileGameObject5 = CreateTrans(projectileGameObject5,20);
					projectileGameObject1 = CreateTrans(projectileGameObject1,10);
					projectileGameObject3 = CreateTrans(projectileGameObject3,0);
					projectileGameObject2 = CreateTrans(projectileGameObject2,-10);
					projectileGameObject4 = CreateTrans(projectileGameObject4,-20);
			}

            // Keep the heirarchy organized
            if (projectileHolder != null)
            {
				if (level == 1){
                	projectileGameObject1.transform.SetParent(projectileHolder);
				}
				else if (level == 2){
                	projectileGameObject1.transform.SetParent(projectileHolder);
                	projectileGameObject2.transform.SetParent(projectileHolder);
                	projectileGameObject3.transform.SetParent(projectileHolder);
				} else if (level == 3) { 
                	projectileGameObject1.transform.SetParent(projectileHolder);
                	projectileGameObject2.transform.SetParent(projectileHolder);
                	projectileGameObject3.transform.SetParent(projectileHolder);
                	projectileGameObject4.transform.SetParent(projectileHolder);
                	projectileGameObject5.transform.SetParent(projectileHolder);
				}
            }
        }
    }
}
