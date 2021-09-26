using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Auraluminous.Common
{
	public static class Reflection
	{
		private static Assembly[] mvarAvailableAssemblies = null;
		public static Assembly[] GetAvailableAssemblies()
		{
			if (mvarAvailableAssemblies == null)
			{
				List<Assembly> list = new List<Assembly>();
				string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				string[] filenames = System.IO.Directory.GetFiles(basePath, "*.dll", System.IO.SearchOption.AllDirectories);
				foreach (string filename in filenames)
				{
					Assembly asm = null;
					try
					{
						asm = Assembly.LoadFile(filename);
					}
					catch
					{

					}
					if (asm != null) list.Add(asm);
				}
				mvarAvailableAssemblies = list.ToArray();
			}
			return mvarAvailableAssemblies;
		}

		private static Type[] mvarAvailableTypes = null;
		public static Type[] GetAvailableTypes(Type[] inheritsFromTypes = null)
		{
			if (mvarAvailableTypes == null)
			{
				Assembly[] asms = GetAvailableAssemblies();
				List<Type> list = new List<Type>();
				foreach (Assembly asm in asms)
				{
					Type[] types = null;
					try
					{
						types = asm.GetTypes();
					}
					catch (ReflectionTypeLoadException ex)
					{
						types = ex.Types;
					}

					foreach (Type type in types)
					{
						if (type == null) continue;
						list.Add(type);
					}
				}
				mvarAvailableTypes = list.ToArray();
			}

			List<Type> reallist = new List<Type>();
			foreach (Type type in mvarAvailableTypes)
			{
				foreach (Type inheritsType in inheritsFromTypes)
				{
					if (!type.IsAbstract && type.IsSubclassOf(inheritsType)) reallist.Add(type);
				}
			}
			return reallist.ToArray();
		}

		public static T[] GetAvailableInstances<T>()
		{
			List<T> list = new List<T>();
			Type[] types = GetAvailableTypes(new Type[] { typeof(T) });
			foreach (Type type in types)
			{
				T inst = (T)type.Assembly.CreateInstance(type.FullName);
				list.Add(inst);
			}
			return list.ToArray();
		}
	}
}
