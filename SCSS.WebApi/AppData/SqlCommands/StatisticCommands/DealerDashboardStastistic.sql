SELECT SUM([Total]) AS TotalCollecting, 
       SUM([TransactionServiceFee]) AS TotalFee,
	   SUM([BonusAmount]) AS BonusAmount,
	   COUNT([Id]) AS NumOfCompletedTrans
FROM [CollectDealTransaction]
WHERE [DealerAccountId] = @DealerId AND
	  CONVERT(DATE, [CreatedTime]) >= @FromDate AND
	  CONVERT(DATE, [CreatedTime]) <= @ToDate