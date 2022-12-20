﻿// ReSharper disable once CheckNamespace

namespace System.Buffers;

internal delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);
