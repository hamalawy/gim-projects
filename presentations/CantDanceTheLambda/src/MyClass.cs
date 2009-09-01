using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using System.Data.SqlClient;

namespace CantDanceTheLambda {
    public class MyClass {

        public void Main() {
            SqlConnection conn = new SqlConnection("connection-string");
            try {
                var cmd = new SqlCommand("SELECT * FROM Employees", conn);
                conn.Open();
                var r = cmd.ExecuteReader();
            }
            finally {
                conn.Dispose();
            }

            StreamReader streamReader = new StreamReader("file-path");
            try {
                streamReader.ReadLine();
            }
            finally {
                streamReader.Dispose();
            }
        }
    }
}
