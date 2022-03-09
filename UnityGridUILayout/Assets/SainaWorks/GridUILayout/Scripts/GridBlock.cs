using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GridUILayout
{
    public class GridBlock : MonoBehaviour
    {
        public bool onGridContent = false;
        public bool isUsed = false;
        private Ray ray_ll;
        private Ray ray_lr;
        
        [SerializeField] private Image image;
        
        [SerializeField] private Transform ll;
        [SerializeField] private Transform lr;
        private void Start()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            ray_ll = new Ray(ll.position, -ll.forward);
            ray_lr = new Ray(lr.position,-lr.forward);
            
            //Debug.DrawRay(ray_ll.origin, ray_ll.direction, Color.green, 0, true);
            //Debug.DrawRay(ray_lr.origin, ray_lr.direction, Color.green, 0, true);
            
            onGridContent = false;
            
            if (Physics.Raycast(ray_ll, out RaycastHit hit_ll))
            {
                if (hit_ll.transform.TryGetComponent(out GridContent gridContent))
                {
                    onGridContent = true;
                }
            }
            if (Physics.Raycast(ray_lr, out RaycastHit hit_lr))
            {
                if (hit_lr.transform.TryGetComponent(out GridContent gridContent))
                {
                    onGridContent = true;
                }
            }

            if(onGridContent)
                image.color = Color.red;
            else
                image.color = Color.white;
        }
    }
}
