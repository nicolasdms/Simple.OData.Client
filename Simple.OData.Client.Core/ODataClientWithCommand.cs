﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Simple.OData.Client
{
    // ALthough ODataClientWithCommand is never instantiated directly (only via IClientWithCommand interface)
    // it's declared as public in order to resolve problem when it is used with dynamic C#
    // For the same reason ODataCommand is also declared as public
    // More: http://bloggingabout.net/blogs/vagif/archive/2013/08/05/we-need-better-interoperability-between-dynamic-and-statically-compiled-c.aspx

    public partial class ODataClientWithCommand : IClientWithCommand
    {
        private readonly ODataClient _client;
        private readonly ISchema _schema;
        private readonly ODataCommand _command;

        public ODataClientWithCommand(ODataClient client, ISchema schema, ODataCommand parent = null)
        {
            _client = client;
            _schema = schema;
            _command = new ODataCommand(this, parent);
        }

        public ISchema Schema
        {
            get { return _schema; }
        }

        public string CommandText
        {
            get { return _command.ToString(); }
        }

        public ODataClientWithCommand Link(ODataCommand command, string linkName)
        {
            var linkedClient = new ODataClientWithCommand(_client, _schema, command);
            linkedClient._command.Link(linkName);
            return linkedClient;
        }

        public IClientWithCommand For(string collectionName)
        {
            return _command.For(collectionName);
        }

        public IClientWithCommand For<T>()
        {
            return _command.For(typeof(T).Name);
        }

        public IClientWithCommand As(string derivedCollectionName)
        {
            return _command.As(derivedCollectionName);
        }

        public IClientWithCommand As<T>()
        {
            return _command.As(typeof(T).Name);
        }

        public IClientWithCommand Key(params object[] key)
        {
            return _command.Key(key);
        }

        public IClientWithCommand Key(IEnumerable<object> key)
        {
            return _command.Key(key);
        }

        public IClientWithCommand Key(IDictionary<string, object> key)
        {
            return _command.Key(key);
        }

        public IClientWithCommand Filter(string filter)
        {
            return _command.Filter(filter);
        }

        public IClientWithCommand Filter(FilterExpression expression)
        {
            return _command.Filter(expression);
        }

        public IClientWithCommand Filter<T>(Expression<Func<T, bool>> expression)
        {
            return _command.Filter(expression);
        }

        public IClientWithCommand Skip(int count)
        {
            return _command.Skip(count);
        }

        public IClientWithCommand Top(int count)
        {
            return _command.Top(count);
        }

        public IClientWithCommand Expand(IEnumerable<string> associations)
        {
            return _command.Expand(associations);
        }

        public IClientWithCommand Expand(params string[] associations)
        {
            return _command.Expand(associations);
        }

        public IClientWithCommand Select(IEnumerable<string> columns)
        {
            return _command.Select(columns);
        }

        public IClientWithCommand Select(params string[] columns)
        {
            return _command.Select(columns);
        }

        public IClientWithCommand OrderBy(IEnumerable<KeyValuePair<string,bool>> columns)
        {
            return _command.OrderBy(columns);
        }

        public IClientWithCommand OrderBy(params string[] columns)
        {
            return _command.OrderBy(columns);
        }

        public IClientWithCommand OrderByDescending(params string[] columns)
        {
            return _command.OrderByDescending(columns);
        }

        public IClientWithCommand Count()
        {
            return _command.Count();
        }

        public IClientWithCommand Set(object value)
        {
            return _command.Set(value);
        }

        public IClientWithCommand Set(IDictionary<string, object> value)
        {
            return _command.Set(value);
        }

        public IClientWithCommand Function(string functionName)
        {
            return _command.Function(functionName);
        }

        public IClientWithCommand Parameters(IDictionary<string, object> parameters)
        {
            return _command.Parameters(parameters);
        }

        public IClientWithCommand NavigateTo(string linkName)
        {
            return _command.NavigateTo(linkName);
        }

        public IClientWithCommand NavigateTo<T>()
        {
            return _command.NavigateTo(typeof(T).Name);
        }

        public bool FilterIsKey
        {
            get { return _command.FilterIsKey; }
        }

        public IDictionary<string, object> FilterAsKey
        {
            get { return _command.FilterAsKey; }
        }

        public IDictionary<string, object> NewValues
        {
            get { return _command.EntryData; }
        }
    }
}
