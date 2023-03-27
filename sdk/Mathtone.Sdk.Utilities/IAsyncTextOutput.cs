using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Utilities {
	//public interface IAsyncTextOutput : ITextOutput {
	//	public Task WriteLineAsync(string text) => WriteAsync(text + Environment.NewLine);
	//	public Task WriteAsync(string text);
	//}

	//public interface ITextOutput {
	//	public void Write(string text);
	//	public void WriteLine(string text) => Write(text + Environment.NewLine);
	//}

	//public class TextOutputAdapter : IAsyncTextOutput {

	//	readonly Func<string, Task> _asyncHandler;
	//	readonly Action<string> _blockingHandler;

	//	public TextOutputAdapter(Func<string, Task> asyncHandler) :
	//		this(asyncHandler, async v => await asyncHandler(v)) { }

	//	public TextOutputAdapter(Action<string> blockingHandler) :
	//		this(t => Task.Run(() => blockingHandler(t)), blockingHandler) { }

	//	public TextOutputAdapter(Func<string, Task> asyncHandler, Action<string> blockingHandler) {
	//		_asyncHandler = asyncHandler;
	//		_blockingHandler = blockingHandler;
	//	}

	//	public Task WriteAsync(string text) => _asyncHandler(text);
	//	public void Write(string text) => _blockingHandler(text);
	//}
}
