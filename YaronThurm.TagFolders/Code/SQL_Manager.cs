using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace YaronThurm.TagFolders
{
    public class SQL_Manager
    {
        private string connectionString = "server=localhost\\SQLEXPRESS;" +
            "Trusted_Connection=yes;" +
            "database=Files_and_Tags_2; " +
            "connection timeout=10";
        
        public SQL_Manager(TagFilesDatabase database)
        {            
            if (false)
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = this.connectionString;
                connection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = connection;
                int fileIndex = 1;
                int tagIndex = 0;
                foreach (FileWithTags file in database.Files)
                {
                    foreach (FileTag tag in file.Tags)
                    {
                        tagIndex = database.Tags.IndexOf(tag) + 1;

                        myCommand.CommandText = "INSERT INTO Files_Tags ([File ID], [Tag ID]) " +
                            "Values (" + fileIndex.ToString() + ", " + tagIndex.ToString() + ")";

                        myCommand.ExecuteNonQuery();
                    }                                        
                    fileIndex++;
                }
                connection.Close();
            }            
        }

        public void GetFilesByTag(List<string> tags)
        {
            return;

            // Create SQL query. Where section
            string whereString = "where Tag like ";
            for (int i = 0; i < tags.Count; i++)
               if (i < tags.Count - 1)
                    whereString += "'" + tags[i].ToString() + "' or Tag like ";
                else
                    whereString += "'" + tags[i].ToString() + "'";

            string query1 = 
                "select [File Name] from [Files] where ID in " +
                "(select [File ID] from [Files_Tags] where [Tag ID] in " +
                "(select ID from [Tags] " + whereString + "))";
            
            string query2 = 
                "select Files.[File Name] from Tags " +
                "inner join Files_Tags on (Files_Tags.[Tag ID]=Tags.ID) " +
                "inner join Files on Files.ID=Files_Tags.[File ID] " + whereString;

            string queryString = query1;
            List<string> files = new List<string>();
                        
            // Open connection to the databse
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = this.connectionString;
            connection.Open();
            
            // Create command
            SqlCommand myCommand = new SqlCommand();
            myCommand.Connection = connection;
            myCommand.CommandText = queryString;

            // Record the time
            TimeSpan span = new TimeSpan();
            DateTime start = DateTime.Now;
            // Loop the query
            for (int j = 0; j < 200; j++)
            {                                                
                // Execute command and start reading                
                SqlDataReader reader = null;
                reader = myCommand.ExecuteReader();
                files.Clear();
                while (reader.Read())
                    files.Add(reader["File Name"].ToString());
                reader.Close();
            }
            // Close connection
            connection.Close();

            DateTime stop = DateTime.Now;
            span = stop - start;
            Console.WriteLine(span);

        }
    }
}
