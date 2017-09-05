//using Assets.Code.ConcreteClasses.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using UnityEngine;

//namespace Assets.Code.Data
//{
//	public class BeatenLevelCommunicator
//	{
//		private string host = "sometest.cjhuduo1mo61.us-west-1.rds.amazonaws.com,1433";
//		private string id = "RoctagonsGuest";
//		private string password = "SomeRandomPassword";
//		private string database = "tempdb";

//		public void SendBeatenLevel(string uniqueId, int moves, float timeTaken)
//		{
//			try
//			{
//				using (SqlConnection myConnection = new SqlConnection())
//				{
//					myConnection.ConnectionString = GenerateConnectionString();
//					myConnection.Open();

//					SqlCommand myCommand = new SqlCommand("INSERT INTO BeatenLevel(DeviceName, Moves, TimeTaken) Values ('testDevice123avd', 1, 1.2)");
//					myCommand.Connection = myConnection;
//					myCommand.ExecuteNonQuery();

//					myConnection.Close();
//				}
//			}
//			catch(Exception ex)
//			{
//				Debug.Log("Failed to insert level");
//			}
			
//		}

//		public List<BeatenLevelDTO> GetClosestLevels()
//		{
//			throw new NotImplementedException();
//		}

//		private string GenerateConnectionString()
//		{
//			return String.Format("Data Source={0}; Initial Catalog={1}; Integrated Security=false; User Id=RoctagonsGuest; password=A1B2C3", host, database);
//		}
//	}
//}
