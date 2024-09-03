CREATE TABLE Balanco (
	BalancoId bigint NOT NULL IDENTITY (1, 1),
	[Hash] uniqueidentifier NOT NULL ROWGUIDCOL,
	DataCadastro datetime NOT NULL,
	DataAtualizacao datetime NOT NULL,
	MoedaId bigint NOT NULL,
	ContaId bigint NOT NULL,
	Saldo money NOT NULL,
	SaldoAlocado money NOT NULL,
	[Status] int NOT NULL
	)  ON [PRIMARY]
GO

ALTER TABLE Balanco ADD CONSTRAINT
	DF_Balanco_Hash DEFAULT (newid()) FOR [Hash]
GO

ALTER TABLE Balanco ADD CONSTRAINT
	PK_Balanco PRIMARY KEY CLUSTERED 
	(
	BalancoId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE Balanco ADD CONSTRAINT
	DF_Balanco_DataCadastro DEFAULT GETDATE() FOR DataCadastro
GO

ALTER TABLE Balanco ADD CONSTRAINT
	DF_Balanco_DataAtualizacao DEFAULT GETDATE() FOR DataAtualizacao
GO

ALTER TABLE Balanco ADD CONSTRAINT
	FK_Balanco_Moeda FOREIGN KEY
	(
	MoedaId
	) REFERENCES dbo.Moeda
	(
	MoedaId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

ALTER TABLE Balanco ADD CONSTRAINT
	FK_Balanco_Conta FOREIGN KEY
	(
	ContaId
	) REFERENCES dbo.Conta
	(
	ContaId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 	
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_MoedaConta] ON [dbo].[Balanco]
(
	[MoedaId] ASC,
	[ContaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE VIEW [dbo].[LancamentosTreepToken]
AS
SELECT        l.ContaId, l.LancamentoId, l.StatusLancamento, l.Valor, c.Url, m.Simbolo, l.Descricao
FROM            dbo.Lancamento AS l INNER JOIN
                         dbo.Carteira AS c ON l.ContaId = c.ContaId AND l.MoedaId = c.MoedaId INNER JOIN
                         dbo.Moeda AS m ON l.MoedaId = m.MoedaId AND c.MoedaId = m.MoedaId
WHERE        (l.MoedaId = 6) AND (l.StatusLancamento = 1)
GO

INSERT INTO Balanco (MoedaId, ContaId, Saldo, SaldoAlocado, Status)
SELECT a.MoedaId, c.ContaId, 0, 0, 1 FROM Conta AS c 
CROSS JOIN Moeda AS a WHERE (NOT EXISTS (SELECT 1 FROM Balanco AS b WHERE (MoedaId = a.MoedaId) AND (ContaId = c.ContaId)));


UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 4285.71428571 WHERE LancamentoId = 2341087;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 10149.85714286 WHERE LancamentoId = 2341085;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 9257.14285714 WHERE LancamentoId = 2341086;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 5271.42857143 WHERE LancamentoId = 2341115;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 7507.50428571 WHERE LancamentoId = 2341116;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 1064.57142857 WHERE LancamentoId = 2341119;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 2925 WHERE LancamentoId = 2341113;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 7738.71428571 WHERE LancamentoId = 2341114;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 9192.85714286 WHERE LancamentoId = 2341103;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 12412.71428571 WHERE LancamentoId = 2341104;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 6750 WHERE LancamentoId = 2341123;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 8592.42857143 WHERE LancamentoId = 2341124;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 2142.85714286 WHERE LancamentoId = 2341080;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 428.57142857 WHERE LancamentoId = 2341081;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 428.57142857 WHERE LancamentoId = 2341082;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 428.57142857 WHERE LancamentoId = 2341083;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 42.85714286 WHERE LancamentoId = 2341084;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 6782.14285714 WHERE LancamentoId = 2341093;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 4145.14285714 WHERE LancamentoId = 2341094;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 441.42857143 WHERE LancamentoId = 2341106;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 578.57142857 WHERE LancamentoId = 2341088;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 17817.42857143 WHERE LancamentoId = 2341089;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 3342.85714286 WHERE LancamentoId = 2341120;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 9392.14285714 WHERE LancamentoId = 2341121;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 6153 WHERE LancamentoId = 2341122;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 6199.28571429 WHERE LancamentoId = 2341129;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 5011.71428571 WHERE LancamentoId = 2341130;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 1350 WHERE LancamentoId = 2341137;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 8577.42857143 WHERE LancamentoId = 2341138;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 8571.42857143 WHERE LancamentoId = 2341139;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 8571.42857143 WHERE LancamentoId = 2341140;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 9321.42857143 WHERE LancamentoId = 2341125;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 3854.14285714 WHERE LancamentoId = 2341126;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 9332.14285714 WHERE LancamentoId = 2341117;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 7344.42857143 WHERE LancamentoId = 2341118;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 405 WHERE LancamentoId = 2341090;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 546.42857143 WHERE LancamentoId = 2341091;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 2220 WHERE LancamentoId = 2341092;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 13761 WHERE LancamentoId = 2341076;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 7810.71428571 WHERE LancamentoId = 2341077;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 5946.42857143 WHERE LancamentoId = 2341097;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 17901.42857143 WHERE LancamentoId = 2341098;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 23245.71428571 WHERE LancamentoId = 2341099;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 867.85714286 WHERE LancamentoId = 2341100;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 932.14285714 WHERE LancamentoId = 2341078;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 22077.85714286 WHERE LancamentoId = 2341079;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 20180.14285714 WHERE LancamentoId = 2341107;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 2442.85714286 WHERE LancamentoId = 2341108;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 1577.57142857 WHERE LancamentoId = 2341109;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 2380.71428571 WHERE LancamentoId = 2341110;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 6720 WHERE LancamentoId = 2341111;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 4962.64285714 WHERE LancamentoId = 2341112;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 4917.85714286 WHERE LancamentoId = 2341074;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 18390 WHERE LancamentoId = 2341075;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 8364.42857143 WHERE LancamentoId = 2341141;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 1350 WHERE LancamentoId = 2341142;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 1269.85714286 WHERE LancamentoId = 2341143;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 15402.85714286 WHERE LancamentoId = 2341105;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 1834.28571429 WHERE LancamentoId = 2341133;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 5697.42857143 WHERE LancamentoId = 2341134;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 5185.71428571 WHERE LancamentoId = 2341127;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 417.85714286 WHERE LancamentoId = 2341128;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 4242.85714286 WHERE LancamentoId = 2341135;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 3831 WHERE LancamentoId = 2341136;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 4036.71428571 WHERE LancamentoId = 2341131;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 417.85714286 WHERE LancamentoId = 2341132;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 19800 WHERE LancamentoId = 2341095;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 9585 WHERE LancamentoId = 2341096;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 428.57142857 WHERE LancamentoId = 2341069;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 149.57142857 WHERE LancamentoId = 2341070;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 12092.79 WHERE LancamentoId = 2341071;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 578.14285714 WHERE LancamentoId = 2341072;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 21455.57571429 WHERE LancamentoId = 2341073;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 2442.85714286 WHERE LancamentoId = 2341101;
UPDATE Lancamento SET StatusLancamento = 1, TransacaoId = NULL, Valor = 3397.41 WHERE LancamentoId = 2341102;

DELETE FROM Transacao WHERE MoedaId = 6;

UPDATE Balanco SET Saldo = 0, SaldoAlocado = 0;