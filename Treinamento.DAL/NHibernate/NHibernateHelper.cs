using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using FluentNHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;

namespace Treinamento.DAL.NHibernate
{
	internal static class NHibernateHelper
	{
		private static ISession _sessao;
		public static ISession Sessao
		{
			get { return _sessao = _sessao ?? OpenSession(); }
		}

		private static Dictionary<String, String> _proxy =
			new Dictionary<string, string> 
			{
				{"NHibernate.ByteCode.Castle", "Castle.Core"},
				{"NHibernate.ByteCode.LinFu", "LinFu.DynamicProxy"}
			};
		private static ISessionFactory _sessionFactory;
		private static Configuration _configuration;
		private static SchemaExport _schema;

		public static SchemaExport Schema
		{
			get { return NHibernateHelper._schema; }
			set { NHibernateHelper._schema = value; }
		}

		public static Configuration Configuration
		{
			get { return NHibernateHelper._configuration; }
			set { NHibernateHelper._configuration = value; }
		}

		private static ISessionFactory SessionFactory(bool createDatabase = false)
		{
			if (_sessionFactory != null && !createDatabase) return _sessionFactory;

			_configuration = new Configuration().Configure(Assembly.GetExecutingAssembly(),
				"Treinamento.DAL.NHibernate.NHibernate." + FactoryDAL.Sgbd + "." + FactoryDAL.Ambiente + ".cfg.xml");

			_configuration = Fluently.Configure(_configuration)
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
				.ExposeConfiguration(x => x.SetInterceptor(new SqlStatementInterceptor()))
				.BuildConfiguration();

			#region Copiando ByteCode

			String proxyFactory = _configuration.GetProperty("proxyfactory.factory_class");

			if (proxyFactory != null)
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				String directory = assembly.CodeBase.Substring(8, assembly.CodeBase.Length - assembly.FullName.IndexOf(',') - 4 - 8);
				Stream objStream;
				FileStream objFileStream;
				byte[] abytResource;

				proxyFactory = proxyFactory.Substring(proxyFactory.IndexOf(',') + 2, proxyFactory.Length - proxyFactory.IndexOf(',') - 2);
				if (!File.Exists(directory + proxyFactory + ".dll"))
				{
					objStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Treinamento.DAL.NHibernate.Resource." + proxyFactory + ".dll");
					abytResource = new Byte[objStream.Length];
					objStream.Read(abytResource, 0, (int)objStream.Length);
					objFileStream = new FileStream(directory + proxyFactory + ".dll", FileMode.Create);
					objFileStream.Write(abytResource, 0, (int)objStream.Length);
					objFileStream.Close();
				}
				if (_proxy.Keys.Contains(proxyFactory)) proxyFactory = _proxy[proxyFactory];
				else proxyFactory = "";
				if (!File.Exists(directory + proxyFactory + ".dll") && !proxyFactory.Equals(""))
				{
					objStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Treinamento.DAL.NHibernate.Resource." + proxyFactory + ".dll");
					abytResource = new Byte[objStream.Length];
					objStream.Read(abytResource, 0, (int)objStream.Length);
					objFileStream = new FileStream(directory + proxyFactory + ".dll", FileMode.Create);
					objFileStream.Write(abytResource, 0, (int)objStream.Length);
					objFileStream.Close();
				}
			}
			#endregion

			_schema = new SchemaExport(_configuration);
			if (createDatabase)
			{
				_schema.Drop(false, true);
				_schema.Create(false, true);
			}

			_sessionFactory = _configuration.BuildSessionFactory();

			//CustomMethods.RegisterAll();

			return _sessionFactory;
		}

		private static ISession OpenSession()
		{
			return SessionFactory().OpenSession();
		}

		public static void CriaDatabase()
		{
			SessionFactory(true);
		}

	}
}