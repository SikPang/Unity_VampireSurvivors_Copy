using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

#if UNITY_EDITOR
namespace UnityEditor.U2D.IK
{
    [DefaultExecutionOrder(-2)]
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    [MovedFrom("UnityEditor.Experimental.U2D.IK")]
    internal class IKEditorManagerHelper : MonoBehaviour
    {
        public UnityEvent onLateUpdate = new UnityEvent();

        void Start()
        {
            if(hideFlags != HideFlags.HideAndDontSave)
                Debug.LogWarning("This is an internal IK Component. Please remove it from your GameObject", this.gameObject);
        }
        
        void LateUpdate()
        {
            if (Application.isPlaying)
                return;

            onLateUpdate.Invoke();
        }
    }
}
#endif
