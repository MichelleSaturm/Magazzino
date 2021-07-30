using System;
using System.Data.SqlClient;

namespace Magazzino
{
    public static class Report
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Magazzino;Trusted_Connection=True;";
        public static void Q1()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessione stabilita con successo");
                else
                    Console.WriteLine("Connessione fallita");

                Console.Clear();
                Console.WriteLine("===== Q1: L'elenco dei prodotti con giacenza limitata (Quantità Disponibile < 10) =====");

                SqlCommand leggi = new("SELECT * FROM Prodotti " +
                    "WHERE QuantitaDisponibile < 10", conn);

                SqlDataReader reader = leggi.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-20}{4,-20}{5,-10}", "ID", "Cod.Prod.", "Categoria", "Descrizione", "Prezzo", "QTA");
                Console.WriteLine(new String('-', 100));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-20}{4,-20}{5,-10}",
                        reader["ID"],
                        reader["CodiceProdotto"],
                        reader["Categoria"],
                        reader["Descrizione"],
                        reader["PrezzoUnitario"],
                        reader["QuantitaDisponibile"]
                    );

                }
                Console.WriteLine(new String('-', 100));
                Console.WriteLine("Premi un tasto per tornare al menu principale");
                Console.ReadLine();

                conn.Close();
            }
        }

        public static void Q2()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessione stabilita con successo");
                else
                    Console.WriteLine("Connessione fallita");

                Console.Clear();
                Console.WriteLine("===== Q2: Il numero di Prodotti per ogni Categoria =====");

                SqlCommand leggi = new("SELECT Categoria, COUNT(*) AS NumeroProdotti " +
                    "FROM Prodotti " +
                    "GROUP BY Categoria " +
                    "ORDER BY COUNT (*)", conn);

                SqlDataReader reader = leggi.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("{0,-15}{1,-5}", "Categoria", "N°");
                Console.WriteLine(new String('-', 50));

                while (reader.Read())
                {
                    Console.WriteLine("{0,-15}{1,-5}",
                        reader["Categoria"],
                        reader["NumeroProdotti"]
                    );

                }
                Console.WriteLine(new String('-', 50));
                Console.WriteLine("Premi un tasto per tornare al menu principale");
                Console.ReadLine();

                conn.Close();
            }
        }
    }
}
