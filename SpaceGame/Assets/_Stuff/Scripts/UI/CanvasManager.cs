using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void DeactivateEverythingForScreen()
    {
        FindObjectOfType<PrometeoCarController>().enabled = false;
        FindObjectOfType<CollisionController>().enabled = false;
        FindObjectOfType<AccelerationFeedback>().enabled = false;        
    }
}
