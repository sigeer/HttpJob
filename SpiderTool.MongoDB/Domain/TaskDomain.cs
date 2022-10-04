using AutoMapper;
using MongoDB.Driver;
using SpiderTool.DataBase;
using SpiderTool.Dto.Tasks;
using SpiderTool.IDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public Task<int> AddTaskAsync(TaskEditDto model)
        {
            throw new NotImplementedException();
        }

        public List<TaskSimpleViewModel> GetTaskHistoryList()
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskSimpleViewModel>> GetTaskHistoryListAsync()
        {
            throw new NotImplementedException();
        }

        public List<TaskListItemViewModel> GetTaskList()
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskListItemViewModel>> GetTaskListAsync()
        {
            throw new NotImplementedException();
        }

        public void SetTaskStatus(int taskId, int taskStatus)
        {
            throw new NotImplementedException();
        }

        public Task SetTaskStatusAsync(int taskId, int taskStatus)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(TaskEditDto model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(TaskEditDto model)
        {
            throw new NotImplementedException();
        }
    }
}
