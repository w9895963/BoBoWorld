using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Timer.Base;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimation_Cmp : MonoBehaviour
{
    public GameObject animationHolder;
    public AnimatePrefabHolder animatePrefabHolder;

    [SerializeField] private string currentAnimationName;
    private WaitUpdate updateTimer;
    private string nextAnimation;




    private void _更新动画()
    {
        if (currentAnimationName != nextAnimation)
        {
            currentAnimationName = nextAnimation;
            List<AnimatePrefabHolder.AnimatePrefab> allAnimation = animatePrefabHolder?.animatePrefabs.FindAll((an) => an.动画名.ToString() == currentAnimationName);
            GameObject aniObj = RandomGet(allAnimation);
            if (aniObj == null) return;
            animationHolder.DestroyChildren();
            animationHolder.CreateChild(aniObj);
        }
    }

    private GameObject RandomGet(List<AnimatePrefabHolder.AnimatePrefab> animations)
    {
        return animations.RandomGet().prefab;
    }


    public void SetAnimationFlipX(bool flip)
    {
        // SpriteRenderer spriteRenderer = animator?.GetComponent<SpriteRenderer>();
        // if (spriteRenderer == null) return;
        // spriteRenderer.flipX = flip;
        animationHolder.SetScaleLo(flip == true ? -1f : 1f, null);
    }



    #region PlayAnimation
    public void PlayAnimation(String clipName)
    {
        nextAnimation = clipName;
        if (updateTimer == null)
        {
            updateTimer = TimerF.WaitUpdate(_更新动画);
        }
    }
    public void PlayAnimation(Conf.AnimationName clipName)
    {
        PlayAnimation(clipName.ToString());
    }
    public void PlayAnimation(Conf.AnimationName clipName, UnityAction onEndOnce)
    {
        PlayAnimation(clipName.ToString());
    }





    #endregion
    // * Region  End---------------------------------- 


}
