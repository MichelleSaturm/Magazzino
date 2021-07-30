using System;

namespace Magazzino
{
    public static class PannelloDiControllo
    {
        public static void Start()
        {
            bool continuare = true;

            do
            {
                Console.Clear();
                Console.WriteLine("======= PANNELLO DI CONTROLLO =======");
                Console.WriteLine("[1] Elenco Prodotti");
                Console.WriteLine("[2] Inserisci Prodotto");
                Console.WriteLine("[3] Modifica Prodotto");
                Console.WriteLine("[4] Elimina Prodotto");
                Console.WriteLine("[5] Query");
                Console.WriteLine();
                Console.WriteLine("[0] Uscita");
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Inserisci la sua scelta: ");

                int choice;
                bool isInt;
                do
                {
                    isInt = int.TryParse(Console.ReadLine(), out choice);
                } while (!isInt);


                switch (choice)
                {
                    case 1:
                        DisconnectedMode.ElencoProdotti();
                        break;
                    case 2:
                        DisconnectedMode.InserisciProdotto();
                        break;
                    case 3:
                        DisconnectedMode.ModificaProdotto();
                        break;
                    case 4:
                        DisconnectedMode.EliminaProdotto();
                        break;
                    case 5:
                        PannelloDiControllo.Query();
                        break;
                    case 0:
                        continuare = false;
                        break;
                    default:
                        Console.WriteLine("La scelta è sbagliata. Riprova.");
                        break;
                }
            } while (continuare);
        }

        public static void Query()
        {
            Console.Clear();
            Console.WriteLine("======= QUERY =======");
            Console.WriteLine("[1] L'elenco dei prodotti con giacenza limitata (Quantità Disponibile < 10)");
            Console.WriteLine("[2] Il numero di Prodotti per ogni Categoria");
            Console.WriteLine();
            Console.WriteLine("Inserisci la sua scelta, oppure un tasto qualsiasi per tornare al Pannello di Controllo");

            int choice;
            bool isInt;
            do
            {
                isInt = int.TryParse(Console.ReadLine(), out choice);
            } while (!isInt);


            switch (choice)
            {
                case 1:
                    Report.Q1();
                    break;
                case 2:
                    Report.Q2();
                    break;
                default:
                    break;
            }
        }

    }
}
