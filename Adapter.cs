using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace DBExample.DBAccess_Provider
{
	class Adapter
	{
		internal static void Insert(string Name)
		{
			Tools.Adapter.InsertCommand.Parameters[0].Value = Name;
			Tools.Adapter.InsertCommand.Parameters[1].Value = "test";
			Tools.Adapter.InsertCommand.Parameters[2].Value = 5;

			try
			{
				Tools.connexion.Open();

				int buffer = Tools.Adapter.InsertCommand.ExecuteNonQuery();

				if (buffer != 0) MessageBox.Show("User successfully inserted");
				else MessageBox.Show("User not inserted");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				Tools.connexion.Close();
			}
		}
		internal static void Delete(String Name)
		{
			Tools.Adapter.DeleteCommand.Parameters[0].Value = Name;

			try
			{
				Tools.connexion.Open();

				int buffer = Tools.Adapter.DeleteCommand.ExecuteNonQuery();

				if (buffer != 0) MessageBox.Show("User successfully deleted");
				else MessageBox.Show("User not found");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				Tools.connexion.Close();
			}
		}
		internal static void Fill(DataSet dataset,string TableName, string DB_Table)
		{
			dataset.Tables[TableName].Clear();

			Tools.Adapter.SelectCommand = new OleDbCommand("SELECT * from " + DB_Table + ";", Tools.connexion);
			
			try
			{
				Tools.connexion.Open();

				Tools.Adapter.Fill(dataset.Tables[TableName]);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				Tools.connexion.Close();
			}
		}
		internal static void Update(DataTable Table)
		{
			try
			{
				Tools.connexion.Open();

				Tools.Adapter.Update(Table);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				Tools.connexion.Close();
			}
		}
	}
}
