using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DualPantoFramework
{
    abstract public class ForceField : MonoBehaviour
    {
        public bool onUpper = true;
        public bool onLower = true;
        UpperHandle upperHandle;
        LowerHandle lowerHandle;
        protected abstract Vector3 GetCurrentForce(Collider other);
        protected abstract float GetCurrentStrength(Collider other);

        void OnTriggerStay(Collider other)
        {

            if (other.tag == "MeHandle" && onUpper)
            {
                GameObject.Find("Panto").GetComponent<UpperHandle>().ApplyForce(GetCurrentForce(other), GetCurrentStrength(other));
                Debug.DrawLine(other.transform.position, other.transform.position + GetCurrentForce(other) * GetCurrentStrength(other), Color.red);
            }
            else if (other.tag == "ItHandle" && onLower)
            {
                GameObject.Find("Panto").GetComponent<LowerHandle>().ApplyForce(GetCurrentForce(other), GetCurrentStrength(other));
                Debug.DrawLine(other.transform.position, other.transform.position + GetCurrentForce(other) * GetCurrentStrength(other), Color.red);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "MeHandle" && onUpper)
            {
                //GameObject.Find("Panto").GetComponent<UpperHandle>().StopApplyingForce();
                GameObject.Find("Panto").GetComponent<UpperHandle>().Free();
            }
            else if (other.tag == "ItHandle" && onLower)
            {
                //GameObject.Find("Panto").GetComponent<LowerHandle>().StopApplyingForce();
                GameObject.Find("Panto").GetComponent<LowerHandle>().Free();
            }
        }
    }
}
