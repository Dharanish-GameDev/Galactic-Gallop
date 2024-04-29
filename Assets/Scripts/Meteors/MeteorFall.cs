using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall : MonoBehaviour
{
    [SerializeField]private List<Transform> list = new List<Transform>();
    private Transform playerController;

    private void Start()
    {
        playerController = PlayerStateContext.Instance.transform;
    }

    private void Update()
    {
        if (list.Count == 0) return;
        
            for (int i = 0; i < list.Count; i++)
            {
                if (Mathf.Abs(list[i].position.z - playerController.position.z) < 18f)
                {
                    list[i].GetComponent<MeteorBehaviour>().canFall = true;
                    list.RemoveAt(i);
                }
            }
    }
}
