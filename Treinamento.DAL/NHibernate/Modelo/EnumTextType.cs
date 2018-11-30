using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Type;

namespace Treinamento.DAL.NHibernate.Modelo
{
    public abstract class EnumTextType<T> : EnumStringType where T : struct
    {
        public EnumTextType() : base(typeof(T), EnumTextType<T>.MaxSize) { }

        private static int MaxSize
        {
            get
            {
                int max = 0;
                List<string> lTexts = new List<string>();
                Enum.GetNames(typeof(T)).ToList().ForEach(s => lTexts.Add(s));
                lTexts.ForEach(s => max = Math.Max(max, s.Length));
                return max;
            }
        }
    }
}
