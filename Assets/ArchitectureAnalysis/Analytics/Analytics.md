## Analytics

`ILogSink` decouples logging from its destination, other implementations could be `FileLogSink`,
which appends logs to a local file, or `WebServiceLogSink`, which sends them to an analytics backend.

At bootstrap the desired implementation is injected into `MovementSystemLoggingDecorator` and
`ActionSystemLoggingDecorator`, just like `UnityConsoleLogSink`.

Using dependency injection and keeps the decorators unchanged.

In the project I'm only logging a basic log for each movement/action, this could easily be extendend with
additional information about the action outcome, device, profile, etc.