using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Treinamento.Exceptions
{
    public class Excecao
    {
        protected Excecao() { }
        public Excecao(Exception pException, Excecao pParentException = null)
        {
            _guid = Guid.NewGuid();
            _message = pException.Message;
            _data = DateTime.Now;
            SetTrace(pException.StackTrace);
            _tipo = pException.GetType().FullName;
            _exceptions = new List<Excecao>();
            _parentException = pParentException;
            
            if (pException is BusinessException && ((BusinessException)pException).Child != null)
                _exceptions.Add(new Excecao(((BusinessException)pException).Child, this));

            if (pException.InnerException != null)
                _exceptions.Add(new Excecao(pException.InnerException, this));
        }

        private int _id;
        public virtual int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        private Guid _guid;
        public virtual Guid Guid
        {
            get { return _guid; }
            protected set { _guid = value; }
        }

        private string _message;
        public virtual string Message
        {
            get { return _message; }
            protected set { _message = value; }
        }

        private string _tipo;
        public virtual string Tipo
        {
            get { return _tipo; }
            protected set { _tipo = value; }
        }

        private DateTime _data;
        public virtual DateTime Data
        {
            get { return _data; }
            protected set { _data = value; }
        }

        private string[] _trace;
        public virtual string[] Trace
        {
            get { return _trace; }
            protected set { _trace = value; }
        }

        private IList<Excecao> _exceptions;
        public virtual IList<Excecao> Exceptions
        {
            get { return _exceptions; }
            protected set { _exceptions = value; }
        }

        private Excecao _parentException;
        public virtual Excecao ParentException
        {
            get { return _parentException; }
            protected set { _parentException = value; }
        }

        protected virtual IList<string> SetTrace(string value)
        {
            List<string> lLista = new List<string>();
            while (value != null && value.Length > 0)
            {
                int index = value.IndexOf("\r\n");
                if (index < 0)
                {
                    lLista.Add(value.Trim());
                    value = "";
                }
                else
                {
                    lLista.Add(value.Substring(0, index).Trim());
                    value = value.Remove(0, index+2);
                }
            }
            return _trace = lLista.ToArray();            
        }
    }
}
