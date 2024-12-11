using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BYS_Poject.controllers
{
    public class SglConnectionClass
    {
            public static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-9MRP4SI\\SQLEXPRESS;Initial Catalog=BYS.data;Integrated Security=True;Encrypt=False");

            public static void CheckConnection()
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                else { }
            }
        }
    }

        