UPDATE [CollectingRequest] 
SET [Status] = @CancelBySystemStatus, 
	[UpdatedBy] = @UpdatedBy, 
	[UpdatedTime] = @DateNow
WHERE [CollectingRequestDate] = @DateNow AND
	  ([Status] = @PendingStatus OR [Status] = @ApprovedStatus)