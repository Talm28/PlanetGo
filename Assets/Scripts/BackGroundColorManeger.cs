using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundColorManeger : MonoBehaviour
{
    private Camera camera;
    private float playerAngle;

    [SerializeField] float colorChangeSpeed;
    [SerializeField] Color[] colors;
    [SerializeField] GameObject player;
    [SerializeField] float forestBoundery;
    [SerializeField] float swampBoundery;
    [SerializeField] float desertBoundery;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAngle = playerMovement.playerAngle;
    }

    // Update is called once per frame
    void Update()
    {
        // Transfer angle to full
        playerAngle = playerMovement.playerAngle;
        if(playerAngle < 0)
            playerAngle = playerAngle + 2 * Mathf.PI;
        if(playerAngle >= forestBoundery * Mathf.Deg2Rad && playerAngle <= swampBoundery * Mathf.Deg2Rad) // Forest
            ChangeColor(0);
        else if(playerAngle > swampBoundery * Mathf.Deg2Rad && playerAngle <= desertBoundery * Mathf.Deg2Rad) // Swamp
            ChangeColor(1);
        else // Desert
            ChangeColor(2);
        
    }

    void ChangeColor(int index)
    {
        if(camera.backgroundColor != colors[index])
        {
            camera.backgroundColor = Color.Lerp(camera.backgroundColor, colors[index],colorChangeSpeed * Time.deltaTime);
        }
            
    }
}
