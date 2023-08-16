using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ScanAttach : MonoBehaviour
{
    public Transform planeTransform;
    [SerializeField] protected Transform originPoint;
    [SerializeField] protected Transform rayPoint;
    public VisualEffect visFX;
    public Vector3 vectorToModify;
    private float goalZ, currentZ;

    [SerializeField] protected float ZMultiplier, snapSpeed, goalZOffset;

    private void Awake()
    {
        vectorToModify = visFX.GetVector3("Projectile Line Offset");
        currentZ = goalZ = 5;
    }
    private void Update()
    {
        AttachToNearestSurface();
    }

    private void AttachToNearestSurface()
    {
        Ray ray = new Ray(rayPoint.position, originPoint.forward); // Cast the ray in the forward direction of the originPoint
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);


        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f , LayerMask.GetMask("Default")))
        {
            //planeTransform.position = hitInfo.point;

            goalZ = originPoint.InverseTransformPoint(hitInfo.point).z + goalZOffset;

            //var pos = planeTransform.localPosition;
            //pos.x = pos.y = 0;
            //planeTransform.localPosition = pos;

            // Calculate the rotation to align the plane's up with the hit surface normal
            //Quaternion rotation = Quaternion.FromToRotation(planeTransform.up, hitInfo.normal) * planeTransform.rotation;
            //planeTransform.rotation = rotation;

            //Vector3 distanceDifference = planeTransform.position - originPoint.position;

            
            //visFX.SetVector3("Projectile Line Offset", new Vector3(5, 5, vectorToModify.z - (planeTransform.localPosition.z * ZMultiplier)));
        } else
        {
            Debug.Log("No hit");
            goalZ = 0;
            //visFX.SetVector3("Projectile Line Offset", new Vector3(5,5,5));
            //planeTransform.localPosition = Vector3.zero;
        }

        currentZ = Mathf.Lerp(currentZ, goalZ, Time.deltaTime * snapSpeed);
        visFX.SetVector3("Projectile Line Offset", new Vector3(5, 5, vectorToModify.z - currentZ * ZMultiplier));
        planeTransform.localPosition = new Vector3(0, 0, -currentZ);

    }

    //private void FixedUpdate()
    //{
    //    AttachToNearest();
    //}
    //
    //private void AttachToNearest()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(originPoint.position, Mathf.Infinity, LayerMask.GetMask("Default"));
    //
    //    Transform nearestObject = null;
    //    float nearestDistance = Mathf.Infinity;
    //
    //    foreach (Collider collider in colliders)
    //    {
    //        float distance = Vector3.Distance(originPoint.position, collider.transform.position);
    //        if (distance < nearestDistance)
    //        {
    //            nearestDistance = distance;
    //            nearestObject = collider.transform;
    //        }
    //    }
    //
    //    if (nearestObject != null)
    //    {
    //        planeTransform.position = nearestObject.position;
    //        Vector3 distanceDifference = planeTransform.position - originPoint.position;
    //        Vector3 vectorToModify = visFX.GetVector3("Projectile Line Offset");
    //        visFX.SetVector3("Projectile Line Offset", new Vector3(vectorToModify.x, vectorToModify.y, vectorToModify.z + distanceDifference.z));
    //        // Do something with the distanceDifference vector, e.g., modify another vector
    //    }
    //}
}

