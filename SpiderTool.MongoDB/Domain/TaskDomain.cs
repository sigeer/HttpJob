using AutoMapper;
using MongoDB.Driver;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;

namespace SpiderTool.MongoDB.Domain
{
    public class TaskDomain : ITaskDomain
    {
        readonly IMongoDatabase _db;
        readonly IMapper _mapper;

        public TaskDomain(IMongoClient client, IMapper mapper)
        {
            _db = client.GetDatabase("spider");
            _mapper = mapper;
        }

        public int AddTask(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var maxId = table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefault()?.Id ?? 0;
            var dbModel = _mapper.Map<DB_Task>(model);
            dbModel.Id = maxId + 1;
            dbModel.CreateTime = DateTime.Now;
            table.InsertOne(dbModel);
            return dbModel.Id;
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var maxId = (await table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefaultAsync())?.Id ?? 0;
            var dbModel = _mapper.Map<DB_Task>(model);
            dbModel.Id = maxId + 1;
            dbModel.CreateTime = DateTime.Now;
            await table.InsertOneAsync(dbModel);
            return dbModel.Id;
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = table.Find(Builders<DB_Task>.Filter.Where(x => x.RootUrl != "" && x.RootUrl != null))
                .Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime))
                .ToList();
            return taskList.Select(x => new TaskSimpleViewModel
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).DistinctBy(x => x.RootUrl).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = await table.Find(Builders<DB_Task>.Filter.Where(x => x.RootUrl != "" && x.RootUrl != null))
                .Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime))
                .ToListAsync();
            return taskList.Select(x => new TaskSimpleViewModel
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).DistinctBy(x => x.RootUrl).ToList();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = table.Find(Builders<DB_Task>.Filter.Empty).Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime)).Limit(10).ToList();
            return _mapper.Map<List<TaskListItemViewModel>>(taskList);
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = await table.Find(Builders<DB_Task>.Filter.Empty).Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime)).Limit(10).ToListAsync();
            return _mapper.Map<List<TaskListItemViewModel>>(taskList);
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var updateDifination = Builders<DB_Task>.Update.Set(x => x.Status, taskStatus);
            if (taskStatus == (int)TaskType.Completed)
                updateDifination = updateDifination.Set(x => x.CompleteTime, DateTime.Now);
            table.UpdateMany(x => x.Id == taskId, updateDifination);
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var updateDifination = Builders<DB_Task>.Update.Set(x => x.Status, taskStatus);
            if (taskStatus == (int)TaskType.Completed)
                updateDifination = updateDifination.Set(x => x.CompleteTime, DateTime.Now);
            await table.UpdateManyAsync(x => x.Id == taskId, updateDifination);
        }

        public void UpdateTask(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var updateDifination = Builders<DB_Task>.Update
                .Set(x => x.Description, model.Description)
                .Set(x => x.Status, model.Status);
            table.UpdateMany(x => x.Id == model.Id, updateDifination);
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var updateDifination = Builders<DB_Task>.Update
                .Set(x => x.Description, model.Description)
                .Set(x => x.Status, model.Status);
            await table.UpdateManyAsync(x => x.Id == model.Id, updateDifination);
        }
    }
}
