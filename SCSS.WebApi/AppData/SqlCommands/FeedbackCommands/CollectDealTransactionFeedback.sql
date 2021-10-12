;WITH CollectDealTransactionFeedback AS (
		SELECT A.[Id] AS [FeedbackId],
			   B.[TransactionCode] AS [TransactionCode],
			   C.[DealerName] AS [DealerName],
			   C.[DealerPhone] AS [DealerPhone],
			   D.[Name] AS [DealerAccountName],
			   D.[Phone] AS [DealerAccountPhone],
			   E.[Name] AS [CollectorName],
			   E.[Phone] AS [CollectorPhone],
			   A.SellingReview AS [FeedbackContent],
			   A.[Rate] AS [Rate],
			   A.[CreatedTime] AS [CreatedTime]
		FROM [Feedback] A JOIN [CollectDealTransaction] B ON A.[CollectDealTransactionId] = B.[Id]
						  JOIN [DealerInformation] C ON B.[DealerAccountId] = C.[DealerAccountId]
						  JOIN [Account] D ON B.[DealerAccountId] = D.[Id]
						  JOIN [Account] E ON B.[CollectorAccountId] = E.Id
		WHERE (@TransactionCode IS NULL OR B.[TransactionCode] LIKE '%'+ @TransactionCode +'%') AND
			  (@DealerName IS NULL OR C.[DealerName] LIKE '%' + @DealerName + '%') AND
			  (@CollectorName IS NULL OR E.[Name] LIKE '%' + @CollectorName + '%') AND
			  (@Rate = 0 OR A.[Rate] = @Rate)

	  ),
	  TotalRecord AS (
		SELECT COUNT([FeedbackId]) AS [TotalRecord] FROM [CollectDealTransactionFeedback]
	  )
SELECT A.[TransactionCode],
	   A.[DealerName],
	   A.[DealerPhone],
	   A.[DealerAccountName],
	   A.[DealerAccountPhone],
	   A.[CollectorName],
	   A.[CollectorPhone],
	   A.[FeedbackContent],
	   A.[Rate],
	   B.[TotalRecord]
FROM [CollectDealTransactionFeedback] A CROSS JOIN [TotalRecord] B
ORDER BY A.[CreatedTime] DESC OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY

