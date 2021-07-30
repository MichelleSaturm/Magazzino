-- CREAZIONE TABELLA

CREATE TABLE Prodotti (
ID int IDENTITY (1,1) NOT NULL
,CodiceProdotto nvarchar (10) NOT NULL UNIQUE
,Categoria nvarchar(50) NOT NULL
,Descrizione nvarchar(500) NOT NULL
,PrezzoUnitario decimal (10,2)
,QuantitaDisponibile int NOT NULL
,CONSTRAINT PK_Prodotti PRIMARY KEY (ID)
)

--QUERY
--l'elenco dei prodotti con giacenza limitata (Quantità Disponibile < 10)

SELECT * FROM Prodotti
WHERE QuantitaDisponibile < 10

-- Il numero di Prodotti per ogni Categoria

SELECT 
	Categoria,
	COUNT (*) AS NumeroProdotti
FROM Prodotti
GROUP BY Categoria