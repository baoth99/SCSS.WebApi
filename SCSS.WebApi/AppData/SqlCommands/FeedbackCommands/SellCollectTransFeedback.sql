;WITH SellCollectTransFeedback AS (
		SELECT A.[Id] AS [FeedbackId],
			   C.[CollectingRequestCode] AS [TransactionCode],
			   D.[Name] AS [SellerName],
			   D.[Phone] AS [SellerPhone],
			   E.[Name] AS [CollectorName],
			   E.[Phone] AS [CollectorPhone],
			   A.[SellingReview] AS [FeedbackContent],
			   A.[Rate] AS [Rate],
			   A.[CreatedTime] AS [CreatedTime]
		FROM [Feedback] A JOIN [SellCollectTransaction] B ON A.SellCollectTransactionId = B.Id
						  JOIN [CollectingRequest] C ON B.CollectingRequestId = C.Id
						  JOIN [Account] D ON C.SellerAccountId = D.Id
						  JOIN [Account] E ON C.CollectorAccountId = E.Id
		WHERE (@TransactionCode IS NULL OR C.CollectingRequestCode LIKE '%' + @TransactionCode + '%') AND
			  (@SellerName IS NULL OR D.[Name] LIKE '%' + @SellerName + '%') AND
			  (@CollectorName IS NULL OR E.[Name] LIKE '%' + @CollectorName + '%') AND
			  (@Rate = 0 OR A.[Rate] = @Rate)
	),
	TotalRecord AS (
		SELECT COUNT([FeedbackId]) AS [TotalRecord] FROM [SellCollectTransFeedback]
	)
SELECT A.[TransactionCode],
	   A.[SellerName],
	   A.[SellerPhone],
	   A.[CollectorName],
	   A.[CollectorName],
	   A.[FeedbackContent],
	   A.[Rate],
	   B.[TotalRecord] 
FROM [SellCollectTransFeedback] A CROSS JOIN [TotalRecord] B
ORDER BY A.[CreatedTime] DESC OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY
	
