using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartShard : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        transform.DOMoveY(transform.position.y - 0.5f, 0.8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.CollectHeartShard();
        DOTween.Kill(transform);
        Destroy(gameObject);
    }
}
