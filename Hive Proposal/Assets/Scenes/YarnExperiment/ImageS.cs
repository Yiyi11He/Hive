using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageS : MonoBehaviour
{
        public string TargetName;



        public void OnNodeCall(string NodeName)
        {
            gameObject.SetActive(TargetName == NodeName);
        }

}
