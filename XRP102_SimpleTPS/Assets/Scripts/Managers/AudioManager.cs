using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 오브젝트 풀을 가지게 할 것

    private AudioSource _bgmSource;
    private ObjectPool _sfxPool;

    [SerializeField] private List<AudioClip> _bgmList = new();
    [SerializeField] private SFXController _sfxPrefab;

    private void Awake() => Init();

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();

        _sfxPool = new ObjectPool(transform,_sfxPrefab, 10);
    }
 
    public void BGMPlay(int index)
    {
        if (0 <= index && index < _bgmList.Count)
        {
            _bgmSource.Stop();
            _bgmSource.clip = _bgmList[index];
            _bgmSource.Play();
        }
    }

    public SFXController GetSFX()
    {
        // 풀에서 꺼내와서 반환
        PooledObject po =_sfxPool.PopPool();

        // SFXController로 반환하면서 리턴
        return po as SFXController;
    }
}
