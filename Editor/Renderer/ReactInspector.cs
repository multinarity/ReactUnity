using ReactUnity.Types;
using UnityEditor;
using UnityEngine.UIElements;

namespace ReactUnity.Editor.Renderer
{
    public abstract class ReactInspector : UnityEditor.Editor
    {
#if REACT_UNITY_DEVELOPER
        protected bool DevServerEnabled
        {
            get
            {
                return !EditorPrefs.GetBool($"ReactUnity.Editor.ReactInspector.{GetType().Name}.DevServerDisabled");
            }
            set
            {
                EditorPrefs.SetBool($"ReactUnity.Editor.ReactInspector.{GetType().Name}.DevServerDisabled", !value);
            }
        }
#endif

        public override VisualElement CreateInspectorGUI()
        {
            return new ReactUnityElement(GetScript(), GetGlobals());
        }

        protected abstract ReactScript GetScript();

        protected virtual GlobalRecord GetGlobals()
        {
            return new GlobalRecord()
            {
                { "Inspector", this },
            };
        }
    }
}