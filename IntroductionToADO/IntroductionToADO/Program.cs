using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IntroductionToADO
{
	class Program
	{
		static void Main(string[] args)
		{
			//ADO - ActiveX Data Objects
			//MS SQL Server, Oracle, SQLite....
			//SQL Server Object Explorer
			string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			Console.WriteLine(connectionString);
			Console.WriteLine("\n------------------------------\n");

			SqlConnection connection = new SqlConnection(connectionString);
			string cmd = "SELECT * FROM Authors";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();
			const int padding = 16;
			for (int i = 0; i < reader.FieldCount; i++)
				Console.Write(reader.GetName(i).PadRight(padding));
			Console.WriteLine();

			if (!reader.IsClosed)
			{
				while (reader.Read())
				{
					for (int i = 0; i < reader.FieldCount; i++)
					{
						Console.Write(reader[i].ToString().PadRight(padding));
					}
					Console.WriteLine();
				}
			}

			reader.Close();
			connection.Close();
		}
	}
}
