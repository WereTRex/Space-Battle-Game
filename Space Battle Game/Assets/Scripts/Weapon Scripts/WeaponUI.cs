using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Transform mainCrosshair;

    [Space(5)]

    [SerializeField] GameObject weaponCrosshairPF;
    [SerializeField] List<Transform> weaponCrosshairs;
    [SerializeField] float weaponCrosshairOrbitRadius;

    [Space(5)]

    [SerializeField] float orbitSpeed;
    [SerializeField] float input;

    private void Update()
    {
        mainCrosshair.RotateAround(transform.position, Vector3.forward, orbitSpeed * input * Time.deltaTime);

    }


    public void RecieveValues(float _inputValue, float _orbitSpeed, List<float> _currentAngles)
    {
        input = -_inputValue;
        orbitSpeed = _orbitSpeed;

        //Get angles for all weaponCrosshairs
        for (int i = 0; i <= weaponCrosshairs.Count - 1; i++)
        {
            weaponCrosshairs[i].position = new Vector3(this.transform.position.x + weaponCrosshairOrbitRadius, this.transform.position.y, this.transform.position.z);
            weaponCrosshairs[i].RotateAround(transform.position, Vector3.forward, _currentAngles[i]);
            weaponCrosshairs[i].rotation = new Quaternion(0, 0, 0, weaponCrosshairs[i].rotation.w);
        }
    }

    public IEnumerator CreateAndRemoveCrosshairs(float _numberOfWeapons)
    {
        Debug.Log("Triggered!");
        if (weaponCrosshairs.Count != _numberOfWeapons)
        {
            Debug.Log("Weapon numbers don't match!");
            if (_numberOfWeapons > weaponCrosshairs.Count)//Add More
            {
                while (weaponCrosshairs.Count != _numberOfWeapons)
                {
                    Transform crosshair = Instantiate(weaponCrosshairPF, this.gameObject.transform).transform;
                    crosshair.GetComponentInChildren<TextMeshProUGUI>().text = "" + (weaponCrosshairs.Count + 1);
                    weaponCrosshairs.Add(crosshair);
                }
            } else { //Remove
                while (weaponCrosshairs.Count != _numberOfWeapons)
                {
                    weaponCrosshairs.RemoveAt(weaponCrosshairs.Count - 1);
                }
            }
        }

        yield return null;
    }
}
