using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ControlUpdator : MonoBehaviour
{
    [SerializeField]
    Control[] m_controls = null;

    [SerializeField]
    Transform m_boneRoot = null;

    [SerializeField]
    bool m_shouldUpdate = false;

    [Button]
    void FillControls()
    {
        List<Control> unsortedControls = new List<Control>(
            GetComponentsInChildren<Control>());
        List<Control> sortedControls =
            new List<Control>(unsortedControls.Count);

        Stack<Transform> stack = new Stack<Transform>();
        stack.Push(m_boneRoot);
        while (stack.Count > 0)
        {
            Transform currBone = stack.Pop();

            for (int i = 0; i < unsortedControls.Count; i++)
            {
                var control = unsortedControls[i];
                if (control.BoneTransform == currBone)
                {
                    sortedControls.Add(control);
                    break;
                }
            }

            int childCount = currBone.childCount;
            for (int i = 0; i < childCount; i++)
            {
                stack.Push(currBone.GetChild(i));
            }
        }

        m_controls = new Control[sortedControls.Count];
        sortedControls.CopyTo(m_controls);
    }

    [Button]
    void ResetAll()
    {
        foreach (var control in m_controls)
        {
            control.ResetTransform();
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_shouldUpdate) return;

        foreach (var control in m_controls)
        {
            control.UpdateAssociatedBone();
        }
    }
}
