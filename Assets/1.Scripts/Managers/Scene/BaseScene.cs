using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    private void Awake()
    {
        Enter();
    }
    public abstract void Enter();
    public abstract void Exit();
}
