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
            return maxId;
        }

        public async Task<int> AddTaskAsync(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var maxId = (await table.Find(x => x.Id > 0).SortByDescending(x => x.Id).Skip(0).Limit(1).FirstOrDefaultAsync())?.Id ?? 0;
            var dbModel = _mapper.Map<DB_Task>(model);
            dbModel.Id = maxId + 1;
            dbModel.CreateTime = DateTime.Now;
            await table.InsertOneAsync(dbModel);
            return maxId;
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = table.Find(Builders<DB_Task>.Filter.Empty).ToList();
            return taskList.Select(x => new TaskSimpleViewModel
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = await table.Find(Builders<DB_Task>.Filter.Empty).ToListAsync();
            return taskList.Select(x => new TaskSimpleViewModel
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
            }).ToList();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = table.Find(Builders<DB_Task>.Filter.Empty).ToList();
            return taskList.Select(x => new TaskListItemViewModel
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
                CompleteTime = x.CompleteTime,
                CreateTime = x.CreateTime,
                CronExpression = x.CronExpression,
                Description = x.Description,
                Status = x.Status,
            }).ToList();
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = await table.Find(Builders<DB_Task>.Filter.Empty).ToListAsync();
            return taskList.Select(x => new TaskListItemViewModel
            {
                Id = x.Id,
                RootUrl = x.RootUrl,
                SpiderId = x.SpiderId,
                CompleteTime = x.CompleteTime,
                CreateTime = x.CreateTime,
                CronExpression = x.CronExpression,
                Description = x.Description,
                Status = x.Status,
            }).ToList();
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            table.UpdateMany(x => x.Id == taskId, Builders<DB_Task>.Update.Set(x => x.Status, taskStatus));
        }

        public async Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            await table.UpdateManyAsync(x => x.Id == taskId, Builders<DB_Task>.Update.Set(x => x.Status, taskStatus));
        }

        public void UpdateTask(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            table.ReplaceOne(x => x.Id == model.Id,  _mapper.Map<DB_Task>(model));
        }

        public async Task UpdateTaskAsync(TaskEditDto model)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            await table.ReplaceOneAsync(x => x.Id == model.Id,  _mapper.Map<DB_Task>(model));
        }
    }
}
