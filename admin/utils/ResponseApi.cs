using System;
using System.Collections.Generic;

    public class ResponseApi<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = "";
        public T? Data { get; private set; }
        public List<string>? Errors { get; private set; }
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        private ResponseApi() { }
        public class Builder
        {
            private bool success;
            private string message = "";
            private T? data;
            private List<string>? errors;
            private DateTime timestamp = DateTime.UtcNow;
            public Builder SetSuccess(bool success)
            {
                this.success = success;
                return this;
            }
            public Builder SetMessage(string message)
            {
                this.message = message;
                return this;
            }
            public Builder SetData(T data)
            {
                this.data = data;
                return this;
            }
            public Builder SetErrors(List<string> errors)
            {
                this.errors = errors;
                return this;
            }
            public Builder SetTimestamp(DateTime timestamp)
            {
                this.timestamp = timestamp;
                return this;
            }

            public ResponseApi<T> Build()
            {
                return new ResponseApi<T>
                {
                    Success = this.success,
                    Message = this.message,
                    Data = this.data,
                    Errors = this.errors,
                    Timestamp = this.timestamp
                };
            }
        }
    }


