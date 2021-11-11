;WITH TotalCollecting AS (
		SELECT SUM(([Total] - [TransactionServiceFee])) AS TotalCollecting
		FROM [SellCollectTransaction] 
		WHERE [CreatedBy] = @CollectorId AND
		      CONVERT(DATE, [CreatedTime]) >= @FromDate AND
			  CONVERT(DATE, [CreatedTime]) <= @ToDate
	),
	  TotalSale AS (
	    SELECT SUM(([Total] - [TransactionServiceFee]) + [BonusAmount]) AS TotalSale
		FROM [CollectDealTransaction]
		WHERE [CollectorAccountId] = @CollectorId AND
			  CONVERT(DATE, [CreatedTime]) >= @FromDate AND
			  CONVERT(DATE, [CreatedTime]) <= @ToDate
	),
      TotalCompletedCR AS (
		SELECT COUNT([Id]) AS TotalCompletedCR
		FROM [CollectingRequest]
		WHERE [CollectorAccountId] = @CollectorId AND
		      [Status] = @CompleteStatus AND
			  CONVERT(DATE, [UpdatedTime]) >= @FromDate AND
			  CONVERT(DATE, [UpdatedTime]) <= @ToDate
	),
	  TotalCancelCR AS (
		SELECT COUNT([Id]) AS TotalCancelCR
		FROM [CollectingRequest]
		WHERE [CollectorAccountId] = @CollectorId AND
			  ([Status] = @CancelByCollectorStatus OR [Status] = @CancelBySystemStatus) AND
			  CONVERT(DATE, [UpdatedTime]) >= @FromDate AND
			  CONVERT(DATE, [UpdatedTime]) <= @ToDate
	)
SELECT A.TotalCollecting, 
	   B.TotalSale, 
	   C.TotalCompletedCR,
	   D.TotalCancelCR
FROM [TotalCollecting] A CROSS JOIN [TotalSale] B
						 CROSS JOIN [TotalCompletedCR] C
						 CROSS JOIN [TotalCancelCR] D