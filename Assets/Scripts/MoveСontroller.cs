using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class MoveСontroller : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] GameObject playerView;
    Vector3 touchDir;
    Vector2 touchDir2D;
    Quaternion rotation;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //проверяем, есть ли касание тачпада.
        if (ViveInput.GetPress(HandRole.LeftHand, ControllerButton.PadTouch)) 
        {
            //берем вектор направления (я полагаю, что vive выдает вектор с центром координат в центре пада, если это не так,
            //то надо будет просто сместить его значения на половину)
            touchDir2D = ViveInput.GetPadTouchVector(HandRole.LeftHand);
            //делаем из него плоский трехмерный вектор

            touchDir = new Vector3(touchDir2D.x, touchDir2D.y, 0);
            // получаеми кватернион для вращения вектора направления движеиния
            Quaternion.FromToRotation(Vector3.forward, playerView.transform.forward);
            //Вращаем вектор
            touchDir = rotation * touchDir;
            //двигаем пользователя в направлении
            playerView.transform.position += touchDir * speed * Time.deltaTime; 
            
        }
		
	}
}
