using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPromptHider : MonoBehaviour
{
    public List<GameObject> openRequests;

    void Update()
    {
        if (openRequests.Count == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
