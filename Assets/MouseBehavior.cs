using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{

    private bool isCamMoving = false;
    public int speed = 2;

    private int screenHeight = Screen.height;
    private int screenWidth = Screen.width;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = transform.position;
        if (Input.mousePosition.x > screenWidth - 30)
        {
            isCamMoving = true;
            camPos.x += speed * (Time.deltaTime + 1);
        }
        else if (Input.mousePosition.x < 30)
        {
            isCamMoving = true;
            camPos.x -= speed * (Time.deltaTime + 1);
        }

        else if (Input.mousePosition.y > screenHeight - 30)
        {
            isCamMoving = true;
            camPos.z += speed * (Time.deltaTime + 1);
        }
        else if (Input.mousePosition.y < 30)
        {
            isCamMoving = true;
            camPos.z -= speed * (Time.deltaTime + 1);
        }
        else
        {
            isCamMoving = false;
        }

        transform.position = camPos;

        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (ScrollWheelChange != 0)
        {
            float R = ScrollWheelChange * 15;                                   //The radius from current camera
            float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                       //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            float Y = R * Mathf.Cos(PosX);                                      //^
            float CamX = Camera.main.transform.position.x;                      //Get current camera postition for the offset
            float CamY = Camera.main.transform.position.y;                      //^
            float CamZ = Camera.main.transform.position.z;                      //^
            Camera.main.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);//Move the main camera
        }
    }


}
