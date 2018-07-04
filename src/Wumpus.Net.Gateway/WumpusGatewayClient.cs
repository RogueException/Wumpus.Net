﻿using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Wumpus
{
    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
        Disconnecting
    }

    public class WumpusGatewayClient : IDisposable
    {
        private SemaphoreSlim _stateLock;
        private ClientWebSocket _client;
        private ConnectionState _state;
        private Task _connectionTask;
        private CancellationTokenSource _connectionCts;

        public WumpusGatewayClient()
        {
            _client = new ClientWebSocket();
            _stateLock = new SemaphoreSlim(1, 1);
            _connectionTask = Task.CompletedTask;
            _connectionCts = new CancellationTokenSource();
        }

        public async Task ConnectAsync(string url, int shardId, int shardNum)
        {
            TaskCompletionSource<bool> connectResult;
            await _stateLock.WaitAsync().ConfigureAwait(false);
            try
            {
                await StopAsync().ConfigureAwait(false);

                _connectionCts = new CancellationTokenSource();
                connectResult = new TaskCompletionSource<bool>();
                _connectionTask = RunAsync(url, shardId, shardNum, _connectionCts.Token, connectResult);
            }
            finally
            {
                _stateLock.Release();
            }

            // Must be outside of stateLock to make sure DisconnectAsync can be called mid-connecting
            await connectResult.Task.ConfigureAwait(false);
        }
        public async Task DisconnectAsync()
        {
            await _stateLock.WaitAsync().ConfigureAwait(false);
            try
            {
                await StopAsync().ConfigureAwait(false);
            }
            finally
            {
                _stateLock.Release();
            }
        }

        private async Task RunAsync(string url, int shardId, int shardNum, CancellationToken cancelToken, TaskCompletionSource<bool> connectResult)
        {
            try
            {
                _state = ConnectionState.Connecting;

                var uri = new Uri(url);
                await _client.ConnectAsync(uri, cancelToken).ConfigureAwait(false);

                _state = ConnectionState.Connected;
                connectResult.SetResult(true);
            }
            catch (Exception ex)
            {
                connectResult.SetException(ex);
                throw;
            }

            var receiveTask = RunReceiveAsync(cancelToken);
            var sendTask = RunSendAsync(cancelToken);
            var tasks = new Task[] { receiveTask, sendTask };
            await Task.WhenAny(tasks).ConfigureAwait(false);

            // Either receive or send completed, meaning an exception occurred or cancelToken was cancelled
            _state = ConnectionState.Disconnecting;

            // Wait for the other task to complete
            _connectionCts?.Cancel();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            // receiveTask and sendTask must have completed before we can send/receive from a different thread
            try { await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None); }
            catch { } // We don't actually care if sending a close msg fails

            _state = ConnectionState.Disconnected;
        }

        private async Task StopAsync()
        {
            _connectionCts?.Cancel(); // Cancel any connection attempts or active connections

            try { await _connectionTask.ConfigureAwait(false); } // Wait for current connection to complete
            catch (Exception) { } // We don't care about exceptions here, only that the task completed

            // Double check that the connection task terminated successfully
            var state = _state;
            if (state != ConnectionState.Disconnected)
                throw new InvalidOperationException($"Client did not successfully disconnect (State = {state}).");
        }

        private async Task RunReceiveAsync(CancellationToken cancelToken)
        {
            try
            {
                while (!cancelToken.IsCancellationRequested)
                {
                    _receiveBuffer.Clear();

                    WebSocketReceiveResult result;
                    do
                    {
                        var buffer = _receiveBuffer.GetSegment(10 * 1024); // 10 KB
                        result = await _client.ReceiveAsync(buffer, cancelToken);
                        _receiveBuffer.Advance(result.Count);

                        if (result.CloseStatus != null)
                        {
                            if (!string.IsNullOrEmpty(result.CloseStatusDescription))
                                throw new Exception($"WebSocket was closed: {result.CloseStatus.Value} ({result.CloseStatusDescription})"); // TODO: Exception type?
                            else
                                throw new Exception($"WebSocket was closed: {result.CloseStatus.Value}"); // TODO: Exception type?
                        }
                    }
                    while (!result.EndOfMessage);

                    var frame = _serializer.Read<GatewayPayload>(_receiveBuffer.AsReadOnlySpan());
                    await HandleFrameAsync(frame).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException) { } // Ignore
            catch (Exception) { } // TODO: Log
        }

        private async Task RunSendAsync(CancellationToken cancelToken)
        {
            try
            {
                while (!cancelToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancelToken);
                }
            }
            catch (OperationCanceledException) { } // Ignore
            catch (Exception) { } // TODO: Log
        }

        private async Task HandleFrameAsync(GatewayPayload frame)
        {
            //switch (frame.Operation)
            //{
            //}
            await Task.CompletedTask;
        }

        private async Task HandleDispatchEventAsync(GatewayPayload frame)
        {
            //switch (frame.DispatchType)
            //{
            //}
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            StopAsync().Wait();
            _client.Dispose();
        }
    }
}
