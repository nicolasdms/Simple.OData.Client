﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.OData.Client
{
    /// <summary>
    /// Performs batch processing of OData requests by grouping multiple operations in a single HTTP POST request in accordance with OData protocol
    /// </summary>
    public class ODataBatch : IDisposable
    {
        private bool _active;
        internal ODataClientSettings Settings { get; set; }
        internal BatchRequestBuilder RequestBuilder { get; set; }
        internal BatchRequestRunner RequestRunner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataBatch"/> class.
        /// </summary>
        /// <param name="urlBase">The URL base.</param>
        public ODataBatch(string urlBase)
            : this (new ODataClientSettings { UrlBase = urlBase })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataBatch"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public ODataBatch(ODataClientSettings settings)
        {
            this.Settings = settings;
            this.RequestBuilder = new BatchRequestBuilder(Session.FromUrl(this.Settings.UrlBase, this.Settings.Credentials));
            this.RequestRunner = new BatchRequestRunner();

            //this.RequestBuilder.BeginBatch();
            _active = true;
        }

        /// <summary>
        /// Cancels pending OData batch and releases all resources used by <see cref="ODataBatch"/>.
        /// </summary>
        public void Dispose()
        {
            //if (_active)
            //    this.RequestBuilder.CancelBatch();
            _active = false;
        }

        /// <summary>
        /// Completes the OData batch by submitting pending requests to the OData service.
        /// </summary>
        /// <returns></returns>
        public Task CompleteAsync()
        {
            return CompleteAsync(CancellationToken.None);
        }

        /// <summary>
        /// Completes the OData batch by submitting pending requests to the OData service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task CompleteAsync(CancellationToken cancellationToken)
        {
            var requestMessage = await this.RequestBuilder.CompleteBatchAsync();
            using (var response = await this.RequestRunner.ExecuteRequestAsync(
                this.RequestBuilder.CreateBatchRequest(), 
                requestMessage,
                cancellationToken))
            {
                await ParseResponseAsync(response);
            }
            _active = false;
        }

        /// <summary>
        /// Cancels the pending OData batch.
        /// </summary>
        public void Cancel()
        {
            _active = false;
        }

        private async Task ParseResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var batchMarker = Regex.Match(content, @"--batchresponse_[a-zA-Z0-9\-]+").Value;
            var batchResponse = content.Split(new string[] {batchMarker}, StringSplitOptions.None)[1];
            var changesetMarker = Regex.Match(batchResponse, @"--changesetresponse_[a-zA-Z0-9\-]+").Value;
            var changesetResponses =
                batchResponse.Split(new string[] {changesetMarker}, StringSplitOptions.None).ToList();
            changesetResponses = changesetResponses.Skip(1).Take(changesetResponses.Count - 2).ToList();
            foreach (var changesetResponse in changesetResponses)
            {
                var match = Regex.Match(changesetResponse, @"HTTP/[0-9\.]+\s+([0-9]+)\s+(.+)\n");
                var statusCode = int.Parse(match.Groups[1].Value);
                var message = match.Groups[2].Value;
                if (statusCode >= 400)
                {
                    throw new WebRequestException(message, (HttpStatusCode)statusCode);
                }
            }
        }
    }
}
