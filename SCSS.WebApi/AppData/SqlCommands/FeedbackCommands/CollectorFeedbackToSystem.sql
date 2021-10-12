;WITH CollectorFeedbackToSystem AS (
	SELECT A.[Id] AS [FeedbackId],
		   B.[TransactionCode] AS [TransactionCode],
		   A.[SellingFeedback] AS [FeedbackContent],
		   C.[Name] AS [SellingAccountName],
		   C.[Phone] AS [SellingAccountPhone],
		   D.[Name] AS [BuyingAccountName],
		   E.[DealerName] AS [DealerName],
		   E.[DealerPhone] AS [DealerPhone],
		   A.[AdminReply] AS [RepliedContent],
		   A.[CreatedTime] AS [CreatedTime]
	FROM [FeedbackToSystem] A JOIN [CollectDealTransaction] B ON A.[CollectDealTransactionId] = B.[Id]
							  JOIN [Account] C ON A.[SellingAccountId] = C.[Id]
							  JOIN [Account] D ON A.[BuyingAccountId] = D.[Id]
							  JOIN [DealerInformation] E ON D.[Id] = E.[DealerAccountId]
	),
	TotalRecord AS (
		SELECT COUNT([FeedbackId]) AS [TotalRecord] FROM [CollectorFeedbackToSystem]
	)
SELECT A.[FeedbackId],
       A.[TransactionCode],
	   A.[FeedbackContent],
	   A.[SellingAccountName],
	   A.[SellingAccountPhone],
	   A.[BuyingAccountName],
	   A.[DealerName],
	   A.[DealerPhone],
	   A.[RepliedContent],
	   B.[TotalRecord]
FROM [CollectorFeedbackToSystem] A CROSS JOIN [TotalRecord] B
ORDER BY A.[CreatedTime] DESC OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY

