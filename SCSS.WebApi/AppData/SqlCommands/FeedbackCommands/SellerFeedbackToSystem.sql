;WITH SellerFeedbackToSystem AS (
		SELECT A.[Id] AS [FeedbackId],
			   B.[CollectingRequestCode] AS [CollectingRequestCode],
			   A.[SellingFeedback] AS [FeedbackContent],
			   C.[Name] AS [SellingAccountName],
			   C.[Phone] AS [SellingAccountPhone],
			   D.[Name] AS [BuyingAccountName],
			   D.[Phone] AS [BuyingAccountPhone],
			   A.[AdminReply] AS [RepliedContent],
			   A.[CreatedTime] AS [CreatedTime]
		FROM [FeedbackToSystem] A JOIN [CollectingRequest] B ON A.[CollectingRequestId] = B.[Id]
							      JOIN [Account] C ON A.[SellingAccountId] = C.[Id]
								  LEFT JOIN [Account] D ON A.[BuyingAccountId] = D.[Id]
		WHERE (@TransactionCode IS NULL OR B.[CollectingRequestCode] LIKE '%' + @TransactionCode + '%') AND
			  (@SellerName IS NULL OR C.[Name] LIKE '%' + @SellerName + '%') AND
			  (@CollectorName IS NULL OR D.[Name] LIKE '%' + @CollectorName + '%') AND
			  (@CollectorPhone IS NULL OR D.[Phone] LIKE '%' + @CollectorPhone + '%')
	),
	TotalRecord AS (
		SELECT COUNT([FeedbackId]) AS [TotalRecord] FROM [SellerFeedbackToSystem]
	)
SELECT A.[FeedbackId],
	   A.[CollectingRequestCode],
	   A.[FeedbackContent],
	   A.[SellingAccountName],
	   A.[SellingAccountPhone],
	   A.[BuyingAccountName],
	   A.[BuyingAccountPhone],
	   A.[RepliedContent],
	   B.[TotalRecord],
	   A.[CreatedTime]
FROM [SellerFeedbackToSystem] A CROSS JOIN [TotalRecord] B
ORDER BY A.[CreatedTime] DESC OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY


