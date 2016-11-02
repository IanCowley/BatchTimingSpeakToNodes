using System;
using System.Threading;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Auth;

namespace BatchTimingSpeakToNodes
{
    public class TimedNodeCounter
    {
        readonly string _batchAccountName;
        readonly string _batchAccountKey;
        readonly string _poolId;
        readonly int _targetDedicatedNodes;
        TimeOnator _timer = null;

        public TimedNodeCounter(string batchAccountName, string batchAccountKey, string poolId, int targetDedicatedNodes)
        {
            _batchAccountName = batchAccountName;
            _batchAccountKey = batchAccountKey;
            _poolId = poolId;
            _targetDedicatedNodes = targetDedicatedNodes;
        }

        public void Start()
        {
            _timer = new TimeOnator();
            _timer.WriteTimedNote($"Connecting to batch account {_batchAccountName}");

            using (var batchClient = GetBatchClient())
            {
                _timer.WriteTimedNote($"Get batch pool {_batchAccountName}");
                var pool = batchClient.PoolOperations.GetPool(_poolId);

                _timer.WriteTimedNote($"Sending scale up to pool {_poolId} and target dedicated {_targetDedicatedNodes}");
                pool.Resize(targetDedicated:_targetDedicatedNodes);
            }
        }

        public void StartListing()
        {
            _timer.WriteTimedNote($"Connecting to batch account {_batchAccountName}");

            using (var batchClient = GetBatchClient())
            {
                _timer.WriteTimedNote($"Get batch pool {_batchAccountName}");
                var pool = batchClient.PoolOperations.GetPool(_poolId);

                _timer.WriteTimedNote($"Starting to count nodes in pool");
                string nodeDetail = string.Empty;
                int nodeIndex = 0;
                    
                var nodes = pool.ListComputeNodes(new ODATADetailLevel(selectClause: "id,runningTasksCount"), null);

                nodes.ForEachAsync(node =>
                {
                    nodeIndex++;
                    nodeDetail = nodeDetail + $"{nodeIndex}, {node.RunningTasksCount}\r\n";
                })
                .Wait();

                _timer.WriteTimedNote($"Count complete");
                Console.WriteLine("NodeIndex, TaskCount \r\n");
                Console.WriteLine(nodeDetail);
            }
        }

        public void ScaleDown()
        {
            _timer.WriteTimedNote($"Connecting to batch account {_batchAccountName}");

            using (var batchClient = GetBatchClient())
            {
                _timer.WriteTimedNote($"Get batch pool {_batchAccountName}");
                var pool = batchClient.PoolOperations.GetPool(_poolId);
                _timer.WriteTimedNote($"Starting scale down for pool id {_poolId}");
                pool.Resize(targetDedicated: 0);
            }
        }

        BatchClient GetBatchClient()
        {
            var credentials = new BatchSharedKeyCredentials($"https://{_batchAccountName}.northeurope.batch.azure.com", _batchAccountName, _batchAccountKey);
            return BatchClient.Open(credentials);
        }
    }
}
