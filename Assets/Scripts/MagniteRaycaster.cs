using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CharSpell;

namespace HTC.UnityPlugin.Pointer3D
{
    public class MagniteRaycaster : MonoBehaviour
    {
        public enum TypeOfMagnite
        {
            Blue,
            Red,
        }
        [SerializeField] TypeOfMagnite ColorOfMagnite;
        [Tooltip("Ссылка на  spell персонажа 0.")]
        [SerializeField] CharMagnetic refToChar;
        [SerializeField] Pointer3DRaycaster raycaster;
        [SerializeField] RaycastResult curObj;
        [SerializeField] int layerRaycast;
        RaycastHit rh;

        private void OnValidate()
        {
            raycaster = GetComponent<Pointer3DRaycaster>();
        }

        private void LateUpdate()
        {
            Raycasting();
        }

        public void Raycasting()
        {
            curObj = raycaster.FirstRaycastResult();
        }

        public void StartMagnite()
        {
            if (curObj.isValid)
            {
                print(curObj.gameObject.name);
                Rigidbody RG = curObj.gameObject.GetComponent<Rigidbody>();

                switch ((int)ColorOfMagnite)
                {
                    case 0:
                        if (RG != null) refToChar.SetBlue(curObj.gameObject.transform);
                        else refToChar.SetBlue(curObj.worldPosition);
                        break;
                    case 1:
                        if (RG != null) refToChar.SetRed(curObj.gameObject.transform);
                        else refToChar.SetRed(curObj.worldPosition);
                        break;
                }
            }
            else return;
        }



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


