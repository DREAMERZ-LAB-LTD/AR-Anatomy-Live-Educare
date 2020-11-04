using UnityEngine;

public static class Show
{
   public static void Log(string val)
    {
#if UNITY_EDITOR
        Debug.Log(val);
#endif
    }
}
