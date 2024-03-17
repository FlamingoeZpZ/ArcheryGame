using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    
    //NOTE:
    //Having this as it's own component it helpful because it allows you to keep your code compartmentalized, and allows you to disable components at specific times.
    //However, it costs more to have this be outside of player.
    //The choice will lie in the developers ideals.

    public static Player LocalPlayer { get; private set; } // We do this to allow for multiplayer support, and prefabbing objects without dependency.
    private static PlayerControls _self;
    
    public static void DisableControls()
    {
        _self.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        LocalPlayer.EndFire(); //For safety.
    }

    public static void EnableControls()
    {
        _self.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Start()
    {
        LocalPlayer = GetComponent<Player>();
        _self = this;
        _self.enabled = false;
        Cursor.visible = true;
    }

    private void Update()
    {
        //Old input system. Mouse X and Mouse Y also simulate fingers.
        

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL	  // Windows or Editor
        LocalPlayer.Aim(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) LocalPlayer.BeginFire();
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) LocalPlayer.EndFire();
#else
        switch (Input.touchCount)
        {
            case 2:
                LocalPlayer.Aim(Input.GetTouch(0).deltaPosition);
                if (Input.GetTouch(1).phase == TouchPhase.Began) LocalPlayer.BeginFire();
                if (Input.GetTouch(1).phase == TouchPhase.Ended) LocalPlayer.EndFire();
                break;
            case 1:
                LocalPlayer.Aim(Input.GetTouch(0).deltaPosition);
                break;
        }
#endif
    }

}
