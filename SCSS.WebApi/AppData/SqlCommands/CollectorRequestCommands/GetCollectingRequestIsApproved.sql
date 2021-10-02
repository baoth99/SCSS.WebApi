;WITH SellerAccounts AS (
		SELECT [Id] AS SellerId, 
		       [DeviceId] AS SellerDeviceId 
		FROM [Account] WHERE [RoleId] = 'b603d531-12d7-eb11-aae9-062fc87b306c'
      ),
      CollectorAccounts AS (
		SELECT [Id] AS CollectorId, 
		       [DeviceId] AS CollectorDeviceId 
		FROM [Account] WHERE [RoleId] = '37b2c93b-12d7-eb11-aae9-062fc87b306c'
	  )
	SELECT A.Id AS CollectingRequestID, 
		   A.SellerAccountId, 
		   B.SellerDeviceId, 
		   A.CollectorAccountId, 
		   C.CollectorDeviceId
	FROM [CollectingRequest] A JOIN [SellerAccounts] B ON A.SellerAccountId = B.SellerId
							   JOIN [CollectorAccounts] C ON A.CollectorAccountId = C.CollectorId
	WHERE A.[CollectingRequestDate] = @DateNow AND
	      A.[Status] = @CollectingRequestStatus