SELECT        l.ContaId, l.LancamentoId, l.StatusLancamento, l.Valor, c.Url, m.Simbolo, l.Descricao

FROM            dbo.Lancamento AS l INNER JOIN

                         dbo.Carteira AS c ON l.ContaId = c.ContaId AND l.MoedaId = c.MoedaId INNER JOIN

                         dbo.Moeda AS m ON l.MoedaId = m.MoedaId AND c.MoedaId = m.MoedaId

WHERE        (l.MoedaId = 6) AND (l.StatusLancamento = 1)

#############################################################################

CREATE VIEW LancamentosTreepToken AS
SELECT        l.ContaId, l.LancamentoId, l.StatusLancamento, l.Valor, CASE WHEN l.MoedaId = 6 THEN l.Valor ELSE 0 END AS Unstaked,
                             (SELECT        Url
                               FROM            dbo.Carteira
                               WHERE        (ContaId = l.ContaId) AND (MoedaId = 6)) AS Expr1, m.MoedaId, m.Simbolo, l.Descricao
FROM            dbo.Lancamento AS l INNER JOIN
                         dbo.Moeda AS m ON l.MoedaId = m.MoedaId
WHERE        (m.MoedaId IN (6, 7)) AND (l.TipoLancamento = 4) AND (l.TransacaoId IS NULL);