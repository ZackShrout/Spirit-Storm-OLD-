using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Tilemap theMap;
    private Vector3 bottomLeftLimit, topRightLimit;
    
    private float halfHeight, halfWidth;
    
    void Start()
    {
        target = PlayerController2.instance.transform;
        //target = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
        topRightLimit = theMap.localBounds.max - new Vector3(halfWidth, halfHeight, 0);

        PlayerController2.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //Keep camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }
}
