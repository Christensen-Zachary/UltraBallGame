using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ParametricTranslator
{
    public class TranslateLemniscate : TranslateParametric
    {
        public LemniscateParams LemniscateParams { get; set; }

        public void Construct(LemniscateParams parameters)
        {
            this.LemniscateParams = Instantiate(parameters);
            this._t = LemniscateParams.startT;

            _getX = (t) => { return LemniscateParams.a * Mathf.Cos(t) / (1 + Mathf.Pow(Mathf.Sin(t), 2)); };
            _getY = (t) => { return (float)(LemniscateParams.a * Math.Sin(t) * Mathf.Cos(t) / (1 + Mathf.Pow(Mathf.Sin(t), 2))); };
            _lastPosition = new Vector3(_getX(_t), _getY(_t), 0);
        }

        public override Vector3 GetPositionFromT(float t)
        {
            return new Vector3(_getX(t), _getY(t), 0);
        }


    }

}