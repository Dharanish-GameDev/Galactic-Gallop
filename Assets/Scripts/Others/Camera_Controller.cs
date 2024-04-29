using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    private Transform target;
    private Vector3 Offset;
    [SerializeField] private GameObject Jimmy;
    [SerializeField] private GameObject Clarie;
    [SerializeField] private GameObject Zombie;
    [SerializeField] private GameObject Boss;

    void Start()
    {
        if(Jimmy.gameObject.activeInHierarchy)
        {
            target = Jimmy.transform;
        }
        else if(Clarie.gameObject.activeInHierarchy)
        {
            target= Clarie.transform;
        }
        else if(Zombie.gameObject.activeInHierarchy)
        {
            target= Zombie.transform;
        }
        else if(Boss.gameObject.activeInHierarchy)
        {
            target = Boss.transform;
        }
        Offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x,transform.position.y,Offset.z + target.position.z );
       // transform.position = Vector3.Lerp(transform.position, newPos , PlayerStateContext.Instance.isSliding ? 1.2f:1) ;
        transform.position = Vector3.MoveTowards(transform.position,newPos, 700f * Time.deltaTime);
    }
    
}
