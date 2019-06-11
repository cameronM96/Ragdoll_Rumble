using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSO : DeliverySO
{
    public GameObject templateObj;
    public int numberOfProjectiles = 1;
    public bool staggerActive = false;
    public int staggerGroupNumb = 1;
    public float staggerAmount = 1f;

    public bool constSpeed = true;
    public bool gravity = false;

    public Vector3 direction;
    public float speed = 5;

    public bool multiTargets = false;
    public int maxTargetsHit = 0;

    protected GameObject[] projectileArray;
    private List<Transform> launchPoints;
    private int currentProjectileIndex = 0;

    public override void ApplyDelivery(GameObject target)
    {
        base.ApplyDelivery(target);

        projectileArray = new GameObject[numberOfProjectiles];
        if (ability.item != null)
        {
            launchPoints.Clear();
            GetLaunchPoints(ability.item.transform);
        }

        if (launchPoints.Count > 0)
        {
            Transform[] launchPointArray = launchPoints.ToArray();
            int j = 0;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                projectileArray[i] = Instantiate(templateObj, launchPointArray[j].position, launchPointArray[j].rotation);

                if (j >= launchPointArray.Length - 1)
                    j = 0;
                else
                    j++;
            }
        }

        currentProjectileIndex = 0;
    }

    public override void Initialise(GameObject targetObject)
    {
        base.Initialise(targetObject);

        Projectile projectile = targetObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.constSpeed = constSpeed;
            projectile.gravity = gravity;

            projectile.direction = direction;
            projectile.speed = speed;

            projectile.multiTargets = multiTargets;
            projectile.maxTargetsHit = maxTargetsHit;

            if (staggerActive)
            {
                // Figure out how delayed this projectile will be after being triggered
            }
            else
                projectile.holdDelayTimer = 0;
        }
        else
            Debug.Log("Failed to find Projectile!");
    }

    private void GetLaunchPoints (Transform parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == "LaunchPoint")
                launchPoints.Add(child);

            if (child.childCount > 0)
                GetLaunchPoints(child);
        }
    }
}
