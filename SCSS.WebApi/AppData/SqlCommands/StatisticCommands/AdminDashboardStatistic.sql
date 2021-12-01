;WITH AmountCompletedRequest AS (
		SELECT COUNT([Id]) AS AmountCompletedRequest
		FROM [CollectingRequest]
		WHERE [Status] = @CompletedCollectingRequest AND
			  CONVERT(DATE, [UpdatedTime]) >= @FromDate AND
			  CONVERT(DATE, [UpdatedTime]) <= @ToDate
	),
	AmountCancelRequestByUser AS (
		SELECT COUNT([Id]) AS AmountCancelRequestByUser
		FROM [CollectingRequest]
		WHERE [Status] IN @CancelCollectingRequestByUser AND
			  CONVERT(DATE, [UpdatedTime]) >= @FromDate AND
			  CONVERT(DATE, [UpdatedTime]) <= @ToDate
	),
	AmountCancelRequestBySystem AS (
		SELECT COUNT([Id]) AS AmountCancelRequestBySystem
		FROM [CollectingRequest]
		WHERE [Status] = @CancelCollectingRequestBySystem AND
			  CONVERT(DATE, [UpdatedTime]) >= @FromDate AND
			  CONVERT(DATE, [UpdatedTime]) <= @ToDate
	),
	AmountCollectDealTransaction AS (
		SELECT COUNT([Id]) AS AmountCollectDealTransaction
		FROM [CollectDealTransaction]
		WHERE CONVERT(DATE, [CreatedTime]) >= @FromDate AND
			  CONVERT(DATE, [CreatedTime]) <= @ToDate
	)
SELECT A.AmountCompletedRequest,
	   B.AmountCancelRequestByUser,
	   C.AmountCancelRequestBySystem,
	   D.AmountCollectDealTransaction
FROM [AmountCompletedRequest] A CROSS JOIN [AmountCancelRequestByUser] B
								CROSS JOIN [AmountCancelRequestBySystem] C
								CROSS JOIN [AmountCollectDealTransaction] D
	
