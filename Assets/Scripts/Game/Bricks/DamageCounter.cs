using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class DamageCounter : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private int _damage = 0;
    public int DamageCount { get { return _damage; } set { _damage = value; }}
    private int _destroyedCount = 0;
    public int DestroyedCount { get { return _destroyedCount; } set { _destroyedCount = value; }}

    private List<(int damage, int destroyedCount, float time)> DamagePerTurn { get; set; } = new List<(int damage, int destroyedCount, float time)>();

    private bool _turnActive = false;
    private float _timer = 0;


    private void Awake() 
    {
        ResourceLocator.AddResource("DamageCounter", this);    
    }

    private void Update() 
    {
        if (_turnActive)
        {
            _timer += Time.deltaTime;
        }    
    }

    public void StartTurn()
    {
        _turnActive = true;
        _timer = 0;
    }

    public void EndTurn()
    {
        _turnActive = false;
        DamagePerTurn.Add((DamageCount, DestroyedCount, _timer));
        DamageCount = 0;
        DestroyedCount = 0;
    }

    public void ResetCounters()
    {
        DamagePerTurn = new();
        DamageCount = 0;
        DestroyedCount = 0;
        _turnActive = false;

    }

    public string TurnDamageString()
    {
        // totalDamage, destroyedCount, 
        return $"{DamagePerTurn.Last().damage},{DamagePerTurn.Last().destroyedCount}";
        //print($"{DamagePerTurn.Count}: {DamagePerTurn.Last().damage} - {DamagePerTurn.Last().destroyedCount} - {DamagePerTurn.Last().time} - {DamagePerTurn.Last().damage / DamagePerTurn.Last().time}");
    }
}
