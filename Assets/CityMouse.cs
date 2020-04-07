using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityMouse : MonoBehaviour

{
    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = Color.blue;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    private bool isOver = false;
    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

    public Text CItyTT;

    public City city;
    public Text cityText;

    public CityMouse(string a)
    {
        Debug.Log("OK CONSTRUCTOR " + a);
    }

    void Start()
    {
        Physics.queriesHitTriggers = true;
        Cursor.visible = true;

        Debug.Log("OK START MOUSE " + gameObject.name);

        //Fetch the mesh renderer component from the GameObject
        m_Renderer = GetComponent<MeshRenderer>();


    }
    void OnMouseOver()
    {
        if (cityText == null)
        {
            GameObject UITT = GameObject.FindGameObjectWithTag("UITT");
            cityText = UITT.GetComponent<Text>();
            cityText.text = city.name + "(" + city.localPops.Count + " pops)" + " | Wheat :" + city.stock["Wheat"] + " | Cow : " + city.stock["Cow"];
            isOver = true;

        }
        else
        {
            isOver = true;
            cityText.text = city.name + "(" + city.localPops.Count + " pops)" + " | Wheat :" + city.stock["Wheat"] + " | Cow : " + city.stock["Cow"];
        }
    }
    void OnMouseExit()
    {
        if (isOver == true)
        {
            isOver = false;

        }
    }
}