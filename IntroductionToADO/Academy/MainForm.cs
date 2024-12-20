using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Configuration;

namespace Academy
{
	public partial class MainForm : Form
	{
		string connectionString;
		SqlConnection connection;

		Dictionary<string, int> d_groups_directions;
		public MainForm()
		{
			InitializeComponent();
			connectionString = ConfigurationManager.ConnectionStrings["Academy"].ConnectionString;
			//MessageBox.Show(this, connectionString, "Connection string", MessageBoxButtons.OK, MessageBoxIcon.Information);
			connection = new SqlConnection(connectionString);

			LoadStudents();
			LoadGroups();
			LoadDirections();
		}
		void LoadStudents()
		{
			#region Loading
			//string cmd = "SELECT * FROM Students";
			//SqlCommand command = new SqlCommand(cmd, connection);
			//connection.Open();
			//SqlDataReader reader = command.ExecuteReader();

			//if (reader.HasRows)
			//{
			//	DataTable table = new DataTable();
			//	for (int i = 0; i < reader.FieldCount; i++)
			//		table.Columns.Add(reader.GetName(i));
			//	while (reader.Read())
			//	{
			//		DataRow row = table.NewRow();
			//		for (int i = 0; i < reader.FieldCount; i++)
			//		{
			//			row[i] = reader[i];
			//		}
			//		table.Rows.Add(row);
			//	}
			//	dataGridViewStudents.DataSource = table;
			//}

			//reader.Close();
			//connection.Close(); 
			#endregion

			dgvStudents.DataSource = Connector.LoadData
				(
					"last_name AS N'Фамилия', " +
					"first_name AS N'Имя', " +
					"ISNULL(middle_name, N'') AS N'Отчество', " +
					"CONVERT(NVARCHAR, birth_date, 104) AS N'Дата рождения', " +    //https://learn.microsoft.com/ru-ru/sql/t-sql/functions/cast-and-convert-transact-sql?view=sql-server-ver16
					"DATEDIFF(DAY,birth_date,GETDATE())/365 AS N'Возраст', " +
					"group_name AS N'Группа'",
					"Students,Groups",
					"[group]=group_id"
				);
			tslStudentsCount.Text = $"Количетво студентов: {dgvStudents.RowCount - 1}.";
			//dataGridViewStudents.Columns["Дата рождения"].DefaultCellStyle.Format = "yyyy/MM/dd";
		}
		void LoadGroups()
		{
			dgvGroups.DataSource = Connector.LoadData
				(
					"group_id AS 'ID', group_name AS N'Название группы', direction_name AS N'Направление обучения'",
					"Groups,Directions",
					"direction=direction_id"
				);
			tslGroupsCount.Text = $"Количество групп: {dgvGroups.RowCount - 1}.";
		}
		void LoadDirections()
		{

			//DataTable dt_directions = Connector.LoadData("direction_id,direction_name", "Directions");
			d_groups_directions = Connector.LoadPair("direction_name", "direction_id", "Directions");
			cbGroupsDirection.Items.AddRange(d_groups_directions.Keys.ToArray());
			cbGroupsDirection.Items.Insert(0, "Все");
			cbGroupsDirection.SelectedIndex = 0;
		}

		private void cbGroupsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbGroupsDirection.SelectedIndex == 0) LoadGroups();
			else dgvGroups.DataSource = Connector.LoadData
				(
					"group_id, group_name, direction_name",
					"Groups,Directions",
					$"direction=direction_id AND direction={d_groups_directions[cbGroupsDirection.SelectedItem.ToString()]}"
				);
			tslGroupsCount.Text = $"Количество групп: {(dgvGroups.RowCount == 0 ? 0 : dgvGroups.RowCount - 1)}.";
		}
	}
}
