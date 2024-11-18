using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndManager : SingletonMonoBehaviour<GameEndManager>
{
    public int MaxScore;
    [SerializeField] private int _score;
    [SerializeField] private string _endLoadSceneName;

    private FadeController _fadeController;


    void Start()
    {
        _fadeController = FadeController.Instance;
    }

    void Update()
    {
        
    }

    public void AddScore()
    {
        _score++;
        if( _score >= MaxScore )
        {
            // Game End
            if(_fadeController != null ) 
            {
                // クリアシーンやリザルトシーンへ飛ぶ
                _fadeController.LoadSceneFadeIn(_endLoadSceneName);
            }
        }
    }
}
