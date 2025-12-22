using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;

public class SceneManager : MonoBehaviour
{
    public Transform[] Trees;
    public Transform[] ApleTrees;
    [SerializeField] private Transform _stickConteiner;
    [SerializeField] private Transform _apleConteiner;
    [SerializeField] private Transform _stick;
    [SerializeField] private Transform _aple;
    [SerializeField] private int _stickCount;
    [SerializeField] private int _apleCount;
    
    
    [SerializeField] private float _deviationRadius;

    private void Start()
    {
        // Spawn(_stickCount, Trees, _stick, _stickConteiner);
        // Spawn(_apleCount, ApleTrees, _aple, _apleConteiner);
    }

    private Random rand = new Random();
    private void Spawn(int v, Transform[] _trees, Transform oreginal, Transform parent = null)
    {
        for (int i = 0; i < v; i++)
            Instantiate(oreginal, VectorDeviation(_trees), UnityEngine.Quaternion.identity, parent);
    }
    public void FindEnyPlace(Transform obj)
    {
        Transform[]_T = Trees.Where(t => t != null).ToArray(); //перевірка на видимість гравцю
        obj.position = VectorDeviation(_T); //Тут ми телепортуєм палку геть поза очі
    }
    public Vector2 VectorDeviation(Transform[]Arr)
    {
        int index = rand.Next(0, Arr.Length);  
        float x = rand.Next(-100,101);
        float y = rand.Next(-100,101);
        if(x == 0) x++; if(y == 0) y++;
        x = Arr[index].position.x + _deviationRadius * (x / 100);
        y = Arr[index].position.y + _deviationRadius * (y / 100);
        return new Vector2(x,y); //Тут ми получаєм позицію з відступом від дерева
    }


}
