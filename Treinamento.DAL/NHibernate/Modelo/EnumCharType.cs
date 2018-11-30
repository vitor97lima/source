using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Type;

namespace Treinamento.DAL.NHibernate.Modelo
{
    /// <summary>
    /// Representação do enum T em um único caractere de código ascii igual ao código numérico do enum.
    /// </summary>
    /// <typeparam name="T">O tipo do enum</typeparam>
    public abstract class EnumCharType<T> : EnumStringType where T : struct
    {
        public EnumCharType() : base(typeof(T), EnumCharType<T>.MaxSize) { }

        public override object GetValue(object pObject)
        {
            if (pObject == null) return null;
            return Convert.ToChar(pObject);
        }
        public override object GetInstance(object code)
        {
            return Enum.Parse(typeof(T), Convert.ToInt32(((string) code)[0]).ToString());
        }

        private static int MaxSize
        {
            get 
            {
                int[] values = (int[]) Enum.GetValues(typeof(T));
                if (values.Count() != values.Distinct().Count())
                    throw new Exception("Tipo de dados \'"+typeof(T).FullName+"\' possui elementos conflitantes!");
                return 1;
            }
        }
    }
}
