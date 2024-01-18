using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraFader : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    private List<RaycastHit> _oldHits;
    private List<RaycastHit> _removableHits;

    private void Start()
    {
        _oldHits = new List<RaycastHit>();
        _removableHits = new List<RaycastHit>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        var position1 = target.transform.position;
        Vector3 direction = position1 - position;
        
        RaycastHit[] hits;

        hits = Physics.RaycastAll(position, direction,
            Vector3.Distance(position, position1));
        
        bool found = false;

        foreach (RaycastHit oldHit in _oldHits)
        {
            found = false;
            foreach (RaycastHit hit in hits)
            {
                if (oldHit.Equals(hit))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                oldHit.collider.gameObject.GetComponent<ObjectFader>().doFade = false;
                _removableHits.Add(oldHit);
            }
        }

        foreach (RaycastHit removableHit in _removableHits)
        {
            _oldHits.Remove(removableHit);
        }

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Environment"))
            {
                if (!_oldHits.Contains(hit))
                {
                    hit.collider.gameObject.transform.GetChild(0).GetComponent<Renderer>().shadowCastingMode =
                        ShadowCastingMode.ShadowsOnly;
                    hit.collider.gameObject.GetComponent<ObjectFader>().doFade = true;
                    _oldHits.Add(hit);
                }
            }
        }
    }
}
