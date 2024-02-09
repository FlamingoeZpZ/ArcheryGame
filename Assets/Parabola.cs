using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Parabola : MonoBehaviour
{
    [SerializeField, Min(0)] private int numNodes;
    private LineRenderer lr;

    [SerializeField, Range(0.1f, 10)] private float k;

    private Weapon weapon;
    private Vector3[] positions;
    private static readonly float grav = Physics.gravity.y/2;
    
    public void Init(Weapon w)
    {
        weapon = w;
        lr.enabled = true;
        transform.SetParent(w.firePoint);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        weapon.OnCanShoot += () => lr.enabled = true;
        weapon.OnShoot += () => lr.enabled = false;
    }

    private void OnDestroy()
    {
        weapon.OnCanShoot += () => lr.enabled = true;
        weapon.OnShoot += () => lr.enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        positions = new Vector3[numNodes];
        lr.positionCount = numNodes;
        Init(transform.root.GetComponentInChildren<Weapon>());
        positions[0].Set(0,0,0);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!lr.enabled) return;

        
        //The arrow has a forward force of weapon.currentFirePower();
        //The arrow has a downward force of gravity.
        //Then we need to set N nodes. X is sampling that value.
        float n = weapon.CurrentFirePower();
        Vector3 velocity = weapon.firePoint.right * n;
        for (int i = 1; i < numNodes; i++)
        {
            float time = i * k;
            float y = velocity.y * time + grav * time * time;
            float z = velocity.z * time;

            Vector3 point = new Vector3(0, y, z);

            positions[i] = point;
           // positions[i].Set(0,  n * i * i + grav * i, n*i);
        }
        
        
        lr.SetPositions(positions);
        //We need to set each point based on a parabola.
        //y = ax^2 + bx + c
        //X is distance,
        //a is gravity?
        //b is pullback force
        //c is offset.
        
    }
}
