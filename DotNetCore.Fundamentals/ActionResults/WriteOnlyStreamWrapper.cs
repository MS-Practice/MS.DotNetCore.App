using System;
using System.IO;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.ActionResults {
    public class WriteOnlyStreamWrapper : Stream {
        private readonly Stream _stream;
        private long _position;
        public WriteOnlyStreamWrapper(Stream stream) {
            _stream = stream;
        }

        public override long Position {
            get { return _position; }
            set { throw new NotSupportedException(); }
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override void Write(byte[] buffer, int offset, int count) {
            _position += count;
            _stream.Write(buffer, offset, count);
        }
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            _position += count;
            return _stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult) => _stream.EndWrite(asyncResult);
        public override void WriteByte(byte value) {
            _position += 1;
            _stream.WriteByte(value);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken) {
            _position += count;
            return _stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void Flush() {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value) {
            _stream.SetLength(value);
        }
    }
}