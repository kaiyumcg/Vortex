using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

public class SegIntTester : MonoBehaviour
{
    [SerializeField] AnimationSequence seq;
    // Start is called before the first frame update
    void Start()
    {
        seq.notifies.ExForEach((i) =>
        {
            if (i != null)
            {
                var sk = i as ISkeletalNotifyConfig;
                if (sk == null)
                {
                    Debug.Log("not skeletal, type: " + i.GetType());
                }
                else
                {
                    Debug.Log("skeletal and notify name: " + sk.SkeletalNotifyName);
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
