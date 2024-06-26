﻿using AutoMapper;
using MongoDB.Driver;
using SpiderTool.Data.Constants;
using SpiderTool.Data.DataBase;
using SpiderTool.Data.Dto.Tasks;
using SpiderTool.IDomain;

namespace SpiderTool.MongoDB.Domain
{
    public class TaskDomain : ITaskDomain
    {
        readonly IMongoDatabase _db;
        readonly IMapper _mapper;

        public TaskDomain(IMongoClient client, IMapper mapper)
        {
            _db = client.GetDatabase(Constants.DBName);
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
                .Project<TaskSimpleViewModel>(Builders<DB_Task>.Projection.Include(x => x.Id).Include(x => x.RootUrl).Include(x => x.SpiderId))
                .ToList();
            return taskList.DistinctBy(x => x.RootUrl).ToList();
        }

        public async Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = await table.Find(Builders<DB_Task>.Filter.Where(x => x.RootUrl != "" && x.RootUrl != null))
                .Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime))
                .Project<TaskSimpleViewModel>(Builders<DB_Task>.Projection.Include(x => x.Id).Include(x => x.RootUrl).Include(x => x.SpiderId))
                .ToListAsync();
            return taskList.DistinctBy(x => x.RootUrl).ToList();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = table.Find(Builders<DB_Task>.Filter.Empty).Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime)).Limit(GlobalVariable.TaskListMaxCount).ToList();
            return _mapper.Map<List<TaskListItemViewModel>>(taskList);
        }

        public async Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var taskList = await table.Find(Builders<DB_Task>.Filter.Empty).Sort(Builders<DB_Task>.Sort.Descending(x => x.CreateTime)).Limit(GlobalVariable.TaskListMaxCount).ToListAsync();
            return _mapper.Map<List<TaskListItemViewModel>>(taskList);
        }

        public async Task<List<TaskListItemViewModel>> GetTaskPageListAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
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

        public void BulkUpdateTaskStatus(IEnumerable<int> tasks, int taskStatus)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var updateDifination = Builders<DB_Task>.Update
                .Set(x => x.Status, taskStatus);
            table.UpdateMany(x => tasks.Contains(x.Id), updateDifination);
        }

        public async Task BulkUpdateTaskStatusAsync(IEnumerable<int> tasks, int taskStatus)
        {
            var table = _db.GetCollection<DB_Task>(nameof(DB_Task));
            var updateDifination = Builders<DB_Task>.Update
                .Set(x => x.Status, taskStatus);
            await table.UpdateManyAsync(x => tasks.Contains(x.Id), updateDifination);
        }

        public void RemoveTask(int taskId)
        {
            var collection = _db.GetCollection<DB_Task>(nameof(DB_Task));
            collection.DeleteMany(x => x.Id == taskId);
        }

        public async Task RemoveTaskAsync(int taskId)
        {
            var collection = _db.GetCollection<DB_Task>(nameof(DB_Task));
            await collection.DeleteManyAsync(x => x.Id == taskId);
        }
    }
}
