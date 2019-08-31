using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductApp
{
	class Class1
	{
		public static string ConnectionString()
		{
			return ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
		}
	}
}
