using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadingCameraScript : MonoBehaviour
{
    [SerializeField] GameObject playerShip;
    
    private void Update()
    {
        this.transform.rotation = Quaternion.Euler(0 - playerShip.transform.rotation.x, 360 - playerShip.transform.rotation.y, 0 - playerShip.transform.rotation.z);
    }
}
