using UnityEngine;

namespace Grid
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        private bool _identifier;
        public bool Identifier => _identifier;
        
        private void ChangeBackgroundColor(bool IsActive)
        {
            _background.color = IsActive? Color.gray : Color.white;
        }

        public void ChangIdentifier(bool identifier)
        {
            _identifier = identifier;
            ChangeBackgroundColor(identifier);
        }

    }
}