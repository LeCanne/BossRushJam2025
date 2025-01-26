using UnityEditor.UI;
using UnityEngine;

public class Script_camera : MonoBehaviour
{
    public GameObject Cinemachine_instance;
    public Camera camera_instance;
    public GameObject Player;
    public GameObject EnnemySample;
    public GameObject camera_target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position.Set(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {  
        //Calcule le point entre le joueur et le boss
        Vector3 middleposition = Vector3.Lerp(Player.transform.position, EnnemySample.transform.position, 0.5f);
        transform.position = middleposition;

        //Ajuste la size de la caméra par rapport à la distance joueur-boss
        //float dist = Vector3.Distance(Player.transform.position, EnnemySample.transform.position);
        //camera_instance.orthographicSize = dist;
        
    }
}
    