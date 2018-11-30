using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Type;

namespace Treinamento.DAL.NHibernate.Modelo
{
    /// <summary>
    /// Representação dos elementos do enum T em uma string contendo os caracteres maiúsculos de cada elemento do enum.
    /// </summary>
    /// <typeparam name="T">O tipo do enum</typeparam>
    public abstract class EnumSiglaType<T> : EnumStringType where T : struct
    {
        public EnumSiglaType() : base(typeof(T), EnumSiglaType<T>.MaxSize) { }

        public override object GetValue(object pObject)
        {
            if (pObject == null) return null;
            return Sigla(pObject.ToString());
        }

        public override object GetInstance(object code)
        {
            string value = Enum.GetNames(base.PrimitiveClass).ToList().Find(s => Sigla(s).Equals(code));
            if (value != null) return base.GetInstance(value);
            return null;
        }

        private static string Sigla(string value)
        {
            string result = "";
            foreach (char c in value)
                if (Char.IsUpper(c)) result += c;
            return result;
        }

        private static int MaxSize
        {
            get
            {
                int max = 0;
                List<string> siglas = new List<string>();
                Enum.GetNames(typeof(T)).ToList().ForEach(s => siglas.Add(Sigla(s)));
                if (siglas.Count != siglas.Distinct().Count())
                    throw new Exception("Os elementos de " + typeof(T).FullName + " não formam acrônimos distintos!");
                siglas.ForEach(s => max = Math.Max(max, s.Length));
                return max;
            }
        }
    }
}
