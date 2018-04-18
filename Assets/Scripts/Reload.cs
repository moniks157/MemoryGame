using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour {
    [SerializeField] private SceneController controller;

    public void OnMouseDown()
    {
        Debug.Log("XD");
        controller.Restart();
    }
}
