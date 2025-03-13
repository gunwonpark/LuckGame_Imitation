using UnityEngine;

public class TestingCode : MonoBehaviour
{
    private void Awake()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} is loaded. {count}/{totalCount}");
            if(count == totalCount)
            {
                Managers.Data.Init();
            }
        });
    }
}
