using UnityEngine;
using UnityEngine.UI;
using EnemyStaff;

namespace Debugging
{
    public class EnemyStatePreview : MonoBehaviour
    {
         private Text _text;

         private void Awake()
         {
             _text = GetComponent<Text>();
         }
         
         static string GetSubstringAfterLastDot(string input)
         {
             int lastDotIndex = input.LastIndexOf('.');
             if (lastDotIndex >= 0 && lastDotIndex < input.Length - 1)
             {
                 return input.Substring(lastDotIndex + 1);
             }
             return string.Empty;
         }

         public void UpdateState(EnemyState stateMachineCurrentEnemyState)
        {
            if (_text != null)
                _text.text = GetSubstringAfterLastDot(stateMachineCurrentEnemyState.ToString());
        }
    }
}
