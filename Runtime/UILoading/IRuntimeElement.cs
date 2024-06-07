using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;
using UtilEssentials.UIDocumentExtenderer;


namespace UtilEssentials.UIToolkitUtility.VisualElements
{
    public interface IRuntimeElement
    {
        public UnityAction loadFinished { get; set; }
        public bool loaded { get; set; }

        public void LoadAddressableAsset(string address)
        {
            UILoader.instance.LoadAssetFromKey(address, OnLoadFinished);
        }

        void OnLoadFinished(AsyncOperationHandle<VisualTreeAsset> x)
        {
            var root = this as VisualElement;
            int count = root.childCount;
            VisualElement childContainer = new();
            for (int i = 0; i < count; i++)
            {
                childContainer.Add(root.ElementAt(0));
            }
            x.Result.CloneTree(root);
            root.Add(childContainer);
            loaded = true;
            loadFinished?.Invoke();
        }
    }
}
