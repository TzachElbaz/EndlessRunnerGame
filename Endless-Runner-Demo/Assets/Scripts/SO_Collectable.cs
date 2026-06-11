using UnityEngine;

[CreateAssetMenu(fileName = "SO_Collectable", menuName = "Scriptable Objects/SO_Collectable")]
public class SO_Collectable : ScriptableObject
{
    [SerializeField] public int _colectableId;
    [SerializeField] public SCREEN_COL _zone;
    [SerializeField] public Sprite _sprite;
    [SerializeField] public Animator _animation;
    [SerializeField] public string _name;
    public enum SCREEN_COL
    {
        FOREST,
        DESERT
    }
}
