using UnityEngine;
using UnityEditor;

namespace FatshihEditor
{
    public class MenuItems
    {
        [MenuItem("Tools/Clear PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}