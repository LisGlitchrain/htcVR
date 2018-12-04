using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharSpell
{
    public class InputManager : MonoBehaviour
    {

        [SerializeField] CharMagnetic cm;
        [SerializeField] HTC.UnityPlugin.Pointer3D.MagniteRaycaster mraycaster;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                mraycaster.ColorOfMagnite = HTC.UnityPlugin.Pointer3D.MagniteRaycaster.TypeOfMagnite.Blue;
                mraycaster.StartMagnite();
            }
            if (Input.GetMouseButtonDown(1))
            {
                mraycaster.ColorOfMagnite = HTC.UnityPlugin.Pointer3D.MagniteRaycaster.TypeOfMagnite.Red;
                mraycaster.StartMagnite();
            }


            if (Input.GetAxis("Mouse ScrollWheel") >= 0.1) { cm.ChangeSpingPower(+2f); }
            if (Input.GetAxis("Mouse ScrollWheel") <= -0.1) { cm.ChangeSpingPower(-2f); }

            if (Input.GetMouseButtonDown(2)) { mraycaster.AddElementToChain(); print("middle"); }

            if (Input.GetKeyDown(KeyCode.C)) cm.DestroyAllJoint();

        }
    }
}

