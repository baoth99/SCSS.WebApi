;WITH DealerComplaintData AS (
		SELECT A.[Id] AS [DealerComplaintId],
			   C.[TransactionCode] AS [CollectDealTransactionCode],
			   D.[Name] AS [SellingAccountName],
			   D.[Phone] AS [SellingAccountPhone],
			   E.[Name] AS [BuyingAccountName],
			   E.[Phone] AS [BuyingAccountPhone],
			   F.[DealerName] AS [DealerName],
			   F.[DealerPhone] AS [DealerPhone],
			   A.[ComplaintContent] AS [ComplaintContent],
			   A.[AdminReply] AS [RepliedContent],
			   A.[CreatedTime] AS [CreatedTime]
		FROM [DealerComplaint] A JOIN [Complaint] B ON A.[ComplaintId] = B.[Id]
								 JOIN [CollectDealTransaction] C ON B.[CollectDealTransactionId] = C.[Id]
								 JOIN [Account] D ON C.[CollectorAccountId] = D.[Id]
								 JOIN [Account] E ON C.[DealerAccountId] = E.[Id]
								 JOIN [DealerInformation] F ON E.[Id] = F.[DealerAccountId]
		WHERE (@DealerPhone IS NULL OR F.[DealerPhone] LIKE '%' + @DealerPhone + '%') AND
			  (@DealerName IS NULL OR F.[DealerName] LIKE '%' + @DealerName + '%') 
	),
	TotalRecord AS (
		SELECT COUNT([DealerComplaintId]) AS [TotalRecord]
		FROM [DealerComplaintData]
	)
SELECT A.[DealerComplaintId],
       A.[CollectDealTransactionCode],
	   A.[SellingAccountName],
	   A.[SellingAccountPhone],
	   A.[BuyingAccountName],
	   A.[BuyingAccountPhone],
	   A.[DealerName],
	   A.[DealerPhone],
	   A.[ComplaintContent],
	   A.[RepliedContent],
	   A.[CreatedTime],
	   B.[TotalRecord]
FROM [DealerComplaintData] A CROSS JOIN [TotalRecord] B
ORDER BY A.[CreatedTime] DESC OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY
