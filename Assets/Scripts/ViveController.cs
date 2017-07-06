using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/*
** Put this Object on the vive left and/or right controller then set the InputController interface implementation.
*/
public class ViveController : MonoBehaviour
{

    private static ulong TRIGGER = SteamVR_Controller.ButtonMask.Trigger;
    private static ulong GRIPS = SteamVR_Controller.ButtonMask.Grip;
    private static ulong TOUCHPAD = SteamVR_Controller.ButtonMask.Touchpad;

    public Hand viveController;
    public GameObject menuController;

    private SteamVR_Controller.Device device;
    private Vector2 touchpadPressLocation;

    private Menu menu;

    // Use this for initialization
    void Start()
    {
        menu = (Menu)menuController.GetComponent("MenuImpl");
    }

    // Update is called once per frame
    void Update()
    {
        device = viveController.controller;
        if (device != null)
        {
            handleGrips();
            handleTriggers();
            handleTouchpads();
        }
    }

    private void handleGrips() {
        if (device.GetPress(GRIPS))
        {
            viveController.enabled = !viveController.enabled;
        }
    }
    
    private void handleTriggers() {
        if (device.GetPressDown(TRIGGER))
        {
            
        }

        if (device.GetPressUp(TRIGGER))
        {
            
        }
    }

    //Either navigation or menu activation
    private void handleTouchpads() { 
        if (device.GetTouch(TOUCHPAD))
        {
          menu.enable();
        }

        if (device.GetPressDown(TOUCHPAD))
        {
          this.onMenuPress();
        }

        if (device.GetTouchUp(TOUCHPAD))
        {
          menu.disable();
        }
    }

    //Collision detection with some objects
    void OnTriggerEnter(Collider collider)
    {
       
    }

    void OnTriggerExit(Collider collider)
    {
     
    }
    
    //Handle touchpad presses
    private void onMenuPress()
    {
        //Read the touchpad values
        touchpadPressLocation = device.GetAxis();
        //Debug.Log(touchpadLoc);

        //Simple center click
        if ((touchpadPressLocation.x >= -0.5) && (touchpadPressLocation.x <= 0.5) &&
            (touchpadPressLocation.y >= -0.5) && (touchpadPressLocation.y <= 0.5))
        {
            menu.navigateSelect();
            return;
        }

        //Diagonals
        if ((touchpadPressLocation.x > 0.5) && (touchpadPressLocation.y > 0.5) ||    //top right
            (touchpadPressLocation.x < -0.5) && (touchpadPressLocation.y < -0.5) ||  //bottom left
            (touchpadPressLocation.x > 0.5) && (touchpadPressLocation.y < -0.5) ||   //bottom right
            (touchpadPressLocation.x < -0.5) && (touchpadPressLocation.y > 0.5))     //top left
        {
            //Diagonals are dead zones, don't do anything if pressed there
        }

        if (touchpadPressLocation.y > 0.5)
        {
            menu.navigateUp();
        }

        if (touchpadPressLocation.y < -0.5)
        {
            menu.navigateDown();
        }

        if (touchpadPressLocation.x < -0.5)
        {
            menu.navigateLeft();
        }

        if (touchpadPressLocation.x > 0.5)
        {
            menu.navigateRight();
        }
    }
}
