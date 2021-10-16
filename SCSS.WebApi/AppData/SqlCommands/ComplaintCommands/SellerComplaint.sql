;WITH SellerComplaintData AS (
	SELECT A.[Id] AS [SellerComplaintId],
		   C.[CollectingRequestCode] AS [CollectingRequestCode],
		   A.[ComplaintContent] AS [ComplaintContent],
		   D.[Name] AS [SellingAccountName],
		   D.[Phone] AS [SellingAccountPhone],
		   E.[Name] AS [BuyingAccountName],
		   E.[Phone] AS [BuyingAccountPhone],
		   A.[AdminReply] AS [RepliedContent],
		   A.[CreatedTime] AS [CreatedTime]
	FROM [SellerComplaint] A JOIN [Complaint] B ON A.[ComplaintId] = B.[Id] 
						     JOIN [CollectingRequest] C ON B.[CollectingRequestId] = C.[Id]
							 JOIN [Account] D ON C.[SellerAccountId] = D.[Id]
							 JOIN [Account] E ON C.[CollectorAccountId] = E.[Id]
	WHERE (@SellerPhone IS NULL OR D.[Phone] LIKE '%' + @SellerPhone + '%') AND
		  (@SellerName IS NULL OR D.[Name] LIKE '%' + @SellerName + '%')
	),
	TotalRecord AS (
		SELECT COUNT([SellerComplaintId]) AS [TotalRecord]
		FROM [SellerComplaintData]
	)
SELECT A.[SellerComplaintId],
	   A.[CollectingRequestCode],
	   A.[ComplaintContent],
	   A.[SellingAccountName],
	   A.[SellingAccountPhone],
	   A.[BuyingAccountName],
	   A.[BuyingAccountPhone],
	   A.[RepliedContent],
	   A.[CreatedTime],
	   B.[TotalRecord]
FROM [SellerComplaintData] A CROSS JOIN [TotalRecord] B
ORDER BY A.[CreatedTime] DESC OFFSET @Page ROWS FETCH NEXT @PageSize ROWS ONLY


