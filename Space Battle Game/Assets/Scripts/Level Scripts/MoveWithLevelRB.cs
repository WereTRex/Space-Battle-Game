using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithLevelRB : MonoBehaviour
{
    #region OnEnable/Disable
    private void OnEnable()
    {
        RestOfLevel.OnRestOfLevelMoved += MoveSelf;
    }
    private void OnDisable()
    {
        RestOfLevel.OnRestOfLevelMoved -= MoveSelf;
    }
    #endregion


    void MoveSelf(float xChange, float yChange)
    {
        transform.position = new Vector3(transform.position.x + xChange, transform.position.y + yChange, transform.position.z);
    }
}
