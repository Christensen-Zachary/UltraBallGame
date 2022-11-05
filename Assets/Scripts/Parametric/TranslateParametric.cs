using System;
using UnityEngine;

namespace ParametricTranslator 
{
    public abstract class TranslateParametric : MonoBehaviour
    {
        [SerializeField]
        protected float _t = Mathf.PI / 2f;
        protected float _startT = Mathf.PI / 2f;
        protected Func<float, float> _getX;
        protected Func<float, float> _getY;

        protected Vector3 _lastPosition;

        public abstract Vector3 GetPositionFromT(float t);
        public virtual void UpdatePosition(float speed)
        {
            _t += Time.deltaTime * speed;
            Vector3 newPosition = GetPositionFromT(_t);
            transform.position += newPosition - _lastPosition;
            _lastPosition = newPosition;
        }

        public void ResetPositionFields()
        {
            _t = _startT;
            _lastPosition = GetPositionFromT(_startT);
        }
    }
}