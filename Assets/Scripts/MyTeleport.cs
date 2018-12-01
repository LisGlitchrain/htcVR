using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTC.UnityPlugin.Vive
{
    public class MyTeleport : Teleportable
    {
        [SerializeField] float speed;
        [SerializeField] float coolDown;

        public override IEnumerator StartTeleport(Vector3 position, float duration)
        {
            while(true)
            {
                yield return new WaitForFixedUpdate();

                target.position = Vector3.MoveTowards(target.position, position, speed * Time.deltaTime);
                Vector3 v = position;
                v.y = target.position.y;

                if(Vector3.Distance(target.position,v)< 0.1f)
                {
                    yield return new WaitForSeconds(coolDown);
                    teleportCoroutine = null;
                    yield break;
                }
            }
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

