using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    
    //NOTE:
    //Having this as it's own component it helpful because it allows you to keep your code compartmentalized, and allows you to disable components at specific times.
    //However, it costs more to have this be outside of player.
    //The choice will lie in the developers ideals.

    private Player _p;
    private static PlayerControls _self;
    
    public static void DisableControls()
    {
        _self.enabled = false;
        Cursor.visible = true;
    }

    public static void EnableControls()
    {
        _self.enabled = true;
        Cursor.visible = false;
    }


    private void Awake()
    {
        _p = GetComponent<Player>();
        _self = this;
        enabled = false;
    }

    private void Update()
    {
        //Old input system. Mouse X and Mouse Y also simulate fingers.
        _p.Aim(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

#if UNITY_EDITOR || UNITY_STANDALONE // Windows or Editor
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) _p.BeginFire();
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) _p.EndFire();
#else
        if (Input.GetTouch(0).phase == TouchPhase.Began) p.BeginFire();
        if (Input.GetTouch(0).phase == TouchPhase.Ended) p.BeginFire();
#endif
    }

}
