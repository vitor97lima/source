using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Treinamento.DAL.NHibernate;
using Treinamento.DTO.Beneficio;
using Treinamento.DTO.Global;

namespace Treinamento.DAL
{
    public static class FactoryDAL
    {
        private static Int32 _framework;
        private static String _ambiente;
        private static String _sgbd;

        public const Int32 ENTITY_FRAMEWORK = 0;
        public const Int32 NHIBERNATE = 1;
        public const String TESTE = "TESTE";
        public const String PRODUCAO = "PRODUCAO";
        public const String ORACLE = "ORACLE";
        public const String MSSQL = "MSSQL";

        static FactoryDAL()
        {
            _framework = NHIBERNATE;
            _ambiente = TESTE;
            _sgbd = MSSQL;
        }

        public static Int32 Framework
        {
            get { return FactoryDAL._framework; }
        }

        public static String Ambiente
        {
            get { return FactoryDAL._ambiente; }
        }
        
        public static String Sgbd
        {
            get { return FactoryDAL._sgbd; }
        }

        public static IDAL<T> CreateDAL<T>() where T : class
        {
            if (Framework == NHIBERNATE) return new DALNH<T>();
            return null;
        }


    }
}
