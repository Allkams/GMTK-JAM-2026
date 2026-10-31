using Unity.VisualScripting;
using UnityEngine;

public class Quit : MonoBehaviour
{
    [SerializeField] Camera repairCamera;

    public void QuitGame()
    {
        if(repairCamera.enabled)
        {
            return;
        }
        Application.Quit();
    }
}
