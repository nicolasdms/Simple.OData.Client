﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.OData.Client
{
    class BatchRequestRunner : RequestRunner
    {
        public override Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(HttpRequest request, bool scalarResult)
        {
            return Utils.EmptyTask<IEnumerable<IDictionary<string, object>>>.Task;
        }

        public override Task<Tuple<IEnumerable<IDictionary<string, object>>, int>> FindEntriesWithCountAsync(HttpRequest request, bool scalarResult)
        {
            return Utils.EmptyTask<Tuple<IEnumerable<IDictionary<string, object>>, int>>.Task;
        }

        public override Task<IDictionary<string, object>> GetEntryAsync(HttpRequest request)
        {
            return Utils.EmptyTask<IDictionary<string, object>>.Task;
        }

        public override async Task<IDictionary<string, object>> InsertEntryAsync(HttpRequest request)
        {
            return await Utils.GetTaskFromResult(request.EntryData);
        }

        public override async Task<IDictionary<string, object>> UpdateEntryAsync(HttpRequest request)
        {
            return await Utils.GetTaskFromResult(request.EntryData);
        }

        public override Task DeleteEntryAsync(HttpRequest request)
        {
            return Utils.EmptyVoidTask.Task;
        }

        public override Task<IEnumerable<IDictionary<string, object>>> ExecuteFunctionAsync(HttpRequest request)
        {
            return Utils.EmptyTask<IEnumerable<IDictionary<string, object>>>.Task;
        }
    }
}
