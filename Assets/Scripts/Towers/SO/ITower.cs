using System.Collections.Generic;
using UnityEngine;


public interface ITower
{
    public string Name { get; set; }
    public int coast { get; set; }
    public int range { get; set; }
    public float damages { get; set; }
    public float attackCooldown { get; set; }
    public damagesType damagesType { get; set; }
    public Sprite sprite { get; set; }
    public GameObject bulletPrefab { get; set; }
}

public class Tower : ScriptableObject, ITower
{
    [SerializeField] private string _name;
    [SerializeField] private int _coast;
    [SerializeField] private int _range;
    [SerializeField] private float _damages;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private damagesType _damagesType;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _bulletPrefab;

    public string Name { get => _name; set => _name = value; }
    public int coast { get => _coast; set => _coast = value; }
    public int range { get => _range; set => _range = value; }
    public float damages { get => _damages; set => _damages = value; }
    public float attackCooldown { get => _attackCooldown; set => _attackCooldown = value; }
    public damagesType damagesType { get => _damagesType; set => _damagesType = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public GameObject bulletPrefab { get => _bulletPrefab; set => _bulletPrefab = value; }

    public List<Tower> NextTowers = new List<Tower>();

    public int refundCoast;
    public AudioClip shootSFX;
}

public enum Capacity
{
    attackAir,
    AOE
}

public enum damagesType
{
    none,
    physical,
    magical
}
