UPDATE [CollectingRequest] 
SET [Status] = @CancelBySystemStatus, 
	[UpdatedBy] = @UpdatedBy, 
	[UpdatedTime] = @DateTimeNow
WHERE [CollectingRequestDate] = @DateNow AND
	  [Status] = @ApprovedStatus