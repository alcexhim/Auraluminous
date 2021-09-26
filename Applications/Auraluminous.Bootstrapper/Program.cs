using System;
using System.Collections.Generic;
using System.Reflection;
using MBS.Framework.UserInterface;
using MBS.Framework.UserInterface.Dialogs;
using UniversalEditor.UserInterface;

namespace Auraluminous.Bootstrapper
{
	class Program : EditorApplication
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			(new Program()).Start();
		}
	}
}
