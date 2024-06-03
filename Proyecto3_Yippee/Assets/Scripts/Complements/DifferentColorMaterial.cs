using UnityEngine;

namespace UtilsComplements
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer))]
    public class DifferentColorMaterial : MonoBehaviour
    {
        private const string COLOR_ID = "_BaseColor";

        [Header("References")]
        [SerializeField] private Color _color;
        private MeshRenderer _meshRenderer;

        private MaterialPropertyBlock _materialPropertyBlock;

        private MaterialPropertyBlock ThisMaterialPropertyBlock
        {
            get
            {
                if (_materialPropertyBlock == null)
                    _materialPropertyBlock = new MaterialPropertyBlock();
                return _materialPropertyBlock;
            }
        }

        private MeshRenderer ThisMeshRenderer
        {
            get
            {
                if (_meshRenderer == null)
                    _meshRenderer = GetComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }

        private void OnEnable()
        {
            ChangeColor();
        }

        private void OnValidate()
        {
            ChangeColor();
        }

        private void ChangeColor()
        {
            ThisMaterialPropertyBlock.SetColor(COLOR_ID, _color);
            ThisMeshRenderer.SetPropertyBlock(ThisMaterialPropertyBlock);
        }

        public void SetNewColor(Color newColor)
        {
            _color = newColor;
            ChangeColor();
        }
    }
}
