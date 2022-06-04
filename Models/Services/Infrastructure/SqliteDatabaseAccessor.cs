using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MyCourse.Models.Services.Infrastructure
{
    public class SqliteDatabaseAccessor : IDatabaseAccessor
    {
        
        public async Task <DataSet> QueryAsync(FormattableString formattableQuery)
        {
            var queryArguments = formattableQuery.GetArguments();
            var sqliteParameters = new List<SqliteParameter>();
            for(var i = 0; i < queryArguments.Length; i++)
            {
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]);
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@" + i;

            } 
            string query = formattableQuery.ToString();
            //Connessione al DataBase//
            //*************************************************************************************//
            using (var conn = new SqliteConnection("Data Source=Data/MyCourse.db"))
            {

                    await conn.OpenAsync();
                    //************************************************************************************//
                    //Query verso il database con SqliteCommand che ha due argomenti stringa: la query da fare e la Connessione
                    //***********************************************************************************//
                    //Quando utilizziamo il metoto dispose è necessario utilizzare using per impedire eccezioni che impedirebbero la chiusura del database
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(sqliteParameters);
                    //Con il metodo ExecuteReader, leggiamo i dati della query che abbiamo effettuato
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            //Il dataSet è una collezione di DataTable
                            var dataSet = new DataSet();
                            //C'era un bug Microsoft che creava impedimento. Con la riga sotto riportata si riusciva a raggirare il problema
                            // dataSet.EnforceConstraints = false;
                            do
                            {
                               var dataTable = new DataTable();
                               dataSet.Tables.Add(dataTable);
                               dataTable.Load(reader);

                            }while (!reader.IsClosed);
                            
                            return dataSet;

                        }
                    }
            }
           
        }
    }
}