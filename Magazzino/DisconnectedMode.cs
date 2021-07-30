using System;
using System.Data;
using System.Data.SqlClient;

namespace Magazzino
{
    public static class DisconnectedMode
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Magazzino;Trusted_Connection=True;";

        static DataSet dsMagazzino = new DataSet();

        public static void ElencoProdotti()
        {
            Console.Clear();
            StampaElencoProdotti();
            Console.WriteLine("Premi un tasto per tornare al Pannello di Controllo");
            Console.ReadLine();

        }

        public static void StampaElencoProdotti()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Impossibile stabilire la connessione");

                DataSet dsMagazzino = new DataSet();

                SqlDataAdapter magazzinoAdapter = new SqlDataAdapter();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                magazzinoAdapter.SelectCommand = selectProdotti;

                magazzinoAdapter.Fill(dsMagazzino, "Magazzino");

                conn.Close();

                Console.WriteLine("==== ELENCO PRODOTTI ====");
                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-20}{4,-20}{5,-10}", "ID", "Cod.Prod.", "Categoria", "Descrizione", "Prezzo", "QTA");
                Console.WriteLine(new String('-', 100));
                foreach (DataRow row in dsMagazzino.Tables["Magazzino"].Rows)
                {

                    Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-20}{4,-20}{5,-10}",
                                  row["ID"],
                                  row["CodiceProdotto"],
                                  row["Categoria"],
                                  row["Descrizione"],
                                  row["PrezzoUnitario"],
                                  row["QuantitaDisponibile"]);
                }
                Console.WriteLine(new String('-', 100));
            }
        }

        public static void InserisciProdotto()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Impossibile stabilire la connessione");


                SqlDataAdapter magazzinoAdapter = new();
                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);
                magazzinoAdapter.SelectCommand = selectProdotti;

                magazzinoAdapter.InsertCommand = GetMagazzinoInsertCommand(conn);

                magazzinoAdapter.Fill(dsMagazzino, "Magazzino");

                conn.Close();

                DataRow newRow = dsMagazzino.Tables["Magazzino"].NewRow();

                Console.Clear();
                Console.WriteLine("===== INSERIMENTO NUOVO PRODOTTO =====");


                Console.WriteLine("Codice Prodotto:");
                string codice = Console.ReadLine();
                Console.WriteLine("Categoria:");
                string categoria = Console.ReadLine();
                Console.WriteLine("Descrizione:");
                string descrizione = Console.ReadLine();
                Console.WriteLine("Prezzo Unitario:");
                decimal prezzo = Helpers.CheckDecimal();
                Console.WriteLine("Quantità Disponibile:");
                int qta = Helpers.CheckInt();

                newRow["CodiceProdotto"] = codice;
                newRow["Categoria"] = categoria;
                newRow["Descrizione"] = descrizione;
                newRow["PrezzoUnitario"] = prezzo;
                newRow["QuantitaDisponibile"] = qta;

                dsMagazzino.Tables["Magazzino"].Rows.Add(newRow);


                magazzinoAdapter.Update(dsMagazzino, "Magazzino");
            }

            Console.WriteLine("Inserimento del prodotto eseguito con successo");
            Console.WriteLine();
            Console.WriteLine("Premi un tasto per tornare al Pannello di Controllo");
            Console.ReadLine();

        }

        public static void EliminaProdotto()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Impossibile stabilire la connessione");

                SqlDataAdapter magazzinoAdapter = new();

                magazzinoAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);
                magazzinoAdapter.SelectCommand = selectProdotti;

                magazzinoAdapter.DeleteCommand = GetProdottoDeleteCommand(conn);

                magazzinoAdapter.Fill(dsMagazzino, "Magazzino");

                conn.Close();

                Console.Clear();
                Console.WriteLine("===== ELIMINAZIONE PRODOTTO =====");

                Console.WriteLine("==== ELENCO PRODOTTI ====");
                Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-20}", "ID", "Cod.Prod.", "Categoria", "Descrizione");
                Console.WriteLine(new String('-', 60));
                foreach (DataRow row in dsMagazzino.Tables["Magazzino"].Rows)
                {
                    Console.WriteLine("{0,-5}{1,-15}{2,-20}{3,-20}",
                                  row["ID"],
                                  row["CodiceProdotto"],
                                  row["Categoria"],
                                  row["Descrizione"]);
                }
                Console.WriteLine(new String('-', 60));

                Console.WriteLine("Inserisci ID del Prodotto da Eliminare: ");
                string id = Console.ReadLine();

                DataRow rowToDelete = dsMagazzino.Tables["Magazzino"].Rows.Find(id);

                if (rowToDelete != null)
                {
                    rowToDelete.Delete();

                    magazzinoAdapter.Update(dsMagazzino, "Magazzino");
                    Console.WriteLine("Eliminazione del prodotto esequita con successo");
                    Console.WriteLine();
                    Console.WriteLine("Premi un tasto per tornare al Pannello di Controllo");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Non è presente nessun prodotto con ID {id}.\n" +
                          $"Premi un tasto per tornare al Pannello di Controllo");
                    Console.ReadLine();
                }

            }
        }

        public static void ModificaProdotto()
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Impossibile stabilire la connessione");


                SqlDataAdapter magazzinoAdapter = new();

                magazzinoAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);
                magazzinoAdapter.SelectCommand = selectProdotti;

                magazzinoAdapter.UpdateCommand = GetProdottiUpdateCommand(conn);

                magazzinoAdapter.Fill(dsMagazzino, "Magazzino");

                conn.Close();

                Console.Clear();

                StampaElencoProdotti();

                Console.WriteLine("===== MODIFICA PRODOTTO =====");
                Console.WriteLine("Inserisci ID del Prodotto da modificare:");
                string id = Console.ReadLine();

                DataRow rowToChange = dsMagazzino.Tables["Magazzino"].Rows.Find(id);

                if (rowToChange != null)
                {
                    bool scelta = true;
                    do
                    {
                        Console.WriteLine("Cosa vuoi modificare?");
                        Console.WriteLine("[1] Categoria");
                        Console.WriteLine("[2] Descrizione");
                        Console.WriteLine("[3] Prezzo");
                        Console.WriteLine("[4] Quantità Disponibile");
                        int choice;
                        bool isInt;
                        do
                        {
                            isInt = int.TryParse(Console.ReadLine(), out choice);
                        } while (!isInt);

                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Stai modificando la Categoria.\n" +
                                "Inserisci la Categoria corretta/nuova:");
                                string categoria = Console.ReadLine();
                                rowToChange["Categoria"] = categoria;
                                scelta = false;
                                break;
                            case 2:
                                Console.WriteLine("Stai modificando la Descrizione.\n" +
                                "Inserisci la Descrizione corretta/nuova:");
                                string descrizione = Console.ReadLine();
                                rowToChange["Descrizione"] = descrizione;
                                scelta = false;
                                break;
                            case 3:
                                Console.WriteLine("Stai modificando il Prezzo.\n" +
                                "Inserisci il Prezzo corretto/nuovo:");
                                decimal prezzo = Helpers.CheckDecimal();
                                rowToChange["PrezzoUnitario"] = prezzo;
                                scelta = false;
                                break;
                            case 4:
                                Console.WriteLine("Stai modificando la Quantita.\n" +
                                "Inserisci la Quantità corretta/nuova:");
                                int qta = Helpers.CheckInt();
                                rowToChange["QuantitaDisponibile"] = qta;
                                scelta = false;
                                break;
                            default:
                                Console.WriteLine("La scelta è sbagliata. Riprova.");
                                break;
                        }
                    } while (scelta);

                    magazzinoAdapter.Update(dsMagazzino, "Magazzino");
                    Console.WriteLine("Modifica del prodotto esequita con successo");
                    Console.WriteLine();
                    Console.WriteLine("Premi un tasto per tornare al Pannello di Controllo");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Non è presente nessun prodotto con ID {id}.\n" +
                          $"Premi un tasto per tornare al Pannello di Controllo");
                    Console.ReadLine();
                }


            }
        }


        #region SUPPORT METHOD
        private static SqlCommand GetMagazzinoInsertCommand(SqlConnection conn)
        {
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO Prodotti VALUES(@codiceProdotto, @categoria, @descrizione, @prezzoUnitario, @quantitaDisponibile)";

            insertCommand.CommandType = System.Data.CommandType.Text;

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@codiceProdotto",
                    SqlDbType.NVarChar,
                    10,
                    "CodiceProdotto"
                    )
                );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@categoria",
                    SqlDbType.NVarChar,
                    50,
                    "Categoria"
                    )
                );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@descrizione",
                    SqlDbType.NVarChar,
                    500,
                    "Descrizione"
                    )
                );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@prezzoUnitario",
                    SqlDbType.Decimal,
                    10,
                    "PrezzoUnitario"
                    )
                );

            insertCommand.Parameters.Add(
                    new SqlParameter(
                    "@quantitaDisponibile",
                    SqlDbType.Int,
                    50,
                    "QuantitaDisponibile"
                    )
                );

            return insertCommand;
        }

        private static SqlCommand GetProdottiUpdateCommand(SqlConnection conn)
        {
            SqlCommand updateCommand = new SqlCommand();
            updateCommand.Connection = conn;

            updateCommand.CommandText = "UPDATE Prodotti " +
                "SET Categoria = @categoria, Descrizione = @descrizione, PrezzoUnitario = @prezzoUnitario, QuantitaDisponibile = @quantitaDisponibile " +
                "WHERE ID = @id";

            updateCommand.CommandType = System.Data.CommandType.Text;

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@id",
                    SqlDbType.Int,
                    50,
                    "ID"
                    )
                );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@categoria",
                    SqlDbType.NVarChar,
                    50,
                    "Categoria"
                    )
                );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@descrizione",
                    SqlDbType.NVarChar,
                    500,
                    "Descrizione"
                    )
                );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@prezzoUnitario",
                    SqlDbType.Decimal,
                    10,
                    "PrezzoUnitario"
                    )
                );

            updateCommand.Parameters.Add(
                    new SqlParameter(
                    "@quantitaDisponibile",
                    SqlDbType.Int,
                    50,
                    "QuantitaDisponibile"
                    )
                );

            return updateCommand;
        }

        private static SqlCommand GetProdottoDeleteCommand(SqlConnection conn)
        {
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.Connection = conn;

            deleteCommand.CommandText = "DELETE FROM Prodotti " +
                "WHERE ID = @id";

            deleteCommand.CommandType = System.Data.CommandType.Text;

            deleteCommand.Parameters.Add(
                new SqlParameter(
                    "@id",
                    SqlDbType.Int,
                    50,
                    "ID"
                    )
                );

            return deleteCommand;
        }

        #endregion
    }
}
