using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public ScriptableObject[] 配置文件;



    private void Start()
    {
        FollowMainCharacter();
    }


    public void FollowMainCharacter(bool enabled = true)
    {
        CameraAction_Follow_Conf conf = 配置文件.FIndType<CameraAction_Follow_Conf>();




        if (enabled)
        {
            CameraAction_Follow comp = gameObject.GetComponent<CameraAction_Follow>(true);
            comp.target = Fc.FindOnlyComponent<CharacterManager>()?.gameObject;
            comp.config = conf;


        }
        else
        {
            gameObject.GetComponent<CameraAction_Follow>()?.Destroy();
        }
    }


}
