using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;

public class HapticTexture : MonoBehaviour
{
    public float distanceZ;
    public float distanceX;
    public LayerMask layerMask;

    void Start()
    {
        Collider c = GetComponent<Collider>();

        if (distanceZ > 0)
        {
            float m_Min = c.bounds.min.z;
            float m_Max = c.bounds.max.z;
            int numberOfRails = (int)Mathf.Floor((m_Max - m_Min) / distanceZ);
            for (int i = 0; i <= numberOfRails; i++)
            {
                float height = i * distanceZ + m_Min;

                RaycastHit hit1;
                RaycastHit hit2;
                Vector3 start = new Vector3(c.bounds.min.x - 0.01f, c.bounds.center.y, height);
                Vector3 end = new Vector3(c.bounds.max.x + 0.01f, c.bounds.center.y, height);
                if (!Physics.Linecast(start, end, out hit1, layerMask)) continue;
                if (!Physics.Linecast(end, start, out hit2, layerMask)) continue;

                float length = Vector3.Distance(hit1.point, hit2.point);

                GameObject rail = Instantiate(Resources.Load("Rail"), transform) as GameObject;
                Vector3 ls = rail.transform.parent.lossyScale;
                rail.transform.localScale = new Vector3((1.0f / ls.x) * length, 1 / ls.y, (1.0f / ls.z) * 0.4f);
                rail.transform.position = new Vector3((hit1.point.x - hit2.point.x) * 0.5f + hit2.point.x, 0, height);
                rail.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        if (distanceX > 0)
        {
            float m_Min = c.bounds.min.x;
            float m_Max = c.bounds.max.x;
            int numberOfRails = (int)Mathf.Floor((m_Max - m_Min) / distanceX);
            for (int i = 0; i <= numberOfRails; i++)
            {
                float height = i * distanceX + m_Min;

                RaycastHit hit1;
                RaycastHit hit2;
                Vector3 start = new Vector3(height, c.bounds.center.y, c.bounds.min.z - 0.01f);
                Vector3 end = new Vector3(height, c.bounds.center.y, c.bounds.max.z + 0.01f);
                if (!Physics.Linecast(start, end, out hit1, layerMask)) continue;
                if (!Physics.Linecast(end, start, out hit2, layerMask)) continue;

                float length = Vector3.Distance(hit1.point, hit2.point);

                GameObject rail = Instantiate(Resources.Load("Rail"), transform) as GameObject;
                Vector3 ls = rail.transform.parent.lossyScale;
                rail.transform.localScale = new Vector3((1.0f / ls.x) * 0.4f, 1 / ls.y, (1.0f / ls.z) * length);
                rail.transform.position = new Vector3(height, 0, (hit1.point.z - hit2.point.z) * 0.5f + hit2.point.z);
                rail.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
