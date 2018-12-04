using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharSpell
{
    [System.Serializable]
    public struct MagnitePoint
    {
        public List<SpringJoint> JointList; //список существующих джоинтов
        public List<Rigidbody> RG;          //список всех связанных RB
        public List<ParticleSystem> HighLight; //подсветка с помощью частиц
        public Transform BlueObj, RedObj;   //ссылка на активные объекты, помеченные цветом
        public Vector3 BluePos, RedPos;     //ссылка на точку для измерения расстояния
    }

    public class CharMagnetic    : MonoBehaviour
    {
        [SerializeField] float spellDistance = 20; //дистанция снаряда магнита
        [SerializeField] float MaxMagniteForce; //максимальная сила магнита
        [SerializeField] MagnitePoint MagniteSpell; //работаем с нашей структурой
        [SerializeField] Transform BlueHolder, RedHolder; //вспомогательные объекты
        [SerializeField] Material RedMat, BlueMat, YellowMat; //ссылки на материалы частиц
        [SerializeField] ParticleSystem hl_Reference; //ссылка на систему частиц

        public void SetBlue(Transform trans) //вызываем, если попали по движимому объекту
        {
            MagniteSpell.BlueObj = trans;   //даем ссылку на активный объект
            MagniteSpell.BluePos = trans.position; // ссылка на точку для проверки расстояния
            Highlighting(true, trans); //создание подсветки
            CheckToJoint(); //проверка можно ли уже создать сочленение
        }

        public void SetRed(Transform trans) //вызываем, если попали по подвижному объекту
        {
            MagniteSpell.RedObj = trans;
            MagniteSpell.RedPos = trans.position;
            Highlighting(false, trans);
            CheckToJoint();
        }

        public void SetBlue(Vector3 trans)
        {
            MagniteSpell.BlueObj = BlueHolder;
            MagniteSpell.BluePos = trans;
            BlueHolder.position = trans;
            BlueHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }


        public void SetRed(Vector3 trans)
        {
            MagniteSpell.RedObj = RedHolder;
            MagniteSpell.RedPos = trans;
            RedHolder.position = trans;
            RedHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }

        void CheckToJoint()
        {
            if (MagniteSpell.BlueObj != null && MagniteSpell.RedObj != null)
            {
                if (Vector3.Distance(MagniteSpell.RedPos, MagniteSpell.BluePos) < spellDistance) CreateJoint();
                else EreaseSpell(); //очистка всех ссылок и приведение всех объектов  в состояние ожидания
                //Сделать запрет за создание сочленения между плейсхолдерами
            }
        }

        void CreateJoint()
        {
            SpringJoint sp = MagniteSpell.BlueObj.gameObject.AddComponent<SpringJoint>();
            sp.autoConfigureConnectedAnchor = false;

            sp.anchor = Vector3.zero;
            sp.connectedAnchor = Vector3.zero;
            sp.enableCollision = true;
            sp.enablePreprocessing = false;

            sp.connectedBody = MagniteSpell.RedObj.GetComponent<Rigidbody>();

            EreaseSpell();
            MagniteSpell.JointList.Add(sp);

            Rigidbody rg = sp.GetComponent<Rigidbody>();
            MagniteSpell.RG.Add(rg);
            AddRG(sp.connectedBody);
        }

        void EreaseSpell()
        {
            MagniteSpell.BlueObj = null;
            MagniteSpell.RedObj = null;

            for(var i=0;i< MagniteSpell.HighLight.Count; i++)
            {
                MagniteSpell.HighLight[i].GetComponent<Renderer>().material = YellowMat;
            }

        }
        //Добавление элемента в цепочку, при указании одного элемента
        public void AddElementToChain(Transform trans)
        {
            for(var i=0; i< MagniteSpell.RG.Count;i++)
            {
                print("I =" + i + " = " + MagniteSpell.RG[i].gameObject.name);
            }
            SetBlue(MagniteSpell.RG[MagniteSpell.RG.Count - 1].transform);
            SetRed(trans);
        }

        void AddRG(Rigidbody RG)
        {
            for(var i=0; i< MagniteSpell.RG.Count; i++)
            {
                if (RG == MagniteSpell.RG[i]) break;
                if (MagniteSpell.RG != null)
                {
                    if (i == MagniteSpell.RG.Count - 1) MagniteSpell.RG.Add(RG);
                    break;
                }
            }
        }

        void Highlighting(bool isBlue, Transform trans)
        {
            ParticleSystem ps = trans.GetComponentInChildren<ParticleSystem>();
            if (ps == null)
            {
                Instantiate(hl_Reference, trans, false);
                ps.gameObject.SetActive(true);
                MagniteSpell.HighLight.Add(ps);
            }
            print("name " + ps.name);

            if (isBlue) ps.GetComponent<Renderer>().material = BlueMat;
            else ps.GetComponent<Renderer>().material = RedMat;
        }

        public void DestroyAllJoint()
        {
            for (var i=0;i< MagniteSpell.JointList.Count;i++)
            {
                Destroy(MagniteSpell.JointList[i]);
            }

            for(var i=0;i< MagniteSpell.RG.Count;i++)
            {
                MagniteSpell.RG[i].angularDrag = 0.05f;
                MagniteSpell.RG[i].drag = 0;
                MagniteSpell.RG[i].WakeUp();
            }

            MagniteSpell.JointList.Clear();
            EreaseSpell();

            for (var i = 0; i < MagniteSpell.HighLight.Count; i++)
                Destroy(MagniteSpell.HighLight[i].transform.gameObject);
            MagniteSpell.HighLight.Clear();
            DisableHolders();
        }

        void  DisableHolders()
        {
            BlueHolder.GetChild(0).gameObject.SetActive(false);
            RedHolder.GetChild(0).gameObject.SetActive(false);
        }

        public void ChangeSpingPower(float fNum)
        {
            if (MagniteSpell.JointList.Count >0)
            {
                for(var i=0;i<MagniteSpell.JointList.Count;i++)
                {
                    MagniteSpell.JointList[i].spring += fNum;
                    MagniteSpell.JointList[i].damper += fNum;

                    MagniteSpell.JointList[i].spring = Mathf.Clamp(MagniteSpell.JointList[i].damper,0, MaxMagniteForce);
                    MagniteSpell.JointList[i].damper = Mathf.Clamp(MagniteSpell.JointList[i].damper, 0, MaxMagniteForce);
                }

                for(var i=0;i< MagniteSpell.RG.Count;i++)
                {
                    MagniteSpell.RG[i].WakeUp();
                    MagniteSpell.RG[i].angularDrag += fNum;
                    MagniteSpell.RG[i].drag += fNum;
                    MagniteSpell.RG[i].angularDrag = Mathf.Clamp(MagniteSpell.RG[i].angularDrag, 0, MaxMagniteForce);
                    MagniteSpell.RG[i].drag = Mathf.Clamp(MagniteSpell.RG[i].drag, 0, MaxMagniteForce);

                }
            }
        }

    }
}


