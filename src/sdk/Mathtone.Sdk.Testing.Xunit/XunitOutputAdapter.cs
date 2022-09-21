﻿using Mathtone.Sdk.Common.Utility;
using Xunit.Abstractions;

namespace Mathtone.Sdk.Testing.Xunit {
	public class XunitOutputAdapter : TextOutputAdapter {
		public XunitOutputAdapter(ITestOutputHelper output) :
			base(output.WriteLine) {
		}
	}
}