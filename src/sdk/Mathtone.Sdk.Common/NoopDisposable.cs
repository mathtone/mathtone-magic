using Mathtone.Sdk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Common {

	//public class Disposer : IDisposable {
		
	//	readonly IDisposable[] _disposables;

	//	public Disposer(params IDisposable[] disposables) {
	//		_disposables = disposables;
	//	}

	//	private bool disposedValue;

	//	protected virtual void Dispose(bool disposing) {
	//		if (!disposedValue) {
	//			if (disposing) {
	//				_disposables.ForEach(d => d.Dispose());
	//			}
	//			disposedValue = true;
	//		}
	//	}

	//	public void Dispose() {
	//		Dispose(disposing: true);
	//		GC.SuppressFinalize(this);
	//	}
	//}

	//public interface ILocalTimeService : IDateTimeService {
	//	new DateTimeOffset Now { get; }
	//	new DateTimeOffset UtcNow { get; }
	//}

	//public interface IDateTimeService {
	//	DateTime Now { get; }
	//	DateTime UtcNow { get; }
	//}

	//public class TimeService : ILocalTimeService {
	//	public DateTimeOffset Now { get; }
	//	public DateTimeOffset UtcNow { get; }
	//	DateTime IDateTimeService.Now { get; }
	//	DateTime IDateTimeService.UtcNow { get; }
	//}
}