SELECT SUM([Total]) AS TotalCollecting, 
       SUM([TransactionServiceFee]) AS TotalFee,
	   SUM([BonusAmount]) AS BonusAmount,
	   COUNT([Id]) AS NumOfCompletedTrans
FROM [CollectDealTransaction]
WHERE [DealerAccountId] = @DealerId AND
	  [CreatedTime] >= @FromDate AND
	  [CreatedTime] <= @ToDate