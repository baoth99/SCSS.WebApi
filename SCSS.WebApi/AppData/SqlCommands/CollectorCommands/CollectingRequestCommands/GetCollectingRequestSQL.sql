SELECT [Id], 
	   [CollectingRequestCode], 
	   [CollectingRequestDate], 
	   [TimeFrom], 
	   [TimeTo], 
	   [LocationId],
	   [SellerAccountId], 
	   [CollectorAccountId], 
	   [IsBulky], 
	   [ScrapImageUrl], 
	   [Note], 
	   [CancelReason], 
	   [Status],
	   [IsDeleted]
FROM [CollectingRequest]
WHERE [Id] = @CollectingRequestId AND
	  [Status] = @CollectingRequestStatus AND
	  [CollectorAccountId] = NULL