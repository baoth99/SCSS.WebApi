using SCSS.QueueEngine.QueueModels;
using SCSS.QueueEngine.QueueRepositories;


namespace SCSS.QueueEngine.QueueEngines
{
    public interface IQueueEngineFactory
    {
        #region Queue Repositories

        IQueueRepository<string> StringEngineRepo { get; }

        IQueueRepository<CollectingRequestReminderQueueModel> CollectingRequestReminderQueueRepos { get; }

        #endregion
    }
}
